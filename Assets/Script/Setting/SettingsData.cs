/// <summary>
/// 游戏设置数据结构
/// </summary>
[System.Serializable]
public class SettingsData
{
    // ========== 画面设置 ==========
    
    /// <summary>
    /// 分辨率宽度
    /// </summary>
    public int resolutionWidth = 1920;
    
    /// <summary>
    /// 分辨率高度
    /// </summary>
    public int resolutionHeight = 1080;
    
    /// <summary>
    /// 最大帧数（30/60/120）
    /// </summary>
    public int maxFPS = 60;
    
    /// <summary>
    /// 是否全屏
    /// </summary>
    public bool isFullscreen = false;
    
    // ========== 音频设置 ==========
    
    /// <summary>
    /// 主音量（0~100）
    /// </summary>
    public int masterVolume = 50;
    
    /// <summary>
    /// 音乐音量（0~100）
    /// </summary>
    public int musicVolume = 50;
    
    /// <summary>
    /// 音效音量（0~100）
    /// </summary>
    public int sfxVolume = 50;
}