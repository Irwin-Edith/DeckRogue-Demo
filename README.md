# DeckRogue-Demo

sts2拆解案：[拆解](https://app.notion.com/p/Deconstruction-Slay-The-Spire-2-3862fc0d650380f49a6fd67dfc5aa581?source=copy_link)

# VOID DESCENT — 执行级策划案 v1.0

---

## 第一章：数值框架

### 1.1 全局数值常数

| 参数名                   | 值  | 说明                           |
| ------------------------ | --- | ------------------------------ |
| MAX_ENERGY_PER_TURN      | 3   | 每回合维度能量上限默认值       |
| BASE_DRAW_PER_TURN       | 5   | 每回合基础抽牌数               |
| MAX_HAND_SIZE            | 10  | 手牌上限，超出部分自动弃牌     |
| STARTING_HP              | 70  | 初始生命值（序章）             |
| MAX_HP_CAP               | 150 | 生命值上限                     |
| VOID_METER_MAX_DEFAULT   | 6   | 虚空计量默认值上限             |
| VOID_RESONANCE_THRESHOLD | 6   | 触发虚空共鸣的计量阈值         |
| VOID_OVERFLOW_DAMAGE     | 12  | 虚空反噬每次溢出造成的伤害     |
| BOSS_CONTINUOUS_COUNT    | 3   | 第五维度连续Boss战数量         |
| ACT_NODE_COUNT           | 15  | 每个Act的基础节点数量          |
| HEAL_AMOUNT_PERCENT      | 30  | 维度裂隙休息恢复的生命值百分比 |
| DECK_SIZE_CAP            | 25  | 牌组上限（含初始卡）           |
| POTION_SLOT_COUNT        | 3   | 维度护盾道具栏位数量           |
| ELITE_HP_MULTIPLIER      | 1.5 | 精英怪生命值相对普通怪的倍率   |
| ELITE_DAMAGE_MULTIPLIER  | 1.3 | 精英怪伤害相对普通怪的倍率     |
| BOSS_HP_MULTIPLIER       | 3.0 | Boss相对普通怪的生命值倍率     |
| BOSS_DAMAGE_MULTIPLIER   | 1.8 | Boss相对普通怪的伤害倍率       |
| ACT_SCALING_HP_PERCENT   | 15  | 相邻Act间怪物HP增长百分比      |
| ACT_SCALING_DMG_PERCENT  | 10  | 相邻Act间怪物伤害增长百分比    |

---

### 1.2 卡牌数值常数

| 参数名                        | 值  | 说明                              |
| ----------------------------- | --- | --------------------------------- |
| CARD_COST_MIN                 | 0   | 卡牌能量消耗最小值                |
| CARD_COST_MAX                 | 5   | 卡牌能量消耗最大值                |
| CARD_DAMAGE_SCALE_COMMON      | 6   | 普通卡牌单次伤害基准值            |
| CARD_DAMAGE_SCALE_RARE        | 9   | 稀有卡牌单次伤害基准值            |
| CARD_DAMAGE_SCALE_LEGENDARY   | 14  | 传说卡牌单次伤害基准值            |
| CARD_BLOCK_SCALE_COMMON       | 5   | 普通卡牌护盾基准值                |
| CARD_BLOCK_SCALE_RARE         | 8   | 稀有卡牌护盾基准值                |
| CARD_BLOCK_SCALE_LEGENDARY    | 13  | 传说卡牌护盾基准值                |
| VOID_CARD_FILL_AMOUNT         | 2   | 虚空标签卡牌填充虚空计量的默认值  |
| CONTRACT_ACTIVATION_DELAY_MAX | 3   | 契约牌最大延迟激活回合数          |
| UPGRADE_DAMAGE_BONUS          | 30% | 升级后伤害增幅（百分比）          |
| UPGRADE_BLOCK_BONUS           | 30% | 升级后护盾增幅（百分比）          |
| UPGRADE_COST_REDUCTION        | 1   | 升级后能量消耗降低（最多降低至0） |
| REMOVE_CARD_COST_BASE         | 100 | 首次移除卡牌的基础费用（黄金）    |
| REMOVE_CARD_COST_INCREMENT    | 50  | 后续每次移除卡牌的递增费用        |
| REMOVE_CARD_COST_MAX          | 300 | 单次移除卡牌的费用上限            |

---

### 1.3 经济数值常数

| 参数名                    | 值  | 说明                 |
| ------------------------- | --- | -------------------- |
| GOLD_REWARD_COMBAT_MIN    | 10  | 普通战斗最小黄金奖励 |
| GOLD_REWARD_COMBAT_MAX    | 20  | 普通战斗最大黄金奖励 |
| GOLD_REWARD_ELITE_MIN     | 25  | 精英战最小黄金奖励   |
| GOLD_REWARD_ELITE_MAX     | 40  | 精英战最大黄金奖励   |
| GOLD_REWARD_BOSS_MIN      | 50  | Boss战最小黄金奖励   |
| GOLD_REWARD_BOSS_MAX      | 75  | Boss战最大黄金奖励   |
| GOLD_SHOP_COMMON_RELIC    | 80  | 商店常见遗物价格     |
| GOLD_SHOP_RARE_RELIC      | 160 | 商店稀有遗物价格     |
| GOLD_SHOP_LEGENDARY_RELIC | 280 | 商店传说遗物价格     |
| GOLD_SHOP_COMMON_CARD     | 50  | 商店普通卡牌价格     |
| GOLD_SHOP_RARE_CARD       | 100 | 商店稀有卡牌价格     |
| GOLD_SHOP_POTION          | 40  | 商店维度护盾道具价格 |
| GOLD_EVENT_MIN            | 0   | 事件节点最小黄金得失 |
| GOLD_EVENT_MAX            | 50  | 事件节点最大黄金得失 |

---

### 1.4 成长曲线参数

怪物属性随Act的缩放公式：

Act_N_HP = Act_1_HP × (1 + ACT_SCALING_HP_PERCENT / 100)^(N-1)

Act_N_DMG = Act_1_DMG × (1 + ACT_SCALING_DMG_PERCENT / 100)^(N-1)

玩家随Act的缩放：

下一Act初始HP = min(当前HP - 战斗损失 + 休息恢复, MAX_HP_CAP)

祝福奖励按固定档位，不随Act缩放

---

## 第二章：数据结构

### 2.1 核心实体类

**Card（卡牌）**

CardID: string           // 唯一标识符，格式如 "VOID_001"

CardName: string         // 显示名称

CardType: enum           // ATTACK / SKILL / POWER / CONTRACT

CardRarity: enum         // COMMON / RARE / LEGENDARY

Cost: int                // 能量消耗，0~5

CostReduction: int       // 当前能量消耗减免（来自遗物/Buff）

VoidTag: bool            // 是否为虚空标签卡牌

VoidFillAmount: int     // 打出时填充虚空计量的值（无虚空标签则为0）

BaseDamage: int          // 基础伤害值

BaseBlock: int           // 基础护盾值

BaseEffectValue: int     // 附加效果值（如抽牌数、治疗量等）

EffectList: List `<Effect>` // 效果列表

IsUpgraded: bool         // 是否已升级

UpgradedVersionID: string // 升级版本卡牌ID，若无则为空

TargetType: enum         // SINGLE / ALL / SELF / NONE

Description: string      // 描述文本（含动态占位符）

**Relic（维度遗物）**

RelicID: string

RelicName: string

RelicRarity: enum        // MARK / SHARD / ORIGIN

RelicType: enum          // PASSIVE / TRIGGER / ACTIVE

EffectDescription: string

IsUsed: bool             // 一次性遗物是否已使用

Cooldown: int            // 主动型遗物当前冷却回合数

MaxCooldown: int         // 主动型遗物最大冷却回合数

VoidSynergy: bool        // 是否与虚空共鸣联动

Stacks: int              // 可叠加层数（某些遗物可叠加）

**Character（角色/玩家）**

CharacterID: string

CurrentHP: int

MaxHP: int

CurrentEnergy: int

MaxEnergy: int           // 默认3，可被遗物/祝福改变

CurrentVoidMeter: int    // 当前虚空计量

VoidMeterMax: int        // 虚空计量上限，可被遗物/祝福改变

VoidResonanceActive: bool // 当前是否处于虚空共鸣状态

CurrentShield: int       // 当前护盾值（回合结束时衰减至0）

DrawAmount: int           // 每回合抽牌数（默认5，可被改变）

HandSizeMax: int          // 手牌上限（默认10）

Gold: int

CurrentDeck: List `<CardID>`  // 牌组（所有卡牌）

CurrentHand: List `<Card>`    // 当前手牌

DrawPile: List `<CardID>`     // 抽牌堆

DiscardPile: List `<CardID>`  // 弃牌堆

PotionSlots: List `<Potion>`  // 道具栏位（3格）

EquippedRelics: List `<Relic>` // 已装备遗物（最多3件）

ActiveBlessings: List `<Blessing>` // 当前激活的祝福

CurrentIntent: Intent     // 当前回合敌人意图

BlockValue: int           // 本回合已获得的护盾

IsStunned: bool           // 是否被眩晕（本回合无法行动）

IsVulnerable: bool        // 是否处于易伤状态（受到伤害+50%）

IsWeak: bool             // 是否处于虚弱状态（伤害-25%）

**Potion（维度护盾道具）**

PotionID: string

PotionName: string

EffectType: enum          // HEAL / DAMAGE / BUFF / DEBUFF / DRAW

EffectValue: int

IsConsumable: bool        // 是否为一次性使用

CanUseInCombat: bool      // 战斗中是否可用

CanUseOutsideCombat: bool // 战斗外是否可用

CurrentStack: int         // 当前叠加数量

**Intent（敌人意图）**

IntentType: enum

  // ATTACK：下次对玩家造成伤害

  // DEFEND：下次为自己添加护盾

  // BUFF：为自己添加攻击力提升或特殊Buff

  // DEBUFF：试图对玩家施加减益

  // CHARGE：蓄力一回合，下回合必定执行某攻击

  // SPECIAL：特殊行为，由具体敌人定义

IntentTarget: enum        // PLAYER / SELF / ALL_PLAYERS

IntentValue: int          // 意图数值（如伤害量、护盾量）

IntentNextValue: int     // 下下回合的预期意图值（用于显示预警）

RoundsUntilExecution: int // 蓄力型意图还需几回合执行

**Blessing（祝福）**

BlessingID: string

BlessingName: string

BlessingType: enum        // DIMENSION / VOID / RESONANCE

EffectDescription: string

IsPermanent: bool        // 是否永久生效（大部分祝福是永久）

AppliedToRun: bool        // 是否应用于整个Run

AppliedToAct: bool        // 是否仅应用于当前Act

AppliedToCombat: bool     // 是否仅应用于当前战斗

Stacks: int               // 叠加层数

**Node（地图节点）**

NodeID: string

NodeType: enum

  // COMBAT：普通战斗

  // ELITE：精英遭遇

  // REST：维度裂隙（篝火）

  // SHOP：交易商（商店）

  // EVENT：虚空祭坛（事件）

  // TREASURE：维度裂缝（宝藏）

  // BOSS：维度裂口（Boss）

NodePosition: (x, y)      // 节点在地图上的二维坐标

IsLocked: bool            // 是否已锁定（未满足前置条件）

IsVisited: bool           // 是否已访问

AvailablePaths: List `<NodeID>` // 从该节点可达的下一节点列表

**Map（维度地图）**

MapID: string             // 对应Act编号 Act_1 ~ Act_5

AllNodes: List `<Node>`

StartNode: NodeID         // 起点节点

BossNode: NodeID          // Boss节点（Act_5有3个Boss节点）

CurrentNode: NodeID       // 玩家当前所在节点

VisitedNodes: List `<NodeID>` // 已访问节点列表

AvailableNodes: List `<NodeID>` // 当前可选的下一节点

NodePlacementRules: List `<PlacementRule>` // 节点放置约束规则

**Combat（战斗）**

CombatID: string

CombatType: enum          // COMBAT / ELITE / BOSS

EnemyParty: List `<Enemy>`   // 敌方单位列表（最多3个）

Player: Character         // 玩家角色实体

TurnNumber: int           // 当前回合编号（从1开始）

IsPlayerTurn: bool        // 当前是否为玩家回合

CombatState: enum         // PLAYER_TURN / ENEMY_TURN / BETWEEN_TURNS / ENDED

VictoryCondition: bool    // 是否满足胜利条件

DefeatCondition: bool     // 是否满足失败条件

CurrentRewards: List `<Reward>` // 战斗结束奖励列表

CombatLog: List `<CombatEvent>` // 战斗事件日志

**Enemy（敌人）**

EnemyID: string

EnemyName: string

MaxHP: int

CurrentHP: int

BaseDamage: int           // 基础攻击力

CurrentShield: int         // 当前护盾值

CurrentIntent: Intent      // 当前意图

StatusEffects: List `<StatusEffect>` // 状态效果列表

IsDead: bool

IsElite: bool

IsBoss: bool

ActLevel: int             // 所属Act等级（用于属性缩放）

PowerStacks: int          // 攻击力提升层数

DefenseStacks: int        // 护甲层数

VulnerableStacks: int      // 易伤层数

WeakStacks: int           // 虚弱层数

**StatusEffect（状态效果）**

EffectType: enum

  // POISON：中毒，每回合开始受到固定伤害

  // WOUND：伤口，无法获得护盾

  // VULNERABLE：易伤，受到伤害+50%

  // WEAK：虚弱，造成的伤害-25%

  // FLEX：临时力量，本回合有效

  // REGEN：再生，每回合开始恢复生命值

  // INTANGIBLE：无形，受到伤害降至1

  // TIME_WARP：时间扭曲，下回合少抽一张牌

Stacks: int

Duration: int             // 持续回合数，-1表示永久

IsAppliedThisTurn: bool  // 本回合是否已施加过（防止重复触发）

---

## 第三章：规则细节

### 3.1 抽牌规则

初始化时将牌组全部洗入抽牌堆。回合开始时从抽牌堆依次抽取手牌至手牌上限。若抽牌堆为空，则将弃牌堆全部洗入抽牌堆后继续抽取。若抽牌堆和弃牌堆均为空，则不抽牌。

手牌达到上限后多余的卡牌进入弃牌堆，不触发任何效果。

战斗中打出的卡牌移入弃牌堆。回合结束时未打出的手牌全部移入弃牌堆。

### 3.2 能量规则

每回合开始时将当前能量重置为能量上限（默认3）。

打出卡牌时消耗对应能量。若当前能量不足则无法打出该卡牌。

能量在回合结束时不清零，而是于下一回合开始时完全重置。

### 3.3 虚空计量规则

虚空计量初始为0，无上限约束（由VoidMeterMax决定共鸣阈值和反噬阈值）。

打出带有VoidTag的卡牌时，虚空计量增加VoidFillAmount值。

回合结束时若虚空计量达到或超过VoidResonanceThreshold（默认6），触发虚空共鸣：立即获得一次强力效果（具体效果由EquippedRelics中的VoidSynergy遗物定义），然后虚空计量在共鸣效果结算后降低至0。

若虚空计量超过VoidMeterMax（默认为共鸣阈值的1倍，即超过6的部分），在共鸣效果触发后额外受到VOID_OVERFLOW_DAMAGE（12点）伤害。

未达到共鸣阈值时，虚空计量跨回合保留，不会自动衰减。

### 3.4 护盾规则

护盾在获得当回合即可生效，用于抵消即将到来的伤害。

护盾在回合结束时完全衰减至0，不跨回合保留。

护盾优先于HP承受伤害。若有多次伤害，护盾依次抵消直至耗尽，剩余伤害作用于HP。

### 3.5 伤害计算规则

最终伤害 = 基础伤害 × (1 + 力量修正) × (易伤修正) × (虚弱修正) × (其他Buff修正)

力量修正：正值代表额外伤害，负值代表降低伤害，无力量时为0。

易伤修正：目标处于易伤状态时，受到的伤害增加50%，否则为0。

虚弱修正：攻击者处于虚弱状态时，造成的伤害降低25%，否则为0。

最小伤害：任何伤害的最低值为1（无视护盾的伤害除外）。

### 3.6 状态效果叠加规则

同类型状态效果叠加时，层数直接相加。

大多数状态效果有持续时间，每回合开始时持续时间减1，归零时效果消失。持续时间为-1表示永久效果。

易伤和虚弱效果无法叠加到自身身上（层数不增加，但持续时间刷新）。

### 3.7 契约牌（Contract）特殊规则

契约牌打出后进入"待激活"状态，不消耗能量，但占据手牌位置。

待激活的契约牌在满足其触发条件时自动激活并产生效果，然后移入弃牌堆。

契约牌的触发条件包括：特定回合数后激活、玩家血量降至特定阈值时激活、虚空计量达到特定值时激活、敌人血量降至特定值时激活。

每张契约牌有且只有一个触发条件，在打出时显示给玩家。

### 3.8 战斗胜负规则

胜利条件：所有敌方单位CurrentHP归零。

失败条件：玩家CurrentHP归零。

战斗结束时若玩家存活，无论HP剩余多少均视为胜利，触发奖励结算。

### 3.9 奖励结算规则

普通战斗胜利：从卡牌池中随机抽取3张普通至稀有卡牌供玩家选择，选择1张加入牌组，或选择跳过（不获得任何卡牌）。

精英战斗胜利：直接从遗物池中抽取1件遗物供玩家选择，选择1件装备，或选择跳过。

Boss战胜利：直接从遗物池中抽取3件遗物供玩家选择，选择1件装备，并同时获得普通战斗的卡牌奖励。

跳过奖励不产生任何补偿，后续战斗奖励不受影响。

### 3.10 祝福选择规则

每个Act通关后，玩家从系统提供的3个祝福中选择1个。

祝福分三类随机池：维度系、虚空系、共鸣系，每个池提供不同机制的增益。

同一Run中同一类型的祝福可以被多次选择，叠加效果。

### 3.11 牌组上限规则

牌组最大容量为25张（含初始卡牌）。

战斗奖励中若玩家牌组已达25张，系统不提供卡牌选择，直接跳过卡牌奖励环节。

移除卡牌不受上限约束。

---

## 第四章：异常处理

### 4.1 资源不足异常

**能量不足**：玩家尝试打出能量消耗超过当前能量的卡牌时，系统禁止该操作，卡牌保持在手牌中不动，不触发任何效果，不显示错误提示。玩家需选择其他合法操作。

**黄金不足**：玩家尝试购买商店物品时若黄金不足，系统禁用该购买按钮，悬停显示"黄金不足"提示，玩家无法完成购买操作。

**抽牌异常**：若抽牌堆和弃牌堆均为空，不进行任何抽牌操作，回合正常继续。若手牌已满，溢出的卡牌直接移入弃牌堆，不触发任何效果。

**虚空计量溢出**：超出VoidMeterMax的部分在共鸣结算后造成VOID_OVERFLOW_DAMAGE伤害，该伤害可被护盾抵消。若溢出伤害导致玩家HP归零，视为战斗失败。

### 4.2 牌组边界异常

**空牌组抽牌**：若抽牌堆为空但弃牌堆有卡牌，自动执行洗牌操作，将弃牌堆洗入抽牌堆后继续抽牌。若两者均为空，则不抽牌。

**手牌溢出**：超出HandSizeMax的手牌从最左侧开始依次强制弃牌，弃牌触发弃牌事件（若有相关遗物效果则触发）。

**卡牌已满仍获赠**：若牌组已达DECK_SIZE_CAP上限，战斗奖励的卡牌选项显示"跳过"按钮，移除三选一选择中所有卡牌选项，玩家只能选择跳过。

### 4.3 生命值边界异常

**治疗溢出**：治疗量不会使CurrentHP超过MaxHP，超出部分直接丢弃。

**伤害溢出**：伤害量大于当前HP时，CurrentHP直接归零，触发战斗失败。

**休息溢出**：维度裂隙休息恢复量基于当前MaxHP的百分比计算，不受CurrentHP影响，确保最大恢复量不会超过MaxHP。

### 4.4 战斗流程异常

**玩家无合法操作**：若玩家当前手牌无法打出（所有卡牌能量消耗均超过当前能量且不使用道具），显示"结束回合"按钮，玩家只能结束回合。

**敌人意图不可读**：若敌人因特殊状态无法确定意图，默认显示"蓄力"意图，并在下回合执行随机基础攻击。

**战斗超时保护**：单个回合设置最大操作时间（默认60秒），超时后自动结束玩家回合。

**战斗卡死保护**：若连续10回合双方均未造成实质伤害（HP变化小于1），强制触发特殊机制：双方各受到当前最大HP的25%伤害，加速战斗结束。

### 4.5 状态效果异常

**状态叠加冲突**：当同一效果被多次施加时，层数叠加但持续时间取最大值（不取叠加）。

**负面状态过载**：若易伤层数超过10，自动将其限制为10层。虚弱同理。

**永久效果异常**：标记为永久的状态效果不受Duration衰减，但可被驱散类效果移除。

### 4.6 存档与数据异常

**Run中断保护**：每完成一个节点后自动保存当前Run状态，包括已访问节点、当前HP、牌组、遗物、祝福、虚空计量等全部数据。若游戏异常退出，下次启动时自动恢复到最近保存的节点。

**数据校验**：加载存档时对所有数值字段进行范围校验（HP不低于0不高于MaxHP、黄金不低于0、牌组数量不超上限等）。超出范围的字段自动修正为合法值并记录异常日志。

**版本兼容性**：存档文件包含版本号，读取存档时检测版本匹配。若存档版本低于当前版本，执行数据迁移程序；若高于当前版本，拒绝加载并提示版本不兼容。

---

## 第五章：平衡性基准

### 5.1 战斗时长基准

| 战斗类型 | 预期回合数 | 基准范围  |
| -------- | ---------- | --------- |
| 普通战斗 | 4~6 回合   | 3~8 回合  |
| 精英战斗 | 5~8 回合   | 4~10 回合 |
| Boss战   | 8~12 回合  | 6~15 回合 |

战斗时长偏离基准过多（超过±3回合）时，触发战斗平衡审查。

### 5.2 卡牌价值基准

以一张普通攻击牌"虚空冲击"（消耗1能量，对单个敌人造成6点伤害）为价值基准=1.0。

| 卡牌类型 | 能量消耗 | 预期效果                | 基准价值系数                |
| -------- | -------- | ----------------------- | --------------------------- |
| 攻击牌   | 1        | 6伤害                   | 1.0                         |
| 攻击牌   | 2        | 12伤害 或 9伤害+1效果   | 1.8                         |
| 攻击牌   | 3        | 18伤害 或 14伤害+多目标 | 2.5                         |
| 技能牌   | 1        | 5护盾                   | 0.8                         |
| 技能牌   | 2        | 10护盾 或 8护盾+1效果   | 1.5                         |
| 技能牌   | 3        | 15护盾 或 12护盾+多目标 | 2.2                         |
| 虚空牌   | 2        | 14伤害+填充虚空3        | 价值按基础伤害+共鸣收益评估 |
| 契约牌   | 1        | 延迟强力效果            | 价值按延迟损失折算，约1.3倍 |

虚空牌的实际价值需叠加虚空共鸣的预期收益，建议通过模拟测试确定具体数值。

### 5.3 遗物价值基准

以一件常见被动遗物"虚空印记"（每场战斗首次虚空共鸣时恢复4点HP）为基准价值=1.0。

| 遗物稀有度   | 被动型基准倍率 | 触发型基准倍率 | 主动型基准倍率 |
| ------------ | -------------- | -------------- | -------------- |
| 印记（常见） | 1.0            | 1.2            | 0.9            |
| 裂片（稀有） | 2.0            | 2.5            | 1.8            |
| 本源（传说） | 3.5            | 4.0            | 3.0            |

### 5.4 节点收益基准

以"击败一个普通敌人获得1张卡牌"为基准=1.0。

| 节点类型 | 战斗难度          | 奖励价值                          |
| -------- | ----------------- | --------------------------------- |
| 普通战斗 | 1.0×             | 1.0（1张卡牌）+ 黄金10~20         |
| 精英战斗 | 1.5×伤害 1.4×HP | 3.0（1件遗物）+ 黄金25~40         |
| Boss战   | 3.0×HP 1.8×伤害 | 6.0（3件遗物+卡牌）+ 黄金50~75    |
| 维度裂隙 | 无战斗            | 3.0（休息恢复30%HP 或 升级1张卡） |
| 虚空祭坛 | 随机              | 0.5~5.0（取决于随机事件结果）     |
| 维度裂缝 | 无战斗            | 2.0（直接获得1件遗物）            |

### 5.5 Act 难度梯度基准

| Act   | 怪物基础攻击 | 怪物基础HP | 怪物类型丰富度 | Boss特殊机制           |
| ----- | ------------ | ---------- | -------------- | ---------------------- |
| Act 1 | 基准值       | 基准值     | 低（3~5种）    | 无特殊机制             |
| Act 2 | +10%         | +15%       | 中（5~8种）    | Boss有2个阶段          |
| Act 3 | +21%         | +32%       | 高（8~12种）   | Boss有3个阶段          |
| Act 4 | +33%         | +52%       | 高（8~12种）   | Boss有3个阶段+环境效果 |
| Act 5 | +46%         | +76%       | 无普通怪物     | 连续3场Boss战          |

注：百分比为与Act 1基准值的累积增长，计算方式为(1+0.1)^(N-1)格式。

### 5.6 虚空共鸣收益基准

虚空共鸣触发一次所获得的战斗收益，应当等价于或略低于"消耗2能量打出1张普通攻击牌"的价值，即约等于2.0基准价值。

共鸣触发频率建议：每场普通战斗1~2次，每场精英战斗2~3次，每场Boss战3~5次。

共鸣反噬伤害建议控制在最大HP的8%~15%之间，避免因一次反噬导致战斗失败但也不可完全忽略。

### 5.7 胜率基准

以下胜率为在Ascension Level 0（最低难度）下的单角色预期值：

| 阶段        | 预期玩家状态 |
| ----------- | ------------ |
| Act 1通关率 | >85%         |
| Act 2通关率 | >65%         |
| Act 3通关率 | >40%         |
| 完整Run胜率 | >25%         |

若某一角色胜率持续低于20%或高于35%，需调整该角色的初始卡牌强度或初始遗物效果。

### 5.8 模拟测试标准

每次重大更新后，使用以下自动化测试流程验证平衡性：

第一步：运行1000次随机Bot模拟Run，记录每个Act通关率和完整胜率。

第二步：针对每个卡牌类型，计算Bot在该卡牌被使用时的胜率贡献，胜率贡献低于0.5%的卡牌标记为待审查。

第三步：针对每个遗物，计算装备该遗物与未装备该遗物的胜率差异，差异低于1%的遗物标记为待审查。

第四步：检查战斗时长分布，若超过20%的战斗超出基准范围，触发专项审查。
