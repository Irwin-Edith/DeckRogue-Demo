using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人数值数据库加载器
/// 使用方法：
///   EnemyDatabase db = EnemyLoader.Load();
///   List<EnemyData> normals = db.normalEnemies;
///   BossData boss = EnemyLoader.GetBoss("abyss_lord");
/// </summary>
public static class EnemyLoader
{
    private static EnemyDatabase _cachedDatabase;
    private static readonly string EnemiesDataPath = "Data/Enemies/EnemiesData";

    /// <summary>
    /// 加载敌人数值数据库
    /// </summary>
    public static EnemyDatabase Load()
    {
        if (_cachedDatabase != null)
            return _cachedDatabase;

        TextAsset jsonFile = Resources.Load<TextAsset>(EnemiesDataPath);
        if (jsonFile == null)
        {
            Debug.LogError($"[EnemyLoader] 无法加载敌人数值数据文件: {EnemiesDataPath}");
            return null;
        }

        try
        {
            _cachedDatabase = JsonUtility.FromJson<EnemyDatabase>(jsonFile.text);
            Debug.Log($"[EnemyLoader] 成功加载 {_cachedDatabase.normalEnemies.Count} 个普通敌人, " +
                      $"{_cachedDatabase.eliteEnemies.Count} 个精英敌人, {_cachedDatabase.bosses.Count} 个BOSS");
            return _cachedDatabase;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[EnemyLoader] 解析敌人数值数据失败: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 根据ID获取普通敌人
    /// </summary>
    public static EnemyData GetNormalEnemy(string enemyId)
    {
        EnemyDatabase db = Load();
        if (db == null) return null;
        return db.normalEnemies.Find(e => e.id == enemyId);
    }

    /// <summary>
    /// 根据ID获取精英敌人
    /// </summary>
    public static EnemyData GetEliteEnemy(string enemyId)
    {
        EnemyDatabase db = Load();
        if (db == null) return null;
        return db.eliteEnemies.Find(e => e.id == enemyId);
    }

    /// <summary>
    /// 根据ID获取BOSS
    /// </summary>
    public static BossData GetBoss(string bossId)
    {
        EnemyDatabase db = Load();
        if (db == null) return null;
        return db.bosses.Find(b => b.id == bossId);
    }

    /// <summary>
    /// 获取指定章节的敌人数据（自动应用难度缩放）
    /// </summary>
    /// <param name="chapter">章节号（1或2）</param>
    public static EnemyData GetEnemyForChapter(string enemyId, int chapter)
    {
        EnemyData enemy = GetNormalEnemy(enemyId);
        if (enemy == null)
            enemy = GetEliteEnemy(enemyId);
        
        if (enemy == null) return null;

        // 复制一份，避免修改原始数据
        EnemyData chapterEnemy = new EnemyData
        {
            id = enemy.id,
            enemyName = enemy.enemyName,
            type = enemy.type,
            maxHp = chapter == 1 ? enemy.chapter1Hp : enemy.chapter2Hp,
            baseDamage = enemy.baseDamage,
            blockAmount = enemy.blockAmount,
            actionPattern = enemy.actionPattern,
            description = enemy.description,
            spritePath = enemy.spritePath,
            hasSpecialMechanic = enemy.hasSpecialMechanic,
            specialMechanicDesc = enemy.specialMechanicDesc
        };

        return chapterEnemy;
    }

    /// <summary>
    /// 清除缓存（用于重新加载）
    /// </summary>
    public static void ClearCache()
    {
        _cachedDatabase = null;
    }
}
