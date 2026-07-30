using Dalamud.Bindings.ImGui;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using WrathCombo.Window.Functions;
using static WrathCombo.Window.Functions.UserConfig;
namespace WrathCombo.Combos.PvE;

internal partial class SAM
{
    internal static class Config
    {
        internal static void Draw(CustomComboPreset preset)
        {
            switch (preset)
            {
                case CustomComboPreset.SAM_ST_Opener:
                    DrawBossOnlyChoice(SAM_Balance_Content);
                    ImGui.NewLine();
                    DrawSliderInt(0, 13, SAM_Opener_PrePullDelay,
                        $"從首次{MeikyoShisui.ActionName()}到下一步的延遲（秒）\n此延遲透過將你的按鈕替換為狂怒劍來強制執行。");
                    break;

                case CustomComboPreset.SAM_ST_CDs_Iaijutsu:
                    DrawHorizontalMultiChoice(SAM_ST_CDs_IaijutsuOption, $"Add {Higanbana.ActionName()}", "\u6839\u64da\u5b50\u9078\u9805\u4f7f\u7528\u201c\u5f7c\u5cb8\u82b1\u201d\u3002", 4, 0);
                    DrawHorizontalMultiChoice(SAM_ST_CDs_IaijutsuOption, $"Add {TenkaGoken.ActionName()}", "\u7b49\u7d1a\u540c\u6b65\u4f4e\u65bc 50 \u7d1a\u6642\u4f7f\u7528\u201c\u5929\u4e0b\u4e94\u528d\u201d\u3002", 4, 1);
                    DrawHorizontalMultiChoice(SAM_ST_CDs_IaijutsuOption, $"Use {MidareSetsugekka.ActionName()}", "\u6703\u4f7f\u7528\u201c\u7d1b\u4e82\u96ea\u6708\u82b1\u201d\u548c\u201c\u5929\u9053\u96ea\u6708\u82b1\u201d\u3002", 4, 2);
                    DrawHorizontalMultiChoice(SAM_ST_CDs_IaijutsuOption, $"Use {TsubameGaeshi.ActionName()}", "\u6703\u4f7f\u7528\u201c\u71d5\u56de\u8fd4\u201d\u548c\u201c\u5929\u9053\u56de\u8fd4\u96ea\u6708\u82b1\u201d\u3002", 4, 3);

                    if (SAM_ST_CDs_IaijutsuOption[0])
                    {
                        ImGui.Indent();
                        DrawHorizontalRadioButton(SAM_ST_Higanbana_Suboption,
                            "\u6240\u6709\u6575\u4eba", $"Uses {Higanbana.ActionName()} regardless of targeted enemy type.", 0);

                        DrawHorizontalRadioButton(SAM_ST_Higanbana_Suboption,
                            "Bosses Only", $"Only uses {Higanbana.ActionName()} when the targeted enemy is a boss.", 1);
                        ImGui.Unindent();

                        DrawSliderInt(0, 10, SAM_ST_Higanbana_HP_Threshold,
                            $"Stop using {Higanbana.ActionName()} on targets below this HP % (0% = always use).");

                        DrawSliderInt(0, 15, SAM_ST_Higanbana_Refresh,
                            $"Seconds remaining before reapplying {Higanbana.ActionName()}. Set to Zero to disable this check.");
                    }
                    break;

                case CustomComboPreset.SAM_ST_CDs_MeikyoShisui:
                    DrawHorizontalRadioButton(SAM_ST_Meikyo_Suboption,
                        "Use The Balance Logic in all content", $"Uses {MeikyoShisui.ActionName()} logic regardless of content.", 0);

                    DrawHorizontalRadioButton(SAM_ST_Meikyo_Suboption,
                        "Use The Balance logic only in Boss encounters", $"Only uses {MeikyoShisui.ActionName()} logic when in Boss encounters." +
                                                                         $"\nWill use Meikyo every minute regardless of sen count outside of boss encounters.", 1);
                    break;

                case CustomComboPreset.SAM_ST_ComboHeals:
                    DrawSliderInt(0, 100, SAM_STSecondWindThreshold,
                        $"{Role.SecondWind.ActionName()} HP percentage threshold");

                    DrawSliderInt(0, 100, SAM_STBloodbathThreshold,
                        $"{Role.Bloodbath.ActionName()} HP percentage threshold");
                    break;

                case CustomComboPreset.SAM_AoE_ComboHeals:
                    DrawSliderInt(0, 100, SAM_AoESecondWindThreshold,
                        $"{Role.SecondWind.ActionName()} HP percentage threshold");

                    DrawSliderInt(0, 100, SAM_AoEBloodbathThreshold,
                        $"{Role.Bloodbath.ActionName()} HP percentage threshold");
                    break;

                case CustomComboPreset.SAM_ST_CDs_Senei:
                    DrawAdditionalBoolChoice(SAM_ST_CDs_Guren,
                        "紅蓮", "未解鎖必殺劍·閃影時，加入必殺劍·紅蓮到迴圈。");
                    break;

                case CustomComboPreset.SAM_ST_CDs_OgiNamikiri:
                    DrawAdditionalBoolChoice(SAM_ST_CDs_OgiNamikiri_Movement,
                        "移動檢測", "站立時加入奧義斬浪與回返斬浪。");
                    break;

                case CustomComboPreset.SAM_ST_Shinten:
                    DrawSliderInt(25, 85, SAM_ST_KenkiOvercapAmount,
                        "單體連段劍氣溢位閾值設定");

                    DrawSliderInt(0, 100, SAM_ST_ExecuteThreshold,
                        "不保留劍氣的血量百分比閾值");
                    break;

                case CustomComboPreset.SAM_AoE_Kyuten:
                    DrawSliderInt(25, 85, SAM_AoE_KenkiOvercapAmount,
                        "AOE連段劍氣溢位閾值設定");
                    break;

                case CustomComboPreset.SAM_ST_GekkoCombo:
                    DrawAdditionalBoolChoice(SAM_Gekko_KenkiOvercap,
                        "劍氣溢位保護", "劍氣達到設定值時消耗。");

                    if (SAM_Gekko_KenkiOvercap)
                        DrawSliderInt(25, 100, SAM_Gekko_KenkiOvercapAmount,
                            "劍氣數值", sliderIncrement: SliderIncrements.Fives);
                    break;

                case CustomComboPreset.SAM_ST_KashaCombo:
                    DrawAdditionalBoolChoice(SAM_Kasha_KenkiOvercap,
                        "劍氣溢位保護", "劍氣達到設定值時消耗。");

                    if (SAM_Kasha_KenkiOvercap)
                        DrawSliderInt(25, 100, SAM_Kasha_KenkiOvercapAmount,
                            "劍氣數值", sliderIncrement: SliderIncrements.Fives);
                    break;

                case CustomComboPreset.SAM_ST_YukikazeCombo:
                    DrawAdditionalBoolChoice(SAM_Yukaze_KenkiOvercap,
                        "劍氣溢位保護", "劍氣達到設定值時消耗。");

                    if (SAM_Yukaze_KenkiOvercap)
                        DrawSliderInt(25, 100, SAM_Yukaze_KenkiOvercapAmount,
                            "劍氣數值", sliderIncrement: SliderIncrements.Fives);
                    break;

                case CustomComboPreset.SAM_AoE_OkaCombo:
                    DrawAdditionalBoolChoice(SAM_Oka_KenkiOvercap,
                        "劍氣溢位保護", "劍氣達到設定值時消耗。");

                    if (SAM_Oka_KenkiOvercap)
                        DrawSliderInt(25, 100, SAM_Oka_KenkiOvercapAmount,
                            "劍氣數值", sliderIncrement: SliderIncrements.Fives);
                    break;

                case CustomComboPreset.SAM_AoE_MangetsuCombo:
                    DrawAdditionalBoolChoice(SAM_Mangetsu_KenkiOvercap,
                        "劍氣溢位保護", "劍氣達到設定值時消耗。");

                    if (SAM_Mangetsu_KenkiOvercap)
                        DrawSliderInt(25, 100, SAM_Mangetsu_KenkiOvercapAmount,
                            "劍氣數值", sliderIncrement: SliderIncrements.Fives);
                    break;

                case CustomComboPreset.SAM_Variant_Cure:
                    DrawSliderInt(1, 100, SAM_VariantCure,
                        "HP% to be at or under", 200);
                    break;
            }
        }
        #region Variables

