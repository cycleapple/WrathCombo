using Dalamud.Interface.Colors;
using Dalamud.Bindings.ImGui;
using WrathCombo.CustomComboNS.Functions;
using static WrathCombo.Window.Functions.UserConfig;

namespace WrathCombo.Combos.PvE;

internal partial class BRD
{
    internal static class Config
    {
        public static UserInt
            BRD_RagingJawsRenewTime = new("ragingJawsRenewTime"),            
            BRD_STSecondWindThreshold = new("BRD_STSecondWindThreshold"),
            BRD_AoESecondWindThreshold = new("BRD_AoESecondWindThreshold"),
            BRD_VariantCure = new("BRD_VariantCure"),
            BRD_Adv_Opener_Selection = new("BRD_Adv_Opener_Selection", 0),
            BRD_Balance_Content = new("BRD_Balance_Content", 1),
            BRD_Adv_DoT_Threshold = new("BRD_Adv_DoT_Threshold", 1),
            BRD_Adv_DoT_SubOption = new("BRD_Adv_DoT_SubOption", 1),
            BRD_Adv_Buffs_Threshold = new ("BRD_Adv_Buffs_Threshold", 1),
            BRD_Adv_Buffs_SubOption = new ("BRD_Adv_Buffs_SubOption", 1),
            BRD_AoE_Adv_Buffs_Threshold = new("BRD_AoE_Adv_Buffs_Threshold", 1),
            BRD_AoE_Adv_Buffs_SubOption = new("BRD_AoE_Adv_Buffs_SubOption", 1);

        internal static void Draw(CustomComboPreset preset)
        {
            switch (preset)
            {
                case CustomComboPreset.BRD_ST_Adv_Balance_Standard:
                    DrawRadioButton(BRD_Adv_Opener_Selection, "Standard Opener", "", 0);
                    DrawRadioButton(BRD_Adv_Opener_Selection, "2.48 Adjusted Standard Opener", "", 1);
                    DrawRadioButton(BRD_Adv_Opener_Selection, "2.49 Standard Comfy", "", 2);

                    ImGui.Indent();
                    DrawBossOnlyChoice(BRD_Balance_Content);
                    ImGui.Unindent();
                    break;

                case CustomComboPreset.BRD_Adv_RagingJaws:
                    DrawSliderInt(3, 10, BRD_RagingJawsRenewTime,
                        "Remaining time (In seconds). Recommended 5, increase little by little if refresh is outside of radiant window");

                    break;


                case CustomComboPreset.BRD_Adv_DoT:

                    DrawSliderInt(0, 100, BRD_Adv_DoT_Threshold,
                        $"Stop using Dots on targets below this HP % (0% = always use, 100% = never use).");

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow, "選擇要套用 HP 檢查的敵人類型：");

                    DrawHorizontalRadioButton(BRD_Adv_DoT_SubOption,
                        "\u50c5\u9650\u975e\u9996\u9818\u6230", $"\u50c5\u5c0d\u975e\u9996\u9818\u6575\u4eba\u61c9\u7528 HP \u6aa2\u67e5", 0);

                    DrawHorizontalRadioButton(BRD_Adv_DoT_SubOption,
                        "\u5168\u90e8\u5167\u5bb9", $"\u5c0d\u6240\u6709\u5167\u5bb9\u61c9\u7528 HP \u6aa2\u6e2c", 1);

                    ImGui.Unindent();

                    break;

                case CustomComboPreset.BRD_Adv_Buffs:

                    DrawSliderInt(0, 100, BRD_Adv_Buffs_Threshold,
                       $"Stop using Buffs on targets below this HP % (0% = always use, 100% = never use).");

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow, "選擇要套用 HP 檢查的敵人類型：");

                    DrawHorizontalRadioButton(BRD_Adv_Buffs_SubOption,
                        "\u50c5\u9650\u975e\u9996\u9818\u6230", $"\u50c5\u5c0d\u975e\u9996\u9818\u6575\u4eba\u61c9\u7528 HP \u6aa2\u67e5", 0);

                    DrawHorizontalRadioButton(BRD_Adv_Buffs_SubOption,
                        "\u5168\u90e8\u5167\u5bb9", $"\u5c0d\u6240\u6709\u5167\u5bb9\u61c9\u7528 HP \u6aa2\u6e2c", 1);

                    ImGui.Unindent();

                    break;

                case CustomComboPreset.BRD_AoE_Adv_Buffs:

                    DrawSliderInt(0, 100, BRD_AoE_Adv_Buffs_Threshold,
                        $"Stop using Buffs on targets below this HP % (0% = always use, 100% = never use).");

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow, "選擇要套用 HP 檢查的敵人類型：");

                    DrawHorizontalRadioButton(BRD_AoE_Adv_Buffs_SubOption,
                        "\u50c5\u9650\u975e\u9996\u9818\u6230", $"\u50c5\u5c0d\u975e\u9996\u9818\u6575\u4eba\u61c9\u7528 HP \u6aa2\u67e5", 0);

                    DrawHorizontalRadioButton(BRD_AoE_Adv_Buffs_SubOption,
                        "\u5168\u90e8\u5167\u5bb9", $"\u5c0d\u6240\u6709\u5167\u5bb9\u61c9\u7528 HP \u6aa2\u6e2c", 1);

                    ImGui.Unindent();

                    break;

                case CustomComboPreset.BRD_ST_SecondWind:
                    DrawSliderInt(0, 100, BRD_STSecondWindThreshold,
                        "低於該HP百分比時使用內丹。");

                    break;

                case CustomComboPreset.BRD_AoE_SecondWind:
                    DrawSliderInt(0, 100, BRD_AoESecondWindThreshold,
                        "低於該HP百分比時使用內丹。");

                    break;

                case CustomComboPreset.BRD_Variant_Cure:
                    DrawSliderInt(1, 100, BRD_VariantCure, "HP% to be at or under", 200);

                    break;
            }
        }
    }
}
