using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using static WrathCombo.Window.Functions.UserConfig;
namespace WrathCombo.Combos.PvE;

internal partial class VPR
{
    internal static class Config
    {
        internal static void Draw(CustomComboPreset preset)
        {
            switch (preset)
            {
                case CustomComboPreset.VPR_ST_Opener:
                    DrawBossOnlyChoice(VPR_Balance_Content);

                    DrawAdditionalBoolChoice(VPR_Opener_ExcludeUF,
                        $"排除{UncoiledFury.ActionName()}", "");
                    break;

                case CustomComboPreset.VPR_ST_SerpentsIre:
                    DrawHorizontalRadioButton(VPR_ST_SerpentsIre_SubOption,
                        "全部內容", $"無論內容如何均使用{SerpentsIre.ActionName()}。", 0);

                    DrawHorizontalRadioButton(VPR_ST_SerpentsIre_SubOption,
                        "僅限Boss戰", $"僅在Boss戰中使用{SerpentsIre.ActionName()}。", 1);
                    break;

                case CustomComboPreset.VPR_ST_Reawaken:
                    DrawHorizontalRadioButton(VPR_ST_ReAwaken_SubOption,
                        "全部內容", $"無論內容如何均使用{Reawaken.ActionName()}。", 0);

                    DrawHorizontalRadioButton(VPR_ST_ReAwaken_SubOption,
                        "僅限Boss戰", $"僅在Boss戰中使用{Reawaken.ActionName()}。", 1);

                    DrawSliderInt(0, 5, VPR_ST_ReAwaken_Threshold,
                        $"設定HP百分比閾值，在可用時使用{Reawaken.ActionName()}。（僅限Boss戰）");

                    break;

                case CustomComboPreset.VPR_ST_UncoiledFury:
                    DrawSliderInt(0, 3, VPR_ST_UncoiledFury_HoldCharges,
                        $"預留多少層{UncoiledFury.ActionName()}？（0 = 全部使用）");

                    DrawSliderInt(0, 5, VPR_ST_UncoiledFury_Threshold,
                        $"設定HP百分比閾值，低於該值時全部使用{UncoiledFury.ActionName()}。");
                    break;

                case CustomComboPreset.VPR_ST_RangedUptime:
                    DrawAdditionalBoolChoice(VPR_ST_RangedUptimeUncoiledFury,
                        $"包含{UncoiledFury.ActionName()}", $"當你處於遠端狀態且擁有{RattlingCoil.ActionName()}層數時，將{UncoiledFury.ActionName()}加入迴圈。");
                    break;

                case CustomComboPreset.VPR_ST_Vicewinder:
                    DrawAdditionalBoolChoice(VPR_TrueNortVicewinder,
                        $"{Role.TrueNorth.ActionName()}選項", "可用時加入真北。");
                    break;

                case CustomComboPreset.VPR_ST_ComboHeals:
                    DrawSliderInt(0, 100, VPR_ST_SecondWind_Threshold,
                        $"{Role.SecondWind.ActionName()}觸發HP百分比閾值");

                    DrawSliderInt(0, 100, VPR_ST_Bloodbath_Threshold,
                        $"{Role.Bloodbath.ActionName()}觸發HP百分比閾值");
                    break;

                case CustomComboPreset.VPR_AoE_UncoiledFury:
                    DrawSliderInt(0, 3, VPR_AoE_UncoiledFury_HoldCharges,
                        $"預留多少層{UncoiledFury.ActionName()}？（0 = 全部使用）");

                    DrawSliderInt(0, 5, VPR_AoE_UncoiledFury_Threshold,
                        $"設定HP百分比閾值，低於該值時全部使用{UncoiledFury.ActionName()}。");
                    break;

                case CustomComboPreset.VPR_AoE_Reawaken:
                    DrawHorizontalRadioButton(VPR_AoE_Reawaken_SubOption,
                        "在範圍內", $"為{Reawaken.ActionName()}新增距離檢測，僅在目標在範圍內時使用。", 0);

                    DrawHorizontalRadioButton(VPR_AoE_Reawaken_SubOption,
                        "禁用距離檢測", $"禁用{Reawaken.ActionName()}的距離檢測，即使未選中目標也會使用。", 1);

                    DrawSliderInt(0, 100, VPR_AoE_Reawaken_Usage,
                        $"敵人HP高於該百分比時停止使用{Reawaken.ActionName()}。設為0則不檢測。");
                    break;

                case CustomComboPreset.VPR_AoE_Vicepit:
                    DrawHorizontalRadioButton(VPR_AoE_Vicepit_SubOption,
                        "在範圍內", $"為{Vicepit.ActionName()}新增距離檢測，僅在目標在範圍內時使用。", 0);

                    DrawHorizontalRadioButton(VPR_AoE_Vicepit_SubOption,
                        "禁用距離檢測", $"禁用{Vicepit.ActionName()}的距離檢測，即使未選中目標也會使用。", 1);
                    break;

                case CustomComboPreset.VPR_AoE_VicepitCombo:
                    DrawHorizontalRadioButton(VPR_AoE_VicepitCombo_SubOption,
                        "在範圍內", $"為{HuntersDen.ActionName()}與{SwiftskinsDen.ActionName()}新增距離檢測，僅在目標在範圍內時使用。", 0);

                    DrawHorizontalRadioButton(VPR_AoE_VicepitCombo_SubOption,
                        "禁用距離檢測", $"禁用{HuntersDen.ActionName()}與{SwiftskinsDen.ActionName()}的距離檢測，即使未選中目標也會使用。", 1);
                    break;

                case CustomComboPreset.VPR_AoE_ComboHeals:
                    DrawSliderInt(0, 100, VPR_AoE_SecondWind_Threshold,
                        $"{Role.SecondWind.ActionName()}觸發HP百分比閾值");

                    DrawSliderInt(0, 100, VPR_AoE_Bloodbath_Threshold,
                        $"{Role.Bloodbath.ActionName()}觸發HP百分比閾值");
                    break;

                case CustomComboPreset.VPR_ReawakenLegacy:
                    DrawRadioButton(VPR_ReawakenLegacyButton,
                        $"替換為{Reawaken.ActionName()}", $"將{Reawaken.ActionName()}替換為完整祖靈之牙-祖靈之蛇連擊。", 0);

                    DrawRadioButton(VPR_ReawakenLegacyButton,
                        $"替換為{ReavingFangs.ActionName()}", $"將{ReavingFangs.ActionName()}替換為完整祖靈之牙-祖靈之蛇連擊。", 1);

                    break;

                case CustomComboPreset.VPR_Variant_Cure:
                    DrawSliderInt(1, 100, VPR_VariantCure,
                        "HP% to be at or under", 200);
                    break;
            }
        }

