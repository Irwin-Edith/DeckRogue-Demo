# 更新日志 (Changelog)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/lang/zh-CN/spec/v2.0.0.html).

---

## [v1.0.0-alpha] - 2026-06-24

### Added

- **主菜单系统**

  - PlayGame() - 进入角色选择场景
  - OpenProfile() - 进入存档界面
  - OpenSettings() - 进入设置界面
  - QuitGame() - 退出游戏
- **设置界面系统**

  - 双页签切换（画面设置 / 音频设置）
  - 画面设置
    - 分辨率下拉框（自动读取系统可用分辨率）
    - 帧率下拉框（30 / 60 / 120）
    - 全屏开关（全屏时禁用分辨率选择）
  - 音频设置
    - 主音量滑块（0-100，影响所有音频）
    - 音乐音量滑块（0-100）
    - 音效音量滑块（0-100）
    - AudioMixer 集成（MasterVolume / MusicVolume / SFXVolume）
- **存档系统**

  - SaveManager（单例模式，跨场景持久化）
  - SaveData 数据结构
    - saveTime - 最后保存时间
    - playTimeHours - 游戏时长
    - clearCount - 通关次数
  - 3 槽位存档（save_0.json / save_1.json / save_2.json）
  - ProfileUI 界面控制器
    - 显示/刷新存档列表
    - 删除存档（带确认弹窗）
    - 返回主菜单
- **退出确认弹窗**

  - ShowConfirmPanel() - 显示确认面板
  - HideConfirmPanel() - 隐藏确认面板
  - ConfirmQuit() - 确认退出游戏

### Fixed

- SettingsData.cs: 移除 Screen 依赖，改为默认值
- SettingsUI.cs: Screen.fullscreen → Screen.fullScreen（API 修正）
- 适配 TextMeshPro（TMP_Dropdown / TMP_Text）

### 待开发

- ChooseCharacter 角色选择场景
- 存档保存/加载逻辑接入实际游戏数据
- 设置数据持久化（保存/读取配置文件）

---

## 文件结构

### 脚本文件

| 模块   | 文件                 | 说明           |
| ------ | -------------------- | -------------- |
| 主菜单 | `MainMenuUI.cs`    | 主菜单导航控制 |
| 主菜单 | `QuitConfirmUI.cs` | 退出确认弹窗   |
| 设置   | `SettingsUI.cs`    | 设置界面控制器 |
| 设置   | `SettingsData.cs`  | 设置数据结构   |
| 存档   | `SaveManager.cs`   | 存档管理器     |
| 存档   | `SaveData.cs`      | 存档数据结构   |
| 存档   | `ProfileUI.cs`     | 存档界面控制器 |

### 场景文件

| 场景                      | 说明               |
| ------------------------- | ------------------ |
| `Menu.unity`            | 主菜单             |
| `Settings.unity`        | 设置界面           |
| `Profile.unity`         | 存档界面           |
| `ChooseCharacter.unity` | 角色选择（待开发） |

---

## 技术栈

- Unity 2022.x
- TextMeshPro
- URP (Universal Render Pipeline)
