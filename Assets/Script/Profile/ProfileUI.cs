using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存档界面控制器
/// 管理 Profile 场景中的存档显示和交互
/// </summary>
public class ProfileUI : MonoBehaviour
{
    /// <summary>
    /// 存档栏位预制体数组
    /// 索引0 = Slot 1，索引1 = Slot 2，索引2 = Slot 3
    /// 在 Inspector 中拖入对应的 SaveSlot 对象
    /// </summary>
    public GameObject[] saveSlotPrefabs;
    
    /// <summary>
    /// 删除确认弹窗面板
    /// 初始状态为隐藏，点击删除按钮时显示
    /// </summary>
    public GameObject deleteConfirmPanel;
    
    /// <summary>
    /// 当前正在删除的槽位索引
    /// 用于确认删除时知道删哪一个
    /// </summary>
    private int currentDeleteSlot = -1;
    
    /// <summary>
    /// 场景加载时执行
    /// 初始化显示所有存档
    /// </summary>
    void Start()
    {
        // 刷新所有存档栏位的显示
        RefreshAllSlots();
    }
    
    /// <summary>
    /// 刷新所有存档栏位的显示
    /// 从 SaveManager 读取数据，更新每个槽位的 UI
    /// </summary>
    public void RefreshAllSlots()
    {
        // 遍历所有存档槽位
        for (int i = 0; i < saveSlotPrefabs.Length; i++)
        {
            // 更新单个槽位的显示
            UpdateSlotUI(i);
        }
    }
    
    /// <summary>
    /// 更新单个存档栏位的 UI 显示
    /// 根据存档数据（空/非空）显示不同内容
    /// </summary>
    /// <param name="slotIndex">槽位索引</param>
    void UpdateSlotUI(int slotIndex)
    {
        // 从 SaveManager 获取该槽位的存档数据
        SaveData data = SaveManager.instance.saveSlots[slotIndex];
        
        // 获取对应的存档栏位 GameObject
        GameObject slot = saveSlotPrefabs[slotIndex];
        
        // 查找子物体中的 Text 组件（通过名字定位）
        // 注意：这些名字必须和 Unity 中创建的 Text 对象名一致
        Text timeText = slot.transform.Find("TimeText").GetComponent<Text>();
        Text playTimeText = slot.transform.Find("PlayTimeText").GetComponent<Text>();
        Text clearCountText = slot.transform.Find("ClearCountText").GetComponent<Text>();
        
        // 根据存档是否为空，显示不同内容
        if (data.isEmpty)
        {
            // 空存档：显示 "--" 占位符
            timeText.text = "Last Saved: --";
            playTimeText.text = "Play Time: -- hrs";
            clearCountText.text = "Clear Count: --";
        }
        else
        {
            // 有存档：显示实际数据
            timeText.text = "Last Saved: " + data.saveTime;
            playTimeText.text = "Play Time: " + data.playTimeHours.ToString("F1") + " hrs";
            clearCountText.text = "Clear Count: " + data.clearCount;
        }
    }
    
    /// <summary>
    /// 显示删除确认弹窗
    /// 由每个存档栏位的"删除"按钮调用
    /// </summary>
    /// <param name="slotIndex">要删除的槽位索引</param>
    public void ShowDeleteConfirm(int slotIndex)
    {
        // 记录当前要删除的槽位
        currentDeleteSlot = slotIndex;
        
        // 显示删除确认弹窗
        deleteConfirmPanel.SetActive(true);
    }
    
    /// <summary>
    /// 确认删除存档
    /// 由删除弹窗中的"Confirm"按钮调用
    /// </summary>
    public void ConfirmDelete()
    {
        // 确保有选中要删除的槽位
        if (currentDeleteSlot >= 0)
        {
            // 调用 SaveManager 删除存档
            SaveManager.instance.DeleteSlot(currentDeleteSlot);
            
            // 刷新界面，重新显示
            RefreshAllSlots();
        }
        
        // 隐藏删除确认弹窗
        deleteConfirmPanel.SetActive(false);
        
        // 重置当前删除槽位
        currentDeleteSlot = -1;
    }
    
    /// <summary>
    /// 取消删除存档
    /// 由删除弹窗中的"Cancel"按钮调用
    /// </summary>
    public void CancelDelete()
    {
        // 隐藏删除确认弹窗
        deleteConfirmPanel.SetActive(false);
        
        // 重置当前删除槽位
        currentDeleteSlot = -1;
    }
    
    /// <summary>
    /// 返回主菜单
    /// 由"Back"按钮调用
    /// </summary>
    public void ReturnToMenu()
    {
        // 加载主菜单场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}