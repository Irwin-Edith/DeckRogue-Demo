using System;

/// <summary>
/// 存档数据结构
/// 定义单个存档槽位包含的所有数据
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// 最后保存存档的时间
    /// 格式：2026-06-24 20:30:00
    /// </summary>
    public string saveTime;
    
    /// <summary>
    /// 当前存档的累计游戏时长（小时）
    /// </summary>
    public float playTimeHours;
    
    /// <summary>
    /// 所有角色累计通关次数
    /// </summary>
    public int clearCount;
    
    /// <summary>
    /// 该槽位是否为空（从未保存过）
    /// 用于判断是显示空槽还是已有存档
    /// </summary>
    public bool isEmpty;
    
    /// <summary>
    /// 构造函数：创建空存档
    /// </summary>
    public SaveData()
    {
        isEmpty = true;      // 默认为空
        playTimeHours = 0f;  // 初始游戏时长为0
        clearCount = 0;      // 初始通关次数为0
        saveTime = "";       // 清空保存时间
    }
}