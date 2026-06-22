#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 数据导入工具窗口
/// 策划修改 CSV 后，点击 "Sync Data" 按钮即可同步到 JSON
/// 路径：Window -> Game Data -> Sync CSV to JSON
/// </summary>
public class DataImportWindow : EditorWindow
{
    private static string CardsCSVPath = "Resources/Data/Cards/CardsData.csv";
    private static string EnemiesCSVPath = "Resources/Data/Enemies/EnemiesData.csv";
    private static string BossesCSVPath = "Resources/Data/Enemies/BossesData.csv";
    private static string BossPhasesCSVPath = "Resources/Data/Enemies/BossPhasesData.csv";

    private static string CardsJSONPath = "Resources/Data/Cards/CardsData.json";
    private static string EnemiesJSONPath = "Resources/Data/Enemies/EnemiesData.json";

    [MenuItem("Window/Game Data/Sync CSV to JSON")]
    public static void ShowWindow()
    {
        GetWindow<DataImportWindow>("数据导入工具");
    }

    void OnGUI()
    {
        GUILayout.Label("深渊血契 - 数据同步工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("将 CSV 数据转换为 JSON（供运行时加载）", EditorStyles.helpBox);
        EditorGUILayout.Space();

        // 路径配置
        EditorGUILayout.LabelField("文件路径配置", EditorStyles.boldLabel);
        CardsCSVPath = EditorGUILayout.TextField("卡牌 CSV", CardsCSVPath);
        EnemiesCSVPath = EditorGUILayout.TextField("敌人 CSV", EnemiesCSVPath);
        BossesCSVPath = EditorGUILayout.TextField("BOSS CSV", BossesCSVPath);
        BossPhasesCSVPath = EditorGUILayout.TextField("BOSS阶段 CSV", BossPhasesCSVPath);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("输出路径", EditorStyles.boldLabel);
        CardsJSONPath = EditorGUILayout.TextField("卡牌 JSON", CardsJSONPath);
        EnemiesJSONPath = EditorGUILayout.TextField("敌人 JSON", EnemiesJSONPath);
        EditorGUILayout.Space();

        // 同步按钮
        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("同步所有数据 (Sync All)", GUILayout.Height(40)))
        {
            SyncAllData();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // 分开同步
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("仅同步卡牌"))
        {
            SyncCardsData();
        }
        if (GUILayout.Button("仅同步敌人"))
        {
            SyncEnemiesData();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // 验证按钮
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("数据验证", EditorStyles.boldLabel);
        if (GUILayout.Button("验证卡牌数据"))
        {
            ValidateCards();
        }
        if (GUILayout.Button("验证敌人数值"))
        {
            ValidateEnemies();
        }
        EditorGUILayout.EndVertical();
    }

    private static void SyncAllData()
    {
        SyncCardsData();
        SyncEnemiesData();
        Debug.Log("[DataImport] 所有数据同步完成！");
        EditorUtility.DisplayDialog("同步完成", "所有数据已成功同步到 JSON！", "确定");
    }

    private static void SyncCardsData()
    {
        string fullPath = Path.Combine(Application.dataPath, CardsCSVPath);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[DataImport] 卡牌CSV不存在: {fullPath}");
            return;
        }

        List<CardData> cards = CSVParser.ParseCardCSV(fullPath);
        if (cards.Count == 0)
        {
            Debug.LogError("[DataImport] 卡牌数据为空，同步取消");
            return;
        }

        CardDatabase db = new CardDatabase { cards = cards };
        string json = JsonUtility.ToJson(db, true);

        string outPath = Path.Combine(Application.dataPath, CardsJSONPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
        File.WriteAllText(outPath, json);

        Debug.Log($"[DataImport] 卡牌数据已同步: {cards.Count} 张卡牌 -> {outPath}");
    }

    private static void SyncEnemiesData()
    {
        string fullPath = Path.Combine(Application.dataPath, EnemiesCSVPath);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[DataImport] 敌人CSV不存在: {fullPath}");
            return;
        }

        List<EnemyData> enemies = CSVParser.ParseEnemyCSV(fullPath);
        if (enemies.Count == 0)
        {
            Debug.LogError("[DataImport] 敌人数据为空，同步取消");
            return;
        }

        // 构建数据库
        EnemyDatabase db = new EnemyDatabase
        {
            normalEnemies = enemies.FindAll(e => e.type == EnemyType.Normal),
            eliteEnemies = enemies.FindAll(e => e.type == EnemyType.Elite),
            bosses = new List<BossData>()
        };

        string json = JsonUtility.ToJson(db, true);
        string outPath = Path.Combine(Application.dataPath, EnemiesJSONPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
        File.WriteAllText(outPath, json);

        Debug.Log($"[DataImport] 敌人数值数据已同步: {db.normalEnemies.Count} 普通 + {db.eliteEnemies.Count} 精英 -> {outPath}");
    }

    private static void ValidateCards()
    {
        string fullPath = Path.Combine(Application.dataPath, CardsCSVPath);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[DataImport] 卡牌CSV不存在: {fullPath}");
            return;
        }

        List<CardData> cards = CSVParser.ParseCardCSV(fullPath);
        List<string> errors = new List<string>();

        foreach (CardData card in cards)
        {
            if (string.IsNullOrEmpty(card.id))
                errors.Add($"卡牌缺少ID");
            if (string.IsNullOrEmpty(card.cardName))
                errors.Add($"ID={card.id}: 缺少卡牌名称");
            if (card.cost < 0)
                errors.Add($"ID={card.id}: 费用不能为负数");
            if (card.baseDamage < 0)
                errors.Add($"ID={card.id}: 伤害不能为负数");
            if (card.baseBlock < 0)
                errors.Add($"ID={card.id}: 格挡不能为负数");
        }

        // 检查重复ID
        HashSet<string> ids = new HashSet<string>();
        foreach (CardData card in cards)
        {
            if (ids.Contains(card.id))
                errors.Add($"重复的卡牌ID: {card.id}");
            ids.Add(card.id);
        }

        if (errors.Count > 0)
        {
            foreach (string err in errors)
                Debug.LogError($"[DataImport] {err}");
            EditorUtility.DisplayDialog("验证失败", $"发现 {errors.Count} 个错误，详见Console", "确定");
        }
        else
        {
            Debug.Log($"[DataImport] 卡牌数据验证通过！共 {cards.Count} 张卡牌");
            EditorUtility.DisplayDialog("验证通过", $"卡牌数据验证通过！共 {cards.Count} 张卡牌", "确定");
        }
    }

