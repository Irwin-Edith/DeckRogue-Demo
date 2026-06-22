using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人意图类型
/// </summary>
[Serializable]
public enum EnemyIntentType
{
    Attack,         // 攻击
    Defend,         // 防御（获得格挡）
    Buff,           // 增益
    Debuff,         // 减益（给玩家）
    Special,        // 特殊技能
    AttackBuff,     // 攻击+增益
    MultiAttack,    // 多段攻击
    Heal            // 治疗
}

/// <summary>
/// 单个意图定义
/// </summary>
[Serializable]
public class IntentData
{
    public EnemyIntentType type;      // 意图类型
    public int value;                  // 意图数值（如伤害值、格挡值）
    public int times;                  // 次数（用于多段攻击）
    public string description;          // 描述文本
}

/// <summary>
/// 敌人行为模式
/// </summary>
[Serializable]
public class EnemyAction
{
    public int turn;           // 回合数（从1开始）
    public IntentData intent;  // 该回合的意图
}

/// <summary>
/// 敌人数据定义
/// </summary>
[Serializable]
public class EnemyData
{
    public string id;                      // 唯一标识符
    public string enemyName;               // 显示名称
    public EnemyType type;                 // 敌人类型
    public int maxHp;                      // 最大生命值
    public int chapter1Hp;                 // 第一章生命值
    public int chapter2Hp;                 // 第二章生命值（如果不同）
    public int baseDamage;                  // 基础攻击伤害
    public int blockAmount;                // 格挡数值
    public List<EnemyAction> actionPattern; // 行动模式循环
    public string description;             // 敌人描述
    public string spritePath;              // 精灵图片路径
    public bool hasSpecialMechanic;        // 是否有特殊机制
    public string specialMechanicDesc;     // 特殊机制描述
}

/// <summary>
/// 敌人类型
/// </summary>
[Serializable]
public enum EnemyType
{
    Normal,     // 普通敌人
    Elite,      // 精英敌人
    Boss        // BOSS
}

/// <summary>
/// BOSS多阶段数据
/// </summary>
[Serializable]
public class BossPhaseData
{
    public int phaseId;                    // 阶段ID（从1开始）
    public int hpThreshold;                 // 进入该阶段的血量阈值
    public List<EnemyAction> phasePattern; // 该阶段的行动模式
    public string phaseDescription;         // 阶段描述
}

/// <summary>
/// BOSS完整数据
/// </summary>
[Serializable]
public class BossData
{
    public string id;                              // BOSS ID
    public string bossName;                        // BOSS名称
    public int maxHp;                              // 最大生命值
    public List<BossPhaseData> phases;            // 阶段数据列表
    public bool usesBloodDebtMechanic;            // 是否使用血债机制
    public string description;                     // BOSS描述
    public string spritePath;                      // 精灵图片路径
}

/// <summary>
/// 敌人数值数据库
/// </summary>
[Serializable]
public class EnemyDatabase
{
    public List<EnemyData> normalEnemies;
    public List<EnemyData> eliteEnemies;
    public List<BossData> bosses;
}
