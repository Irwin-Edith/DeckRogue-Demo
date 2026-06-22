using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌数据库加载器
/// 使用方法：
///   CardDatabase db = CardLoader.Load();
///   CardData card = db.cards.Find(c => c.id == "strike");
/// </summary>
public static class CardLoader
{
    private static CardDatabase _cachedDatabase;
    private static readonly string CardsDataPath = "Data/Cards/CardsData";

    /// <summary>
    /// 加载卡牌数据库
    /// </summary>
    public static CardDatabase Load()
    {
        if (_cachedDatabase != null)
            return _cachedDatabase;

        TextAsset jsonFile = Resources.Load<TextAsset>(CardsDataPath);
        if (jsonFile == null)
        {
            Debug.LogError($"[CardLoader] 无法加载卡牌数据文件: {CardsDataPath}");
            return null;
        }

        try
        {
            _cachedDatabase = JsonUtility.FromJson<CardDatabase>(jsonFile.text);
            Debug.Log($"[CardLoader] 成功加载 {_cachedDatabase.cards.Count} 张卡牌");
            return _cachedDatabase;
        }
        catch (Exception e)
        {
            Debug.LogError($"[CardLoader] 解析卡牌数据失败: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 根据ID获取卡牌
    /// </summary>
    public static CardData GetCard(string cardId)
    {
        CardDatabase db = Load();
        if (db == null) return null;
        return db.cards.Find(c => c.id == cardId);
    }

    /// <summary>
    /// 根据类型获取所有卡牌
    /// </summary>
    public static List<CardData> GetCardsByType(CardType type)
    {
        CardDatabase db = Load();
        if (db == null) return new List<CardData>();
        return db.cards.FindAll(c => c.type == type);
    }

    /// <summary>
    /// 根据稀有度获取所有卡牌
    /// </summary>
    public static List<CardData> GetCardsByRarity(CardRarity rarity)
    {
        CardDatabase db = Load();
        if (db == null) return new List<CardData>();
        return db.cards.FindAll(c => c.rarity == rarity);
    }

    /// <summary>
    /// 获取基础牌组（初始卡组）
    /// </summary>
    public static List<CardData> GetStarterDeck()
    {
        CardDatabase db = Load();
        if (db == null) return new List<CardData>();

        List<CardData> starterDeck = new List<CardData>();
        
        // 打击 x5
        for (int i = 0; i < 5; i++)
            starterDeck.Add(GetCard("strike"));
        
        // 防御 x5
        for (int i = 0; i < 5; i++)
            starterDeck.Add(GetCard("defend"));
        
        // 血契觉醒 x1
        starterDeck.Add(GetCard("blood_awakening"));

        return starterDeck;
    }

    /// <summary>
    /// 清除缓存（用于重新加载）
    /// </summary>
    public static void ClearCache()
    {
        _cachedDatabase = null;
    }
}
