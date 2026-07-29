using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using ECommons;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;
using System;
using System.Linq;
using WrathCombo.Combos.PvE;
using WrathCombo.Extensions;
using WrathCombo.Services;
using WrathCombo.Services.IPC;
using WrathCombo.Services.IPC_Subscriber;

namespace WrathCombo.Window.Tabs
{
    internal class AutoRotationTab : ConfigWindow
    {
        private static uint _selectedNpc = 0;
        internal static new void Draw()
        {
            ImGui.TextWrapped("此處可設定自動循環的運作方式。標示「自動模式」核取方塊的功能可供自動循環使用。");
            ImGui.Separator();

            var cfg = Service.Configuration.RotationConfig;
            bool changed = false;

            if (P.UIHelper.ShowIPCControlledIndicatorIfNeeded())
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "啟用自動循環", ref cfg.Enabled);
            else
                changed |= ImGui.Checkbox("啟用自動循環", ref cfg.Enabled);
            if (P.IPC.GetAutoRotationState())
            {
                var inCombatOnly = (bool)P.IPC.GetAutoRotationConfigState(
                    Enum.Parse<AutoRotationConfigOption>("InCombatOnly"))!;
                ImGuiExtensions.Prefix(!inCombatOnly);
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "僅在戰鬥中", ref cfg.InCombatOnly, "InCombatOnly");

                if (inCombatOnly)
                {
                    ImGuiExtensions.Prefix(false);
                    changed |= ImGui.Checkbox("連段建議自身使用技能時略過限制", ref cfg.BypassBuffs);
                    ImGuiComponents.HelpMarker($"許多職業有可在非戰鬥狀態使用的技能，例如 {RPR.Soulsow.ActionName()} 或 {MNK.ForbiddenMeditation.ActionName()}。啟用後可在非戰鬥狀態使用這些技能。");

                    ImGuiExtensions.Prefix(false);
                    changed |= ImGui.Checkbox("任務目標可略過僅限戰鬥限制", ref cfg.BypassQuest);
                    ImGuiComponents.HelpMarker("在非戰鬥狀態停用自動模式，但位於任務目標範圍內時除外。");

                    ImGuiExtensions.Prefix(false);
                    changed |= ImGui.Checkbox("臨危受命目標可略過僅限戰鬥限制", ref cfg.BypassFATE);
                    ImGuiComponents.HelpMarker("在非戰鬥狀態停用自動模式，但已同步參加臨危受命時除外。");

                    ImGuiExtensions.Prefix(true);
                    ImGuiEx.SetNextItemWidthScaled(100);
                    changed |= ImGui.InputInt("進入戰鬥後延遲啟用自動循環（秒）", ref cfg.CombatDelay);

                    if (cfg.CombatDelay < 0)
                        cfg.CombatDelay = 0;
                }
            }

            changed |= ImGui.Checkbox("進入副本時自動啟用", ref cfg.EnableInInstance);
            changed |= ImGui.Checkbox("離開副本後停用", ref cfg.DisableAfterInstance);

