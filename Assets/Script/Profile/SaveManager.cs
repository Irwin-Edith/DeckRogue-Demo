using UnityEngine;
using System.IO;

/// <summary>
/// 存档管理器（单例模式）
/// 负责所有存档的读取、保存、删除操作
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// 单例实例，方便其他脚本访问
    /// </summary>
    public static SaveManager instance;
    
    /// <summary>
    /// 最大存档槽位数量
    /// </summary>
    private const int MAX_SAVE_SLOTS = 3;
    
    /// <summary>
    /// 存档文件名前缀
    /// 最终文件名为：save_0.json, save_1.json, save_2.json
    /// </summary>
    private const string SAVE_FILE_PREFIX = "save_";
    
    /// <summary>
    /// 存档文件扩展名
    /// </summary>
    private const string SAVE_FILE_EXTENSION = ".json";
    
    /// <summary>
    /// 内存中的存档数据数组
    /// 与磁盘文件保持同步
    /// </summary>
    public SaveData[] saveSlots = new SaveData[MAX_SAVE_SLOTS];
    
    /// <summary>
    /// 初始化单例，加载所有存档
    /// </summary>
    void Awake()
    {
        // 第一次创建时，设为单例
        if (instance == null)
        {
            instance = this;
            // 切换场景时不销毁，确保持久存在
            DontDestroyOnLoad(gameObject);
            // 启动时加载所有存档到内存
            LoadAllSaves();
        }
        else
        {
            // 如果已存在单例，销毁当前对象（防止重复）
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 保存游戏到指定槽位
    /// </summary>
    /// <param name="slotIndex">槽位索引（0、1、2）</param>
    public void SaveToSlot(int slotIndex)
    {
        // 越界检查
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS) return;
        
        // 创建存档数据
        SaveData data = new SaveData();
        
        // 记录当前保存时间
        data.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // 从游戏系统获取游戏时长
        data.playTimeHours = GetTotalPlayTime();
        
        // 从游戏系统获取通关次数
        data.clearCount = GetTotalClearCount();
        
        // 标记为非空存档
        data.isEmpty = false;
        
        // 更新内存中的数据
        saveSlots[slotIndex] = data;
        
        // 序列化为 JSON 格式
        string path = GetSavePath(slotIndex);
        string json = JsonUtility.ToJson(data);
        
        // 写入磁盘文件
        File.WriteAllText(path, json);
    }
    
    /// <summary>
    /// 从指定槽位加载存档
    /// </summary>
    /// <param name="slotIndex">槽位索引</param>
    /// <returns>存档数据，如果不存在则返回null</returns>
    public SaveData LoadFromSlot(int slotIndex)
    {
        // 越界检查
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS) return null;
        
        // 获取文件路径
        string path = GetSavePath(slotIndex);
        
        // 检查文件是否存在
        if (File.Exists(path))
        {
            // 读取文件内容
            string json = File.ReadAllText(path);
            
            // 反序列化为 SaveData 对象
            return JsonUtility.FromJson<SaveData>(json);
        }
        
        // 文件不存在，返回null
        return null;
    }
    
    /// <summary>
    /// 删除指定槽位的存档
    /// </summary>
    /// <param name="slotIndex">槽位索引</param>
    public void DeleteSlot(int slotIndex)
    {
        // 越界检查
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS) return;
        
        // 获取文件路径
        string path = GetSavePath(slotIndex);
        
        // 如果文件存在，删除它
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        // 内存中的数据也要清空
        saveSlots[slotIndex] = new SaveData();
    }
    
    /// <summary>
    /// 启动时加载所有存档到内存
    /// </summary>
    public void LoadAllSaves()
    {
        // 遍历所有槽位
        for (int i = 0; i < MAX_SAVE_SLOTS; i++)
        {
            // 加载单个槽位
            SaveData data = LoadFromSlot(i);
            // 如果文件不存在，创建一个空存档
            saveSlots[i] = data ?? new SaveData();
        }
    }
    
    /// <summary>
    /// 获取存档文件的完整路径
    /// </summary>
    /// <param name="slotIndex">槽位索引</param>
    /// <returns>完整的文件路径</returns>
    string GetSavePath(int slotIndex)
    {
        // Application.persistentDataPath 是跨平台的存储路径
        // Windows: C:/Users/用户名/AppData/Local/项目名/
        // Android: /data/data/com.公司名.项目名/
        return Application.persistentDataPath + "/" + SAVE_FILE_PREFIX + slotIndex + SAVE_FILE_EXTENSION;
    }
    
    /// <summary>
    /// 获取当前游戏总时长
    /// TODO: 需要接入实际的游戏计时系统
    /// </summary>
    /// <returns>游戏时长（小时）</returns>
    float GetTotalPlayTime()
    {
        // 这里应该接入你的游戏计时系统
        // 例如：return GameTimeManager.totalHours;
        return 0f;
    }
    
    /// <summary>
    /// 获取所有角色累计通关次数
    /// TODO: 需要接入实际的游戏数据
    /// </summary>
    /// <returns>通关次数</returns>
    int GetTotalClearCount()
    {
        // 这里应该接入你的游戏数据
        // 例如：return CharacterManager.totalClears;
        return 0;
    }
}