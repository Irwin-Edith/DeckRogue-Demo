using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// CSV 数据解析工具
/// 使用方法：
///   List<CardData> cards = CSVParser.ParseCardCSV(path);
/// </summary>
public static class CSVParser
{
    /// <summary>
    /// 解析卡牌 CSV 文件
    /// </summary>
    public static List<CardData> ParseCardCSV(string filePath)
    {
        List<CardData> cards = new List<CardData>();
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[CSVParser] 文件不存在: {filePath}");
            return cards;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            Debug.LogError($"[CSVParser] CSV 文件格式错误（无数据行）: {filePath}");
            return cards;
        }

        // 解析表头
        string[] headers = ParseCSVLine(lines[0]);
        Dictionary<string, int> headerMap = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            headerMap[headers[i].Trim().ToLower()] = i;
        }

        // 解析数据行
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = ParseCSVLine(lines[i]);
            if (values.Length == 0) continue;

            CardData card = new CardData
            {
                id = GetString(values, headerMap, "id"),
                cardName = GetString(values, headerMap, "cardname"),
                description = GetString(values, headerMap, "description"),
                cost = GetInt(values, headerMap, "cost"),
                type = GetEnum<CardType>(values, headerMap, "type"),
                rarity = GetEnum<CardRarity>(values, headerMap, "rarity"),
                baseDamage = GetInt(values, headerMap, "basedamage"),
                baseBlock = GetInt(values, headerMap, "baseblock"),
                bloodDebtGain = GetInt(values, headerMap, "blooddebtgain"),
                bloodDebtConsume = GetInt(values, headerMap, "blooddebtconsume"),
                drawCards = GetInt(values, headerMap, "drawcards"),
                healAmount = GetInt(values, headerMap, "healamount"),
                tempHpGain = GetInt(values, headerMap, "temphpgain"),
                strengthGain = GetInt(values, headerMap, "strengthgain")
            };

            cards.Add(card);
        }

        Debug.Log($"[CSVParser] 成功解析 {cards.Count} 张卡牌");
        return cards;
    }

    /// <summary>
    /// 解析敌人 CSV 文件
    /// </summary>
    public static List<EnemyData> ParseEnemyCSV(string filePath)
    {
        List<EnemyData> enemies = new List<EnemyData>();
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[CSVParser] 文件不存在: {filePath}");
            return enemies;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2) return enemies;

        string[] headers = ParseCSVLine(lines[0]);
        Dictionary<string, int> headerMap = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            headerMap[headers[i].Trim().ToLower()] = i;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = ParseCSVLine(lines[i]);
            if (values.Length == 0) continue;

            EnemyData enemy = new EnemyData
            {
                id = GetString(values, headerMap, "id"),
                enemyName = GetString(values, headerMap, "enemyname"),
                type = GetEnum<EnemyType>(values, headerMap, "type"),
                maxHp = GetInt(values, headerMap, "maxhp"),
                chapter1Hp = GetInt(values, headerMap, "chapter1hp"),
                chapter2Hp = GetInt(values, headerMap, "chapter2hp"),
                baseDamage = GetInt(values, headerMap, "basedamage"),
                blockAmount = GetInt(values, headerMap, "blockamount"),
                description = GetString(values, headerMap, "description"),
                spritePath = GetString(values, headerMap, "spritepath"),
                hasSpecialMechanic = GetBool(values, headerMap, "hasspecialmechanic"),
                specialMechanicDesc = GetString(values, headerMap, "specialmechanicdesc"),
                actionPattern = new List<EnemyAction>()
            };

            // 解析行动模式JSON
            string actionJson = GetString(values, headerMap, "actionpatternjson");
            if (!string.IsNullOrEmpty(actionJson))
            {
                try
                {
                    enemy.actionPattern = ParseActionPattern(actionJson);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[CSVParser] 解析行动模式失败 ({enemy.id}): {e.Message}");
                }
            }

            enemies.Add(enemy);
        }

        Debug.Log($"[CSVParser] 成功解析 {enemies.Count} 个敌人");
        return enemies;
    }

    /// <summary>
    /// 解析 CSV 行（处理引号包围的字段）
    /// </summary>
    private static string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.Trim());
                current = "";
            }
            else
            {
                current += c;
            }
        }
        result.Add(current.Trim());

        return result.ToArray();
    }

    /// <summary>
    /// 简单解析行动模式JSON
    /// </summary>
    private static List<EnemyAction> ParseActionPattern(string json)
    {
        List<EnemyAction> actions = new List<EnemyAction>();
        json = json.Trim();
        if (json.StartsWith("[") && json.EndsWith("]"))
        {
            json = json.Substring(1, json.Length - 2);
        }

        string[] entries = SplitJsonArray(json);
        foreach (string entry in entries)
        {
            if (string.IsNullOrWhiteSpace(entry)) continue;
            EnemyAction action = new EnemyAction();

            // 简单解析 turn
            int turnIndex = entry.IndexOf("\"turn\":");
            if (turnIndex >= 0)
            {
                int start = entry.IndexOf(":", turnIndex) + 1;
                int end = entry.IndexOf(",", start);
                if (end < 0) end = entry.IndexOf("}", start);
                string turnStr = entry.Substring(start, end - start).Trim();
                int.TryParse(turnStr, out action.turn);
            }

            // 解析 intent
            action.intent = new IntentData();
            int intentStart = entry.IndexOf("\"intent\":{");
            if (intentStart >= 0)
            {
                int objStart = entry.IndexOf("{", intentStart);
                int objEnd = entry.LastIndexOf("}");
                if (objEnd > objStart)
                {
                    string intentJson = entry.Substring(objStart, objEnd - objStart + 1);
                    action.intent.type = GetJsonValue<EnemyIntentType>(intentJson, "type");
                    action.intent.value = GetJsonValue<int>(intentJson, "value");
                    action.intent.times = GetJsonValue<int>(intentJson, "times");
                    action.intent.description = GetJsonValue<string>(intentJson, "description");
                }
            }

            actions.Add(action);
        }

        return actions;
    }

    private static string[] SplitJsonArray(string json)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        int depth = 0;
        string current = "";

        foreach (char c in json)
        {
            if (c == '"') inQuotes = !inQuotes;
            else if (!inQuotes)
            {
                if (c == '{') depth++;
                else if (c == '}') depth--;
                else if (c == ',' && depth == 0)
                {
                    result.Add(current);
                    current = "";
                    continue;
                }
            }
            current += c;
        }
        if (!string.IsNullOrWhiteSpace(current))
            result.Add(current);

        return result.ToArray();
    }

    private static T GetJsonValue<T>(string json, string key)
    {
        string search = $"\"{key}\":";
        int index = json.IndexOf(search);
        if (index < 0) return default(T);

        int start = index + search.Length;
        int end = json.IndexOf(",", start);
        if (end < 0) end = json.IndexOf("}", start);

        string valueStr = json.Substring(start, end - start).Trim();

        if (typeof(T) == typeof(string))
        {
            valueStr = valueStr.Trim('"');
            return (T)(object)valueStr;
        }
        if (typeof(T).IsEnum)
        {
            return Enum.Parse<T>(valueStr.Trim());
        }
        return (T)Convert.ChangeType(valueStr, typeof(T));
    }

    #region 字段读取辅助

    private static string GetString(string[] values, Dictionary<string, int> map, string key)
    {
        if (!map.ContainsKey(key) || map[key] >= values.Length) return "";
        return values[map[key]].Trim();
    }

    private static int GetInt(string[] values, Dictionary<string, int> map, string key)
    {
        string str = GetString(values, map, key);
        int.TryParse(str, out int result);
        return result;
    }

    private static bool GetBool(string[] values, Dictionary<string, int> map, string key)
    {
        string str = GetString(values, map, key).ToLower();
        return str == "true" || str == "1";
    }

    private static T GetEnum<T>(string[] values, Dictionary<string, int> map, string key) where T : struct
    {
        string str = GetString(values, map, key);
        if (Enum.TryParse<T>(str, true, out T result))
            return result;
        return default(T);
    }

    #endregion

    /// <summary>
    /// 将卡牌列表导出为 JSON（用于从 CSV 转换到 JSON）
    /// </summary>
    public static void ExportCardsToJSON(List<CardData> cards, string outputPath)
    {
        CardDatabase db = new CardDatabase { cards = cards };
        string json = JsonUtility.ToJson(db, true);
        File.WriteAllText(outputPath, json);
        Debug.Log($"[CSVParser] 已导出 JSON 到: {outputPath}");
    }
}
