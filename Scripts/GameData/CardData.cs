using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 血契骑士卡牌数据定义
/// </summary>
[Serializable]
public class CardData
{
    public string id;              // 唯一标识符，如 "strike"、"defend"
    public string cardName;       // 显示名称，如 "打击"
    public string description;    // 卡牌描述，如 "造成 {0} 点伤害"
    public int cost;              // 能量消耗，-1 表示 X 费用
    public CardType type;         // 卡牌类型
    public CardRarity rarity;     // 稀有度
    public int baseDamage;        // 基础伤害值
    public int baseBlock;         // 基础格挡值
    public int bloodDebtGain;     // 获得血债（正数）或消耗血债（负数）
    public int bloodDebtConsume;  // 消耗血债值（用于强制清算类卡牌）
    public int drawCards;         // 抽牌数量
    public int healAmount;        // 治疗量
    public int tempHpGain;        // 临时生命获取
    public int strengthGain;     // 获得力量
    public bool isSkillUpgraded; // 是否已升级（备用）
    
    /// <summary>
    /// 获取格式化后的描述文本
    /// </summary>
    public string GetFormattedDescription()
    {
        string result = description;
        result = result.Replace("{damage}", baseDamage.ToString());
        result = result.Replace("{block}", baseBlock.ToString());
        result = result.Replace("{bloodDebt}", bloodDebtGain.ToString());
        result = result.Replace("{draw}", drawCards.ToString());
        result = result.Replace("{heal}", healAmount.ToString());
        result = result.Replace("{tempHp}", tempHpGain.ToString());
        result = result.Replace("{strength}", strengthGain.ToString());
        return result;
    }
}

[Serializable]
public enum CardType
{
    Attack,     // 攻击牌
    Skill,      // 技能牌
    Power,      // 能力牌
    Curse       // 诅咒牌
}

[Serializable]
public enum CardRarity
{
    Basic,      // 基础牌
    Common,     // 普通牌
    Rare,       // 稀有牌
    Special     // 特殊牌
}

[Serializable]
public class CardDatabase
{
    public List<CardData> cards;
}