        public static UserInt
            SAM_Balance_Content = new("SAM_Balance_Content", 1),
            SAM_Opener_PrePullDelay = new("SAM_Opener_PrePullDelay", 13),
            SAM_ST_KenkiOvercapAmount = new("SAM_ST_KenkiOvercapAmount", 65),
            SAM_ST_Higanbana_Suboption = new("SAM_ST_Higanbana_Suboption", 1),
            SAM_ST_Meikyo_Suboption = new("SAM_ST_Meikyo_Suboption", 1),
            SAM_ST_Higanbana_HP_Threshold = new("SAM_ST_Higanbana_HP_Threshold", 0),
            SAM_ST_Higanbana_Refresh = new("SAM_ST_Higanbana_Refresh", 15),
            SAM_ST_ExecuteThreshold = new("SAM_ST_ExecuteThreshold", 1),
            SAM_STSecondWindThreshold = new("SAM_STSecondWindThreshold", 40),
            SAM_STBloodbathThreshold = new("SAM_STBloodbathThreshold", 30),
            SAM_AoE_KenkiOvercapAmount = new("SAM_AoE_KenkiOvercapAmount", 50),
            SAM_AoESecondWindThreshold = new("SAM_AoESecondWindThreshold", 40),
            SAM_AoEBloodbathThreshold = new("SAM_AoEBloodbathThreshold", 30),
            SAM_Gekko_KenkiOvercapAmount = new("SAM_Gekko_KenkiOvercapAmount", 65),
            SAM_Kasha_KenkiOvercapAmount = new("SAM_Kasha_KenkiOvercapAmount", 65),
            SAM_Yukaze_KenkiOvercapAmount = new("SAM_Yukaze_KenkiOvercapAmount", 65),
            SAM_Oka_KenkiOvercapAmount = new("SAM_Oka_KenkiOvercapAmount", 50),
            SAM_Mangetsu_KenkiOvercapAmount = new("SAM_Mangetsu_KenkiOvercapAmount", 50),
            SAM_VariantCure = new("SAM_VariantCure", 50);

        public static UserBool
            SAM_Gekko_KenkiOvercap = new("SAM_Gekko_KenkiOvercap"),
            SAM_Kasha_KenkiOvercap = new("SAM_Kasha_KenkiOvercap"),
            SAM_Yukaze_KenkiOvercap = new("SAM_Yukaze_KenkiOvercap"),
            SAM_ST_CDs_Guren = new("SAM_ST_CDs_Guren"),
            SAM_ST_CDs_OgiNamikiri_Movement = new("SAM_ST_CDs_OgiNamikiri_Movement"),
            SAM_Oka_KenkiOvercap = new("SAM_Oka_KenkiOvercap"),
            SAM_Mangetsu_KenkiOvercap = new("SAM_Mangetsu_KenkiOvercap");

        public static UserBoolArray
            SAM_ST_CDs_IaijutsuOption = new("SAM_ST_CDs_IaijutsuOption");

        #endregion
    }
}