        #region Variables

        public static UserInt
            VPR_Balance_Content = new("VPR_Balance_Content", 1),
            VPR_ST_SerpentsIre_SubOption = new("VPR_ST_SerpentsIre_SubOption", 1),
            VPR_ST_UncoiledFury_HoldCharges = new("VPR_ST_UncoiledFury_HoldCharges", 1),
            VPR_ST_UncoiledFury_Threshold = new("VPR_ST_UncoiledFury_Threshold", 1),
            VPR_ST_ReAwaken_SubOption = new("VPR_ST_ReAwaken_SubOption", 0),
            VPR_ST_ReAwaken_Threshold = new("VPR_ST_ReAwaken_Threshold", 1),
            VPR_ST_SecondWind_Threshold = new("VPR_ST_SecondWindThreshold", 40),
            VPR_ST_Bloodbath_Threshold = new("VPR_ST_BloodbathThreshold", 30),
            VPR_AoE_UncoiledFury_Threshold = new("VPR_AoE_UncoiledFury_Threshold", 1),
            VPR_AoE_UncoiledFury_HoldCharges = new("VPR_AoE_UncoiledFury_HoldCharges", 0),
            VPR_AoE_Vicepit_SubOption = new("VPR_AoE_Vicepit_SubOption", 0),
            VPR_AoE_VicepitCombo_SubOption = new("VPR_AoE_VicepitCombo_SubOption", 0),
            VPR_AoE_Reawaken_Usage = new("VPR_AoE_Reawaken_Usage", 20),
            VPR_AoE_Reawaken_SubOption = new("VPR_AoE_Reawaken_SubOption", 0),
            VPR_AoE_SecondWind_Threshold = new("VPR_AoE_SecondWindThreshold", 40),
            VPR_AoE_Bloodbath_Threshold = new("VPR_AoE_BloodbathThreshold", 30),
            VPR_ReawakenLegacyButton = new("VPR_ReawakenLegacyButton", 0),
            VPR_VariantCure = new("VPR_VariantCure", 50);

        public static UserBool
            VPR_Opener_ExcludeUF = new("VPR_Opener_ExcludeUF"),
            VPR_ST_RangedUptimeUncoiledFury = new("VPR_ST_RangedUptimeUncoiledFury"),
            VPR_TrueNortVicewinder = new("VPR_TrueNortVicewinder");

        #endregion
    }
}
