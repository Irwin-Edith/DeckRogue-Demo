using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// 设置界面控制器
/// 负责管理画面设置和音频设置两个页签的切换，以及各项设置的实时应用
/// </summary>
public class SettingsUI : MonoBehaviour
{
    // ========== UI 组件引用 ==========

    /// <summary>
    /// 页签按钮 - 用于切换显示的设置面板
    /// </summary>
    public Button graphicsTab;
    public Button audioTab;

    /// <summary>
    /// 设置面板 - 用于切换面板的显示/隐藏
    /// </summary>
    public GameObject graphicsPanel;
    public GameObject audioPanel;

    // ========== 画面设置 UI ==========

    /// <summary>
    /// 分辨率下拉框
    /// </summary>
    public TMP_Dropdown resolutionDropdown;

    /// <summary>
    /// 最大帧数下拉框
    /// </summary>
    public TMP_Dropdown fpsDropdown;

    /// <summary>
    /// 全屏开关
    /// </summary>
    public Toggle fullscreenToggle;

    // ========== 音频设置 UI ==========

    /// <summary>
    /// 主音量滑块和数值显示
    /// </summary>
    public Slider masterSlider;
    public TMP_Text masterValueText;

    /// <summary>
    /// 音乐音量滑块和数值显示
    /// </summary>
    public Slider musicSlider;
    public TMP_Text musicValueText;

    /// <summary>
    /// 音效音量滑块和数值显示
    /// </summary>
    public Slider sfxSlider;
    public TMP_Text sfxValueText;

    // ========== 音频混合器 ==========

    /// <summary>
    /// 音频混合器
    /// </summary>
    public AudioMixer audioMixer;

    // ========== 私有变量 ==========

    private SettingsData currentSettings;
    private Resolution[] availableResolutions;
    private int currentResolutionIndex = -1;

    // ========== 初始化 ==========

    void Start()
    {
        currentSettings = new SettingsData();
        SetupResolutionDropdown();
        SetupFPSDropdown();
        LoadSettingsToUI();
        ShowGraphicsPanel();
    }

    // ========== 分辨率下拉框初始化 ==========

    void SetupResolutionDropdown()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        currentResolutionIndex = -1;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            var res = availableResolutions[i];
            string option = $"{res.width} × {res.height}";
            options.Add(option);

            if (res.width == currentSettings.resolutionWidth &&
                res.height == currentSettings.resolutionHeight)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    // ========== 帧数下拉框初始化 ==========

    void SetupFPSDropdown()
    {
        int[] fpsOptions = { 30, 60, 120 };

        fpsDropdown.ClearOptions();
        var options = new List<string>();

        foreach (var fps in fpsOptions)
        {
            options.Add(fps.ToString());
        }

        fpsDropdown.AddOptions(options);

        int currentFPSIndex = System.Array.IndexOf(fpsOptions, currentSettings.maxFPS);
        if (currentFPSIndex < 0) currentFPSIndex = 1;
        fpsDropdown.value = currentFPSIndex;

        fpsDropdown.onValueChanged.AddListener(OnFPSChanged);
    }

    // ========== 从设置加载到 UI ==========

    void LoadSettingsToUI()
    {
        fullscreenToggle.isOn = currentSettings.isFullscreen;
        OnFullscreenChanged(currentSettings.isFullscreen);

        masterSlider.value = currentSettings.masterVolume;
        musicSlider.value = currentSettings.musicVolume;
        sfxSlider.value = currentSettings.sfxVolume;

        OnMasterVolumeChanged(currentSettings.masterVolume);
        OnMusicVolumeChanged(currentSettings.musicVolume);
        OnSFXVolumeChanged(currentSettings.sfxVolume);
    }

    // ========== 页签切换 ==========

    public void ShowGraphicsPanel()
    {
        graphicsPanel.SetActive(true);
        audioPanel.SetActive(false);

        graphicsTab.interactable = false;
        audioTab.interactable = true;
    }

    public void ShowAudioPanel()
    {
        graphicsPanel.SetActive(false);
        audioPanel.SetActive(true);

        graphicsTab.interactable = true;
        audioTab.interactable = false;
    }

    // ========== 画面设置回调 ==========

    public void OnResolutionChanged(int index)
    {
        if (index < 0 || index >= availableResolutions.Length) return;

        var res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);

        currentSettings.resolutionWidth = res.width;
        currentSettings.resolutionHeight = res.height;
    }

    public void OnFPSChanged(int index)
    {
        int[] fpsOptions = { 30, 60, 120 };
        int newFPS = fpsOptions[index];

        Application.targetFrameRate = newFPS;
        currentSettings.maxFPS = newFPS;
    }

    public void OnFullscreenChanged(bool isFullscreen)
    {
        resolutionDropdown.interactable = !isFullscreen;

        if (isFullscreen)
        {
            resolutionDropdown.captionText.text = "N/A";
        }
        else
        {
            resolutionDropdown.captionText.text =
                $"{currentSettings.resolutionWidth} × {currentSettings.resolutionHeight}";
        }

        Screen.fullScreen = isFullscreen;
        currentSettings.isFullscreen = isFullscreen;
    }

    // ========== 音频设置回调 ==========

    public void OnMasterVolumeChanged(float value)
    {
        int vol = Mathf.RoundToInt(value);
        masterValueText.text = vol.ToString();
        currentSettings.masterVolume = vol;

        float db = VolumeToDb(vol);
        audioMixer.SetFloat("MasterVolume", db);

        if (vol == 0)
        {
            audioMixer.SetFloat("MusicVolume", -80f);
            audioMixer.SetFloat("SFXVolume", -80f);
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        int vol = Mathf.RoundToInt(value);
        musicValueText.text = vol.ToString();
        currentSettings.musicVolume = vol;

        if (currentSettings.masterVolume > 0)
        {
            float db = VolumeToDb(vol);
            audioMixer.SetFloat("MusicVolume", db);
        }
    }

    public void OnSFXVolumeChanged(float value)
    {
        int vol = Mathf.RoundToInt(value);
        sfxValueText.text = vol.ToString();
        currentSettings.sfxVolume = vol;

        if (currentSettings.masterVolume > 0)
        {
            float db = VolumeToDb(vol);
            audioMixer.SetFloat("SFXVolume", db);
        }
    }

    float VolumeToDb(int volume)
    {
        if (volume == 0) return -80f;
        return Mathf.Log10(volume / 100f) * 20f;
    }

    // ========== 返回主菜单 ==========

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}