    private static void ValidateEnemies()
    {
        string fullPath = Path.Combine(Application.dataPath, EnemiesCSVPath);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[DataImport] 敌人CSV不存在: {fullPath}");
            return;
        }

        List<EnemyData> enemies = CSVParser.ParseEnemyCSV(fullPath);
        List<string> errors = new List<string>();

        foreach (EnemyData enemy in enemies)
        {
            if (string.IsNullOrEmpty(enemy.id))
                errors.Add($"敌人缺少ID");
            if (string.IsNullOrEmpty(enemy.enemyName))
                errors.Add($"ID={enemy.id}: 缺少敌人名称");
            if (enemy.maxHp <= 0)
                errors.Add($"ID={enemy.id}: 生命值必须大于0");
            if (enemy.actionPattern == null || enemy.actionPattern.Count == 0)
                errors.Add($"ID={enemy.id}: 缺少行动模式");
        }

        // 检查重复ID
        HashSet<string> ids = new HashSet<string>();
        foreach (EnemyData enemy in enemies)
        {
            if (ids.Contains(enemy.id))
                errors.Add($"重复的敌人ID: {enemy.id}");
            ids.Add(enemy.id);
        }

        if (errors.Count > 0)
        {
            foreach (string err in errors)
                Debug.LogError($"[DataImport] {err}");
            EditorUtility.DisplayDialog("验证失败", $"发现 {errors.Count} 个错误，详见Console", "确定");
        }
        else
        {
            Debug.Log($"[DataImport] 敌人数值验证通过！共 {enemies.Count} 个敌人");
            EditorUtility.DisplayDialog("验证通过", $"敌人数值验证通过！共 {enemies.Count} 个敌人", "确定");
        }
    }
}
#endif