            if (ImGui.CollapsingHeader("傷害設定"))
            {
                ImGuiEx.TextUnderlined("選擇目標模式");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("DPSRotationMode");
                changed |= P.UIHelper.ShowIPCControlledComboIfNeeded(
                    "###DPSTargetingMode", true, ref cfg.DPSRotationMode,
                    ref cfg.HealerRotationMode, "DPSRotationMode");

                ImGuiComponents.HelpMarker("手動：由你自行選擇目標。\n" +
                    "最高最大 HP：優先選擇最大 HP 最高的敵人。\n" +
                    "最低最大 HP：優先選擇最大 HP 最低的敵人。\n" +
                    "最高目前 HP：優先選擇目前 HP 最高的敵人。\n" +
                    "最低目前 HP：優先選擇目前 HP 最低的敵人。\n" +
                    "防護職業目標：優先選擇隊伍中第一位防護職業的目標。\n" +
                    "最近：優先選擇距離最近的目標。\n" +
                    "最遠：優先選擇距離最遠的目標。");
                ImGui.Spacing();

                if (cfg.DPSRotationMode == AutoRotation.DPSRotationMode.Manual)
                {
                    changed |= ImGui.Checkbox("強制選擇最佳範圍攻擊目標", ref cfg.DPSSettings.AoEIgnoreManual);

                    ImGuiComponents.HelpMarker("其他選擇目標模式會以可命中最多敵人的方式選擇範圍攻擊目標；手動模式只有勾選此項時才會如此運作。");
                }

                var input = ImGuiEx.InputInt(100f.Scale(), "範圍傷害功能所需目標數", ref cfg.DPSSettings.DPSAoETargets);
                if (input)
                {
                    changed |= input;
                    if (cfg.DPSSettings.DPSAoETargets < 0)
                        cfg.DPSSettings.DPSAoETargets = 0;
                }
                ImGuiComponents.HelpMarker($"Disabling this will turn off AoE DPS features. Otherwise will require the amount of targets required to be in range of an AoE feature's attack to use. This applies to all 3 roles, and for any features that deal AoE damage.");

                ImGuiEx.SetNextItemWidthScaled(100);
                changed |= ImGui.SliderFloat("最大目標距離", ref cfg.DPSSettings.MaxDistance, 1, 30);
                cfg.DPSSettings.MaxDistance =
                    Math.Clamp(cfg.DPSSettings.MaxDistance, 1, 30);

                ImGuiComponents.HelpMarker("除手動模式外，各選擇目標模式搜尋目標的最大距離。可設定為 1 至 30。");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("FATEPriority");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "優先選擇臨危受命目標", ref cfg.DPSSettings.FATEPriority, "FATEPriority");
                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("QuestPriority");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "優先選擇任務目標", ref cfg.DPSSettings.QuestPriority, "QuestPriority");
                changed |= ImGui.Checkbox("優先選擇非戰鬥中的目標", ref cfg.DPSSettings.PreferNonCombat);

                if (cfg.DPSSettings.PreferNonCombat && changed)
                    cfg.DPSSettings.OnlyAttackInCombat = false;

                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "只攻擊已進入戰鬥的目標", ref cfg.DPSSettings.OnlyAttackInCombat,
                    "OnlyAttackInCombat");

                if (cfg.DPSSettings.OnlyAttackInCombat && changed)
                    cfg.DPSSettings.PreferNonCombat = false;

                changed |= ImGui.Checkbox("不論技能需求都持續選擇目標", ref cfg.DPSSettings.AlwaysSelectTarget);

                ImGuiComponents.HelpMarker("Normally, Auto-rotation will only target an enemy if the next action it would fire needs a target. This will change the behaviour so it will always select the target regardless of what the action can target.");

                var npcs = Service.Configuration.IgnoredNPCs.ToList();
                var selected = npcs.FirstOrNull(x => x.Key == _selectedNpc);
                var prev = selected is null ? "" : $"{Svc.Data.Excel.GetSheet<BNpcName>().GetRow(selected.Value.Value).Singular} (ID: {selected.Value.Key})";
                ImGuiEx.TextUnderlined("忽略的 NPC");
                using (var combo = ImRaii.Combo("###Ignore", prev))
                {
                    if (combo)
                    {
                        if (ImGui.Selectable(""))
                        {
                            _selectedNpc = 0;
                        }

                        foreach (var npc in npcs)
                        {
                            var npcData = Svc.Data.Excel
                                .GetSheet<BNpcName>().GetRow(npc.Value);
                            if (ImGui.Selectable($"{npcData.Singular} (ID: {npc.Key})"))
                            {
                                _selectedNpc = npc.Key;
                            }
                        }
                    }
                }
                ImGuiComponents.HelpMarker("These NPCs will be ignored by Auto-Rotation.\n" +
                                           "Every instance of this NPC will be excluded from automatic targeting (Manual will still work).\n" +
                                           "To remove an NPC from this list, select it and press the Delete button below.\n" +
                                           "To add an NPC to this list, target the NPC and use the command: /wrath ignore");

                if (_selectedNpc > 0)
                {
                    if (ImGui.Button("從忽略清單移除"))
                    {
                        Service.Configuration.IgnoredNPCs.Remove(_selectedNpc);
                        Service.Configuration.Save();

                        _selectedNpc = 0;
                    }
                }

            }
            ImGui.Spacing();
            if (ImGui.CollapsingHeader("治療設定"))
            {
                ImGuiEx.TextUnderlined("治療目標模式");
                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("HealerRotationMode");
                changed |= P.UIHelper.ShowIPCControlledComboIfNeeded(
                    "###HealerTargetingMode", false, ref cfg.DPSRotationMode,
                    ref cfg.HealerRotationMode, "HealerRotationMode");
                ImGuiComponents.HelpMarker("Manual - Will only heal a target if you select them manually. If the target does not meet the healing threshold settings criteria below it will skip healing in favour of DPSing (if also enabled).\n" +
                    "Highest Current - Prioritises the party member with the highest current HP%.\n" +
                    "Lowest Current - Prioritises the party member with the lowest current HP%.");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("SingleTargetHPP");
                changed |= P.UIHelper.ShowIPCControlledSliderIfNeeded(
                    "單體治療 HP% 閾值", ref cfg.HealerSettings.SingleTargetHPP, "SingleTargetHPP");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("SingleTargetRegenHPP");
                changed |= P.UIHelper.ShowIPCControlledSliderIfNeeded(
                    "單體治療 HP% 閾值（目標已有持續恢復效果）", ref cfg.HealerSettings.SingleTargetRegenHPP, "SingleTargetRegenHPP");
                ImGuiComponents.HelpMarker("一般建議將此值設得比上一項低。");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("AoETargetHPP");
                changed |= P.UIHelper.ShowIPCControlledSliderIfNeeded(
                    "範圍治療 HP% 閾值", ref cfg.HealerSettings.AoETargetHPP, "AoETargetHPP");

                var input = ImGuiEx.InputInt(100f.Scale(), "範圍治療功能所需目標數", ref cfg.HealerSettings.AoEHealTargetCount);
                if (input)
                {
                    changed |= input;
                    if (cfg.HealerSettings.AoEHealTargetCount < 0)
                        cfg.HealerSettings.AoEHealTargetCount = 0;
                }
                ImGuiComponents.HelpMarker($"Disabling this will turn off AoE Healing features. Otherwise will require the amount of targets required to be in range of an AoE feature's heal to use.");
                ImGuiEx.SetNextItemWidthScaled(100);
                changed |= ImGui.InputInt("符合條件後延遲開始治療（秒）", ref cfg.HealerSettings.HealDelay);

                if (cfg.HealerSettings.HealDelay < 0)
                    cfg.HealerSettings.HealDelay = 0;
                ImGuiComponents.HelpMarker("Don't set this too high! 1-2 seconds is normally comfy enough to be considered a natural reaction.");

                ImGui.Spacing();

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("AutoRez");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    "自動復活", ref cfg.HealerSettings.AutoRez, "AutoRez");
                ImGuiComponents.HelpMarker($"Will attempt to resurrect dead party members. Applies to {WHM.ClassID.JobAbbreviation()}, {WHM.JobID.JobAbbreviation()}, {SCH.JobID.JobAbbreviation()}, {AST.JobID.JobAbbreviation()}, {SGE.JobID.JobAbbreviation()} and {OccultCrescent.ContentName} {Svc.Data.GetExcelSheet<MKDSupportJob>().GetRow(10).Unknown0} {OccultCrescent.Revive.ActionName()}");
                var autoRez = (bool)P.IPC.GetAutoRotationConfigState(AutoRotationConfigOption.AutoRez)!;
                if (autoRez)
                {
                    ImGuiExtensions.Prefix(false);
                    changed |= ImGui.Checkbox("套用至隊伍外的玩家", ref cfg.HealerSettings.AutoRezOutOfParty);

                    ImGuiExtensions.Prefix(false);
                    changed |= ImGui.Checkbox("需要即刻詠唱／連續詠唱", ref
                        cfg.HealerSettings.AutoRezRequireSwift);
                    ImGuiComponents.HelpMarker(
                        $"Requires {RoleActions.Magic.Swiftcast.ActionName()} " +
                        $"(or {RDM.JobID.JobAbbreviation()}'s Dualcast) " +
                        $"to be available to resurrect a party member, to avoid hard-casting.");

                    ImGuiExtensions.Prefix(true);
                    P.UIHelper.ShowIPCControlledIndicatorIfNeeded("AutoRezDPSJobs");
                    changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                        $"Apply to {SMN.JobID.JobAbbreviation()} & {RDM.JobID.JobAbbreviation()}", ref cfg.HealerSettings.AutoRezDPSJobs, "AutoRezDPSJobs");
                    ImGuiComponents.HelpMarker($"When playing as {SMN.JobID.JobAbbreviation()} or {RDM.JobID.JobAbbreviation()}, also attempt to raise a dead party member. {RDM.JobID.JobAbbreviation()} will only resurrect with {RoleActions.Magic.Buffs.Swiftcast.StatusName()} or {RDM.Buffs.Dualcast.StatusName()} active.");
                }

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("AutoCleanse");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    $"Auto-{RoleActions.Healer.Esuna.ActionName()}", ref cfg.HealerSettings.AutoCleanse, "AutoCleanse");
                ImGuiComponents.HelpMarker($"Will {RoleActions.Healer.Esuna.ActionName()} any cleansable debuffs (Healing takes priority).");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("ManageKardia");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded(
                    $"[{SGE.JobID.JobAbbreviation()}] Automatically Manage Kardia", ref cfg.HealerSettings.ManageKardia, "ManageKardia");
                ImGuiComponents.HelpMarker($"Switches {SGE.Kardia.ActionName()} to party members currently being targeted by enemies, prioritising tanks if multiple people are being targeted.");
                if (cfg.HealerSettings.ManageKardia)
                {
                    ImGuiExtensions.Prefix(cfg.HealerSettings.ManageKardia);
                    changed |= ImGui.Checkbox($"Limit {SGE.Kardia.ActionName()} swapping to tanks only", ref cfg.HealerSettings.KardiaTanksOnly);
                }

                changed |= ImGui.Checkbox($"[{WHM.JobID.JobAbbreviation()}/{AST.JobID.JobAbbreviation()}] Pre-emptively apply heal over time on focus target", ref cfg.HealerSettings.PreEmptiveHoT);
                ImGuiComponents.HelpMarker($"Applies {WHM.Regen.ActionName()}/{AST.AspectedBenefic.ActionName()} to your focus target when out of combat and they are 30y or less away from an enemy. (Bypasses \"Only in Combat\" setting)");

                P.UIHelper.ShowIPCControlledIndicatorIfNeeded("IncludeNPCs");
                changed |= P.UIHelper.ShowIPCControlledCheckboxIfNeeded("治療友方 NPC", ref cfg.HealerSettings.IncludeNPCs);
                ImGuiComponents.HelpMarker("適用於需要治療未加入隊伍之 NPC 的治療職業任務。");

            }

            ImGuiEx.TextUnderlined("進階");
            changed |= ImGui.InputInt("節流延遲（毫秒）", ref cfg.Throttler);
            ImGuiComponents.HelpMarker("Auto-Rotation has a built in throttler to only run every so many milliseconds for performance reasons. If you experience issues with frame rate, try increasing this value. Do note this may have a side-effect of introducing clipping if set too high, so experiment with the value.");

            using (ImRaii.Disabled(!OrbwalkerIPC.IsEnabled))
            {
                changed |= ImGui.Checkbox("啟用 Orbwalker 整合", ref cfg.OrbwalkerIntegration);

                ImGuiComponents.HelpMarker($"This will make Auto-Rotation use actions with cast times even whilst moving, as Orbwalker will lock movement during the cast.");
            }

            if (changed)
                Service.Configuration.Save();

        }
    }
}
