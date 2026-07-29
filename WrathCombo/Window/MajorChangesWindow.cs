#region

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Common.Math;
using Dalamud.Bindings.ImGui;
using WrathCombo.Core;
using WrathCombo.Services;
using Vector4 = System.Numerics.Vector4;

#endregion

namespace WrathCombo.Window;

internal class MajorChangesWindow : Dalamud.Interface.Windowing.Window
{
    /// <summary>
    ///     Create a major changes window, with some settings about it.
    /// </summary>
    public MajorChangesWindow() : base("Wrath Combo｜重大變更###Wrath Combo | New Changes")
    {
        PluginLog.Debug(
            "MajorChangesWindow: " +
            $"IsVersionProblematic: {DoesVersionHaveChange}, " +
            $"IsSuggestionHiddenForThisVersion: {IsPopupHiddenForThisVersion}, " +
            $"WasUsingOldMouseOverConfigs: {WasUsingOldMouseOverConfigs}"
        );
        if (DoesVersionHaveChange &&
            !IsPopupHiddenForThisVersion)
            IsOpen = true;

        BringToFront();

        Flags = ImGuiWindowFlags.AlwaysAutoResize;
    }

    /// <summary>
    ///     Draw the settings change suggestion window.
    /// </summary>
    public override void Draw()
    {
        PadOutMinimumWidthFor("Wrath Combo｜重大變更");

        #region MouseOver Options moved

        ImGuiEx.TextUnderlined("治療職業的滑鼠指向選項已移動！");
        if (WasUsingOldMouseOverConfigs)
            ImGuiEx.Text(ImGuiColors.DalamudYellow,
                "您原先正在使用其中一項選項，請務必閱讀！");
        ImGuiEx.Text(
            "各治療職業連段中的滑鼠指向目標檢查選項已移除，\n" +
            "改由全域滑鼠指向選項及數個新選項取代。\n\n" +
            "新設定位於：\n" +
            "設定 >「目標設定」>「治療目標優先序自訂選項」"
        );
        ImGui.NewLine();
        if (ImGui.Button("> 開啟設定分頁##majorSettings1"))
            P.OnOpenConfigUi();
        if (ImGui.Button("> 替我啟用新的介面滑鼠指向選項"))
        {
            Service.Configuration.UseUIMouseoverOverridesInDefaultHealStack = true;
            Service.Configuration.Save();
        }
        if (Service.Configuration.UseUIMouseoverOverridesInDefaultHealStack)
        {
            ImGui.SameLine();
            FontAwesome.Print(ImGuiColors.HealerGreen, FontAwesomeIcon.Check);
            ImGui.SameLine();
            ImGuiEx.Text("已啟用");
        }

        #endregion

        ImGuiEx.Spacing(new System.Numerics.Vector2(0, 10));
        ImGui.Separator();
        ImGuiEx.Spacing(new System.Numerics.Vector2(0, 10));

        #region Retargeting

        ImGuiEx.TextUnderlined("新功能：技能目標重新選擇！");
        ImGuiEx.Text(
            "技能目標重新選擇會依照 The Balance 的建議與您的設定，\n" +
            "自動為技能選擇目標，不再需要另外設定 Redirect 或 ReAction。");
        ImGuiComponents.HelpMarker(
            "過去部分功能（例如占星術士的地星）必須搭配 Redirect 或 ReAction，\n" +
            "單體治療連段也會依序檢查滑鼠指向（可選）> 軟目標 > 硬目標的 HP，\n" +
            "可能與您實際選擇的目標不一致而使用錯誤的治療技能。\n\n" +
            "技能目標重新選擇可改善這個問題！"
        );
        ImGuiEx.Text(
            "此外，現在可以自訂治療連段用來檢查 HP 並選擇治療技能的目標優先序，\n" +
            "也能讓所有單體治療技能依照同一優先序重新選擇目標。\n" +
            "（強烈建議啟用「重新選擇治療技能目標」！）");
        ImGuiEx.Text(
            "新設定位於：\n" +
            "設定 >「目標設定」（以及收合的「治療目標優先序自訂選項」）"
        );
        ImGui.NewLine();
        if (ImGui.Button("> 開啟設定分頁##majorSettings2"))
            P.OnOpenConfigUi();
        if (ImGui.Button("> 替我啟用重新選擇治療技能目標"))
        {
            Service.Configuration.RetargetHealingActionsToStack = true;
            Service.Configuration.Save();
        }
        if (Service.Configuration.RetargetHealingActionsToStack)
        {
            ImGui.SameLine();
            FontAwesome.Print(ImGuiColors.HealerGreen, FontAwesomeIcon.Check);
            ImGui.SameLine();
            ImGuiEx.Text("已啟用");
        }
        ImGui.NewLine();
        ImGuiEx.Text(
            "功能旁會以新圖示表示其中的技能是否會重新選擇目標："
        );
        ImGuiEx.Text("依設定可能重新選擇目標：");
        ImGui.SameLine();
        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudYellow))
                ImGui.Text(FontAwesomeIcon.Random.ToIconString());
        }
        ImGui.SameLine();
        ImGuiEx.Text("一定會重新選擇目標：");
        ImGui.SameLine();
        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.ParsedGreen))
                ImGui.Text(FontAwesomeIcon.Random.ToIconString());
        }
        ImGui.NewLine();
        ImGuiEx.Text(ImGuiColors.DalamudYellow,
            "若您先前已在 Redirect／ReAction 中設定目前會重新選擇目標的技能，\n" +
            "或啟用了 ReAction／Bossmod 的立即地面選取功能，建議停用這些選項。");
        ImGuiEx.Text(
            "其中包括占星卡牌、舞伴，以及（若啟用）\n" +
            "單體治療技能。");
        ImGuiComponents.HelpMarker(
            "是否為治療技能啟用此功能可依個人偏好決定（強烈建議啟用）；\n" +
            "舞伴與卡牌的處理則比單純重新選擇目標更完整，會遵循 The Balance\n" +
            "的優先序並檢查傷害降低等狀態。");

        #endregion

        #region Close and Do not Show again

        ImGuiEx.Spacing(new System.Numerics.Vector2(0, 20));
        ImGui.Separator();
        ImGuiHelpers.CenterCursorFor(
            ImGuiHelpers.GetButtonSize("關閉且不再顯示").X
            //+ ImGui.GetStyle().ItemSpacing.X * 2
        );
        if (ImGui.Button("關閉且不再顯示"))
        {
            Service.Configuration.HideMajorChangesForVersion = Version;
            Service.Configuration.Save();
            IsOpen = false;
        }

        #endregion

        if (_centeredWindow < 5)
            CenterWindow();
    }

    #region Minimum Width

    private void PadOutMinimumWidthFor(string windowName)
    {
        using (ImRaii.PushColor(ImGuiCol.Text, new Vector4(0)))
        {
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                ImGui.Text(FontAwesomeIcon.CaretDown.ToIconString());
            }

            ImGui.SameLine();
            ImGui.Text(windowName);
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                ImGui.Text(FontAwesomeIcon.Bars.ToIconString());
            }

            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                ImGui.Text(FontAwesomeIcon.Times.ToIconString());
            }
        }
    }

    #endregion

    #region Version Checking

    /// <summary>
    ///     The current plugin version.
    /// </summary>
    private static readonly Version Version =
        Svc.PluginInterface.Manifest.AssemblyVersion;

    /// <summary>
    ///     The version where the problem was introduced.
    /// </summary>
    private static readonly Version VersionWhereChangeIntroduced =
        new(1, 0, 1, 6);

    /// <summary>
    ///     Whether the current version is problematic.
    /// </summary>
    /// <remarks>No need to update this value to re-use this window.</remarks>
    private static readonly bool DoesVersionHaveChange =
        Version >= VersionWhereChangeIntroduced;

    /// <summary>
    ///     Whether the suggestion should be hidden for this version.
    /// </summary>
    private static readonly bool IsPopupHiddenForThisVersion =
        Service.Configuration.HideMajorChangesForVersion >= VersionWhereChangeIntroduced;

    #endregion

    #region Specific Info to Display for Update

    private static bool _getConfigValue(string config) =>
        PluginConfiguration.GetCustomBoolValue(config);

    /// <summary>
    ///     If the user was using MouseOver options.
    /// </summary>
    private static bool WasUsingOldMouseOverConfigs =>
        _getConfigValue("AST_ST_SimpleHeals_UIMouseOver") ||
        _getConfigValue("SCH_ST_Heal_UIMouseOver") ||
        _getConfigValue("SCH_DeploymentTactics_UIMouseOver") ||
        _getConfigValue("SGE_ST_Heal_UIMouseOver") ||
        _getConfigValue("WHM_STHeals_UIMouseOver");

    #endregion

    #region Window Centering

    private static int _centeredWindow;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left; // x position of upper-left corner
        public int Top; // y position of upper-left corner
        public int Right; // x position of lower-right corner
        public int Bottom; // y position of lower-right corner
        public Vector2 Position => new Vector2(Left, Top);
        public Vector2 Size => new Vector2(Right - Left, Bottom - Top);
    }

    /// <summary>
    ///     Centers the GUI window to the game window.
    /// </summary>
    private void CenterWindow()
    {
        // Get the pointer to the window handle.
        var hWnd = IntPtr.Zero;
        foreach (var pList in Process.GetProcesses())
            if (pList.ProcessName is "ffxiv_dx11" or "ffxiv")
                hWnd = pList.MainWindowHandle;

        // If failing to get the handle then abort.
        if (hWnd == IntPtr.Zero)
            return;

        // Get the game window rectangle
        GetWindowRect(new HandleRef(null, hWnd), out var rGameWindow);

        // Get the size of the current window.
        var vThisSize = ImGui.GetWindowSize();

        // Set the position.
        var centeredPosition = rGameWindow.Position + new Vector2(
            rGameWindow.Size.X / 2 - vThisSize.X / 2,
            rGameWindow.Size.Y / 2 - vThisSize.Y / 2);
        ImGui.SetWindowPos(centeredPosition);
        Position = centeredPosition;
        PositionCondition = ImGuiCond.Once;

        _centeredWindow++;
    }

    #endregion
}
