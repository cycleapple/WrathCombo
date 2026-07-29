using System;
using System.Linq;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using System.Numerics;
using Dalamud.Interface.Colors;
using ECommons.GameHelpers;
using ECommons.ImGuiMethods;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Services;
using WrathCombo.Window.Functions;

namespace WrathCombo.Window.Tabs
{
    internal class Settings : ConfigWindow
    {
        internal new static void Draw()
        {
            using (ImRaii.Child("main", new Vector2(0, 0), true))
            {
                ImGui.Text("此分頁可自訂 Wrath Combo 的各項設定。");

                #region UI Options

                ImGuiEx.Spacing(new Vector2(0, 20));
                ImGuiEx.TextUnderlined("主要介面設定");

                #region SubCombos

                var hideChildren = Service.Configuration.HideChildren;

                if (ImGui.Checkbox("隱藏子連段選項", ref hideChildren))
                {
                    Service.Configuration.HideChildren = hideChildren;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker("隱藏已停用功能的子選項。");

                #endregion

                #region Conflicting

                bool hideConflicting = Service.Configuration.HideConflictedCombos;
                if (ImGui.Checkbox("隱藏衝突的連段", ref hideConflicting))
                {
                    Service.Configuration.HideConflictedCombos = hideConflicting;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker("隱藏與其他已選功能衝突的連段。");

                #endregion

                #region Shorten DTR bar text

                bool shortDTRText = Service.Configuration.ShortDTRText;

                if (ImGui.Checkbox("縮短伺服器資訊列文字", ref shortDTRText))
                {
                    Service.Configuration.ShortDTRText = shortDTRText;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker(
                    "By default, the Server Info Bar for Wrath Combo shows whether Auto-Rotation is on or off, " +
                    "\nthen -if on- it will show how many active Auto-Mode combos you have enabled. " +
                    "\nAnd finally, it will also show if another plugin is controlling that value." +
                    "\nThis option will make the number of active Auto-Mode combos not show."
                );

                #endregion

                #region Message of the Day

                bool motd = Service.Configuration.HideMessageOfTheDay;

                if (ImGui.Checkbox("隱藏每日訊息", ref motd))
                {
                    Service.Configuration.HideMessageOfTheDay = motd;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker("登入時不在聊天視窗顯示每日訊息。");

                #endregion

                #region TargetHelper

                Vector4 colour = Service.Configuration.TargetHighlightColor;
                if (ImGui.ColorEdit4("目標醒目提示顏色", ref colour, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaPreview | ImGuiColorEditFlags.AlphaBar))
                {
                    Service.Configuration.TargetHighlightColor = colour;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker("特定功能選擇隊員為目標時，在遊戲原生隊伍列表上繪製外框。\n將透明度設為 0 可隱藏外框。");

                ImGui.SameLine();
                ImGui.TextColored(ImGuiColors.DalamudGrey, $"（目前僅供 {CustomComboFunctions.JobIDs.JobIDToName(33)} 使用）");

                #endregion

                ImGuiEx.Spacing(new Vector2(0, 10));

                #region Open to PvE

                if (ImGui.Checkbox("開啟 Wrath 時顯示 PvE 功能分頁", ref
                        Service.Configuration.OpenToPvE))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("When you open Wrath with `/wrath`, it will open to the PvE Features tab, instead of the last tab you were on." +
                                           "\nSame as always using the `/wrath pve` command to open Wrath.");

                #endregion

                #region Open to PvP

                if (ImGui.Checkbox("在 PvP 區域開啟 Wrath 時顯示 PvP 功能分頁", ref
                        Service.Configuration.OpenToPvP))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("Same as above, when you open Wrath with `/wrath`, it will open to the PvP Features tab, instead of the last tab you were on, when in a PvP area." +
                                           "\nSimilar to using the `/wrath pvp` command to open Wrath.");

                ImGui.SameLine();
                ImGui.TextColored(ImGuiColors.DalamudGrey, "（此設定會覆蓋上一項）");

                #endregion

                #region Open to Current Job

                if (ImGui.Checkbox("開啟 PvE 功能時自動顯示目前職業", ref Service.Configuration.OpenToCurrentJob))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("When you open Wrath's UI, if your last tab was PvE, it will automatically open to the job you are currently playing.");

                #endregion

                #region Open to Current Job on Switching

                if (ImGui.Checkbox("切換職業時自動顯示目前職業", ref Service.Configuration.OpenToCurrentJobOnSwitch))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("When you switch jobs, it will automatically switch the PvE Features tab to the job you are currently playing.");

                #endregion

                #endregion

                #region Rotation Behavior Options

                ImGuiEx.Spacing(new Vector2(0, 20));
                ImGuiEx.TextUnderlined("循環行為設定");

                #region Spells while Moving

                if (ImGui.Checkbox("移動時阻擋詠唱技能", ref Service.Configuration.BlockSpellOnMove))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("Completely blocks spells from being used if you are moving, by replacing your actions with Savage Blade.\nThis would supersede combo-specific movement options, available for most jobs.\n\nIt is recommended to keep this off, as most combos already handle this more gracefully.\nDefault: Off");

                #endregion

                #region Action Changing

                if (ImGui.Checkbox("技能替換", ref Service.Configuration.ActionChanging))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("Controls whether Actions will be Intercepted Replaced with combos from the plugin.\nIf disabled, your manual presses of abilities will no longer be affected by your Wrath settings.\n\nAuto-Rotation will work regardless of the setting.\n\nControlled by the `/wrath combo` command.\n\nIt is REQUIRED to keep this on if you use want to use Wrath without Auto Rotation.\nDefault: On");

                #endregion

                #region Performance Mode

                if (Service.Configuration.ActionChanging) {
                    ImGui.Indent();
                    if (ImGui.Checkbox("效能模式", ref Service.Configuration.PerformanceMode))
                        Service.Configuration.Save();

                    ImGuiComponents.HelpMarker("This mode will disable actions being changed on your hotbar, but will still continue to work in the background as you press your buttons.\n\nIt is recommended to try turing this on if you have performance issues.\nDefault: Off");
                    ImGui.Unindent();
                }

                #endregion

                #region Queued Action Suppression

                if (ImGui.Checkbox("抑制已排隊技能造成的判定", ref Service.Configuration.SuppressQueuedActions))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("While Enabled:\nWhenever you queue an action that is not the same as the button you are pressing, Wrath will disable every other Combo, preventing them from thinking the queued action should trigger them.\n- This prevents combos from conflicting with each other, just because of overlap in actions that combos return and actions that combos replace.\n- This does however cause the Replaced Action for each combo to 'flash' through as actions are queued.\nThat 'flashed' action won't go through, it is only visual.\n\nWhile Disabled:\nCombos will not be disabled when actions are queued from a combo.\n- This prevents your hotbars 'flashing', that is the only benefit.\n- This does however allow Combos to conflict with each other, if one combo returns an action that another combo has as its Replaced Action.\nWe do NOT mark these types of conflicts, and we do NOT try to avoid them as we add new features.\n\nIt is STRONGLY recommended to keep this setting On.\nIf the 'flashing' bothers you it is MUCH more advised to use Performance Mode,\ninstead of turning this off.\nDefault: On");

                ImGui.SameLine();
                ImGuiEx.Spacing(new Vector2(10, 0));
                ImGui.TextColored(ImGuiColors.DalamudGrey, "詳細說明：");

                ImGuiComponents.HelpMarker($"With this enabled, whenever you queue an action that is not the same as the button you are pressing, it will disable every other button's feature from running. This resolves a number of issues where incorrect actions are performed due to how the game processes queued actions, however the visual experience on your hotbars is degraded. This is not recommended to be disabled, however if you feel uncomfortable with hotbar icons changing quickly this is one way to resolve it (or use Performance Mode) but be aware that this may introduce unintended side effects to combos if you have a lot enabled for a job." +
                    $"\n\n" +
                    $"For a more complicated explanation, whenever an action is used, the following happens:" +
                    $"\n1. If the action invokes the GCD (Weaponskills & Spells), if the GCD currently isn't active it will use it right away." +
                    $"\n2. Otherwise, if you're within the \"Queue Window\" (normally the last 0.5s of the GCD), it gets added to the queue before it is used." +
                    $"\n3. If the action is an Ability, as long as there's no animation lock currently happening it will execute right away." +
                    $"\n4. Otherwise, it is added to the queue immediately and then used when the animation lock is finished." +
                    $"\n\nFor step 1, the action being passed to the game is the original, unmodified action, which is then converted at use time. At step 2, things get messy as the queued action still remains the unmodified action, but when the queue is executed it treats it as if the modified action *is* the unmodified action." +
                    $"\n\nE.g. Original action Cure, modified action Cure II. At step 1, the game is okay to convert Cure to Cure II because that is what we're telling it to do. However, when Cure is passed to the queue, it treats it as if the unmodified action is Cure II." +
                    $"\n\nThis is similar for steps 3 & 4, except it can just happen earlier." +
                    $"\n\nHow this impacts us is if using the example before, we have a feature replacing Cure with Cure II, and another replacing Cure II with Regen and you enable both, the following happens:" +
                    $"\n\nStep 1, Cure is passed to the game, is converted to Cure II.\nYou press Cure again at the Queue Window, Cure is passed to the queue, however the queue when it goes to execute will treat it as Cure II.\nResult is instead of Cure II being executed, it's Regen, because we've told it to modify Cure II to Regen." +
                    $"\nThis was not part of the first feature, therefore an incorrect action." +
                    $"\n\nOur workaround for this is to disable all other actions being replaced if they don't match the queued action, which this setting controls.");

                #endregion

                #region Throttle

                var len = ImGui.CalcTextSize("milliseconds").X;

                ImGui.PushItemWidth(75);
                var throttle = Service.Configuration.Throttle;
                if (ImGui.InputInt("###ActionThrottle",
                        ref throttle, 0, 0))
                {
                    Service.Configuration.Throttle = Math.Clamp(throttle, 0, 1500);
                    Service.Configuration.Save();
                }

                ImGui.SameLine();
                var pos = ImGui.GetCursorPosX() + len;
                ImGui.Text("毫秒");
                ImGui.SameLine(pos);
                ImGui.Text("   -   技能更新節流");


                ImGuiComponents.HelpMarker(
                    "This is the restriction for how often combos will update the action on your hotbar." +
                    "\nBy default this isn't restricting the combos, so you always get an up-to-date action." +
                    "\n\nIf you have minor FPS issues, you can increase this value to make combos run less often." +
                    "\nThis makes your combos less responsive, and perhaps even clips GCDs." +
                    "\nAt high values, this can break your rotation entirely." +
                    "\nMore severe FPS issues should instead be handled with Performance Mode option above." +
                    "\n\n200ms can make a reasonable difference in your FPS." +
                    "\nValues over 500ms are NOT recommended." +
                    "\nDefault: 50");

                #endregion

                #region Movement Check Delay

                ImGui.PushItemWidth(75);
                if (ImGui.InputFloat("###MovementLeeway", ref Service.Configuration.MovementLeeway))
                    Service.Configuration.Save();

                ImGui.SameLine();
                ImGui.Text("秒");

                ImGui.SameLine(pos);

                ImGui.Text("   -   移動判定延遲");

                ImGuiComponents.HelpMarker("Many features check if you are moving to decide actions, this will allow you to set a delay on how long you need to be moving before it recognizes you as moving.\nThis allows you to not have to worry about small movements affecting your rotation, primarily for casters.\n\nIt is recommended to keep this value between 0 and 1 seconds.\nDefault: 0.0");

                #endregion

                #region Opener Failure Timeout

                if (ImGui.InputFloat("###OpenerTimeout", ref Service.Configuration.OpenerTimeout))
                    Service.Configuration.Save();

                ImGui.SameLine();
                ImGui.Text("秒");

                ImGui.SameLine(pos);

                ImGui.Text("   -   起手式失敗逾時");

                ImGuiComponents.HelpMarker("During an opener if this amount of time has passed since your last action, it will fail the opener and resume with non-opener functionality.\n\nIt is recommended to keep this value below 6.\nDefault: 4.0");

                #endregion

                #region Melee Offset
                var offset = (float)Service.Configuration.MeleeOffset;

                if (ImGui.InputFloat("###MeleeOffset", ref offset))
                {
                    Service.Configuration.MeleeOffset = (double)offset;
                    Service.Configuration.Save();
                }

                ImGui.SameLine();
                ImGui.Text("雅姆");
                ImGui.SameLine(pos);

                ImGui.Text("   -   近戰距離偏移");

                ImGuiComponents.HelpMarker("Offset of melee check distance.\nFor those who don't want to immediately use their ranged attack if the boss walks slightly out of range.\n\nFor example, a value of -0.5 would make you have to be 0.5 yalms closer to the target,\nor a value of 2 would disable triggering of ranged features until you are 2 yalms further from the hitbox.\n\nIt is recommended to keep this value at 0.\nDefault: 0.0");
                #endregion

                #region Interrupt Delay

                var delay = (int)(Service.Configuration.InterruptDelay * 100d);

                if (ImGui.SliderInt("###InterruptDelay",
                    ref delay, 0, 100))
                {
                    delay = delay.RoundOff(SliderIncrements.Fives);

                    Service.Configuration.InterruptDelay = ((double)delay) / 100d;
                    Service.Configuration.Save();
                }
                ImGui.SameLine();
                ImGui.Text("詠唱進度 %");
                ImGui.SameLine( pos);
                ImGui.Text("   -   打斷延遲");

                ImGuiComponents.HelpMarker("The percentage of a total cast time to wait before interrupting.\nApplies to all interrupts, in every job's combos.\n\nIt is recommend to keep this value below 50%.\nDefault: 0%");

                #endregion
                
                #region Maximum Weaves

                ImGui.PushItemWidth(75);
                if (ImGui.SliderInt("###MaximumWeaves", ref Service.Configuration.MaximumWeavesPerWindow, 1, 3))
                    Service.Configuration.Save();

                ImGui.SameLine();
                ImGui.Text("能力技");

                ImGui.SameLine(pos);

                ImGui.Text("   -   最大插入能力技數");

                ImGuiComponents.HelpMarker("This controls how many oGCDs are allowed between GCDs.\nThe sort of 'default' for the game is double weaving, but triple weaving is completely possible to do with low enough latency (of every kind); but if you struggle with latency of some sort, single weaving may even be a good answer for you.\nTriple weaving is already done in a manner where we try to avoid clipping GCDs, and as such doesn't happen particularly often even if you do have good latency, so it is a safe option as far as parses/etc goes.\n\nDefault: 2");

                #endregion

                #endregion

                #region Targeting Options

                ImGuiEx.Spacing(new Vector2(0, 20));
                ImGuiEx.TextUnderlined("目標設定");

                var useCusHealStack = Service.Configuration.UseCustomHealStack;

                #region Retarget ST Healing Actions

                bool retargetHealingActions =
                    Service.Configuration.RetargetHealingActionsToStack;
                if (ImGui.Checkbox("重新選擇單體治療技能的目標", ref retargetHealingActions))
                {
                    Service.Configuration.RetargetHealingActionsToStack =
                        retargetHealingActions;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker(
                    "This will retarget all single target healing actions to the Heal Stack as shown below,\nsimilarly to how Redirect or Reaction would.\nThis ensures that the target used to check HP% threshold logic for healing actions is the same target that will receive that heal.\n\nIt is recommended to enable this if you customize the Heal Stack at all.\nDefault: Off");
                Presets.DrawRetargetedSymbolForSettingsPage();

                bool addNpcs = 
                    Service.Configuration.AddOutOfPartyNPCsToRetargeting;

                if (ImGui.Checkbox("重新選擇目標時包含隊伍外 NPC", ref addNpcs))
                {
                    Service.Configuration.AddOutOfPartyNPCsToRetargeting = addNpcs;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker(
                    "This will add any NPCs that are not in your party to the retargeting logic for healing actions.\n\n" +
                    "This is useful for healers who want to be able to target NPCs that are not in their party, such as quest NPCs.\n\n" +
                    "These NPCs will not work with any role based custom stacks (even if an NPC looks like a tank, they're not classed as one)\n\n" +
                    "Default: Off");

                #endregion

                ImGuiEx.Spacing(new Vector2(0, 10));

                #region Current Heal Stack

                ImGui.TextUnformatted("目前治療目標優先序：");

                ImGuiComponents.HelpMarker(
                    "This is the order in which Wrath will try to select a healing target.\n\n" +
                    "If the 'Retarget Healing Actions' option is disabled, that is just the target that will be used for checking the HP threshold to trigger different healing actions to show up in their rotations.\n" +
                    "If the 'Retarget Healing Actions' option is enabled, that target is also the one that healing actions will be targeted onto (even when the action does not first check the HP of that target, like the combo's Replaced Action, for example).");

                var healStackText = "";
                var nextStackItemMarker = "   >   ";
                if (useCusHealStack)
                {
                    foreach (var item in Service.Configuration.CustomHealStack
                                 .Select((value, index) => new { value, index }))
                    {
                        healStackText += UserConfig.TargetDisplayNameFromPropertyName(item.value);
                        if (item.index < Service.Configuration.CustomHealStack.Length - 1)
                            healStackText += nextStackItemMarker;
                    }
                }
                else
                {
                    if (Service.Configuration.UseUIMouseoverOverridesInDefaultHealStack)
                        healStackText += "UI-MouseOver Target" + nextStackItemMarker;
                    if (Service.Configuration.UseFieldMouseoverOverridesInDefaultHealStack)
                        healStackText += "Field-MouseOver Target" + nextStackItemMarker;
                    healStackText += "Soft Target" + nextStackItemMarker;
                    healStackText += "Hard Target" + nextStackItemMarker;
                    if (Service.Configuration.UseFocusTargetOverrideInDefaultHealStack)
                        healStackText += "Focus Target" + nextStackItemMarker;
                    if (Service.Configuration.UseLowestHPOverrideInDefaultHealStack)
                        healStackText += "Lowest HP% Ally" + nextStackItemMarker;
                    healStackText += "Self";
                }
                ImGuiEx.Spacing(new Vector2(10, 0));
                ImGuiEx.TextWrapped(ImGuiColors.DalamudGrey, healStackText);

                ImGuiEx.Spacing(new Vector2(0, 10));

                #endregion

                #region Heal Stack Customization Options

                var labelText = "Heal Stack Customization Options";
                // Nest the Collapse into a Child of varying size, to be able to limit its width
                var dynamicHeight = _unCollapsed
                    ? _healStackCustomizationHeight
                    : ImGui.CalcTextSize("I").Y + 5f.Scale();
                ImGui.BeginChild("##HealStackCustomization",
                    new Vector2(ImGui.CalcTextSize(labelText).X * 2.2f, dynamicHeight),
                    false,
                    ImGuiWindowFlags.NoScrollbar);

                // Collapsing Header for the Heal Stack Customization Options
                _unCollapsed = ImGui.CollapsingHeader(labelText,
                    ImGuiTreeNodeFlags.SpanAvailWidth);
                var collapsibleHeight = ImGui.GetItemRectSize().Y;
                if (_unCollapsed)
                {
                    ImGui.BeginGroup();

                    #region Default Heal Stack Include: UI MouseOver

                    if (useCusHealStack) ImGui.BeginDisabled();

                    bool useUIMouseoverOverridesInDefaultHealStack =
                        Service.Configuration.UseUIMouseoverOverridesInDefaultHealStack;
                    if (ImGui.Checkbox("將介面滑鼠指向目標加入預設治療優先序", ref useUIMouseoverOverridesInDefaultHealStack))
                    {
                        Service.Configuration.UseUIMouseoverOverridesInDefaultHealStack =
                            useUIMouseoverOverridesInDefaultHealStack;
                        Service.Configuration.Save();
                    }

                    if (useCusHealStack) ImGui.EndDisabled();

                    ImGuiComponents.HelpMarker("This will add any UI MouseOver targets to the top of the Default Heal Stack, overriding the rest of the stack if you are mousing over any party member UI.\n\nIt is recommended to enable this if you are a keyboard+mouse user and enable Retarget Healing Actions (or have UI MouseOver targets in your Redirect/Reaction configuration).\nDefault: Off");

                    #endregion

                    #region Default Heal Stack Include: Field MouseOver

                    if (useCusHealStack) ImGui.BeginDisabled();

                    bool useFieldMouseoverOverridesInDefaultHealStack =
                        Service.Configuration.UseFieldMouseoverOverridesInDefaultHealStack;
                    if (ImGui.Checkbox("將場景滑鼠指向目標加入預設治療優先序", ref useFieldMouseoverOverridesInDefaultHealStack))
                    {
                        Service.Configuration.UseFieldMouseoverOverridesInDefaultHealStack =
                            useFieldMouseoverOverridesInDefaultHealStack;
                        Service.Configuration.Save();
                    }

                    if (useCusHealStack) ImGui.EndDisabled();

                    ImGuiComponents.HelpMarker("This will add any MouseOver targets to the top of the Default Heal Stack, overriding the rest of the stack if you are mousing over any nameplate UI or character model.\n\nIt is recommended to enable this only if you regularly intentionally use field mouseover targeting already.\nDefault: Off");

                    #endregion

                    #region Default Heal Stack Include: Focus Target

                    if (useCusHealStack) ImGui.BeginDisabled();

                    bool useFocusTargetOverrideInDefaultHealStack =
                        Service.Configuration.UseFocusTargetOverrideInDefaultHealStack;
                    if (ImGui.Checkbox("將焦點目標加入預設治療優先序", ref useFocusTargetOverrideInDefaultHealStack))
                    {
                        Service.Configuration.UseFocusTargetOverrideInDefaultHealStack =
                            useFocusTargetOverrideInDefaultHealStack;
                        Service.Configuration.Save();
                    }

                    if (useCusHealStack) ImGui.EndDisabled();

                    ImGuiComponents.HelpMarker("This will add your focus target under your hard and soft targets in the Default Heal Stack, overriding the rest of the stack if you have a living focus target.\n\nDefault: Off");

                    #endregion

                    #region Default Heal Stack Include: Lowest HP Ally

                    if (useCusHealStack) ImGui.BeginDisabled();

                    bool useLowestHPOverrideInDefaultHealStack =
                        Service.Configuration.UseLowestHPOverrideInDefaultHealStack;
                    if (ImGui.Checkbox("將 HP% 最低的隊友加入預設治療優先序", ref useLowestHPOverrideInDefaultHealStack))
                    {
                        Service.Configuration.UseLowestHPOverrideInDefaultHealStack =
                            useLowestHPOverrideInDefaultHealStack;
                        Service.Configuration.Save();
                    }

                    if (useCusHealStack) ImGui.EndDisabled();

                    ImGuiComponents.HelpMarker("This will add a nearby party member with the lowest HP% to bottom of the Default Heal Stack, overriding only yourself.\n\nTHIS SHOULD BE USED WITH THE 'RETARGET HEALING ACTIONS' SETTING!\n\nDefault: Off");

                    if (useCusHealStack) ImGui.BeginDisabled();
                    if (useLowestHPOverrideInDefaultHealStack)
                    {
                        ImGuiEx.Spacing(new Vector2(30, 0));
                        ImGuiEx.Text(ImGuiColors.DalamudYellow, "This should be used with the 'Retarget Healing Actions' setting above!");
                    }
                    if (useCusHealStack) ImGui.EndDisabled();

                    #endregion

                    ImGuiEx.Spacing(new Vector2(5, 5));
                    ImGui.TextUnformatted("或");
                    ImGuiEx.Spacing(new Vector2(0, 5));

                    #region Use Custom Heal Stack

                    bool useCustomHealStack = Service.Configuration.UseCustomHealStack;
                    if (ImGui.Checkbox("改用自訂治療目標優先序", ref useCustomHealStack))
                    {
                        Service.Configuration.UseCustomHealStack = useCustomHealStack;
                        Service.Configuration.Save();
                    }

                    ImGuiComponents.HelpMarker("Select this if you would rather make your own stack of target priorities for Heal Targets instead of using our default stack.\n\nIt is recommended to use this to align with your Redirect/Reaction configuration if you're not using the Retarget Healing Actions setup; otherwise it is preference.\nDefault: Off");

                    #endregion

                    #region Custom Heal Stack Manager

                    if (Service.Configuration.UseCustomHealStack)
                    {
                        ImGui.Indent();
                        UserConfig.DrawCustomStackManager(
                            "CustomHealStack",
                            ref Service.Configuration.CustomHealStack,
                            ["Enemy", "Attack", "Dead", "Living"],
                            "The priority goes from top to bottom.\n" +
                            "Scroll down to see all of your items.\n" +
                            "Click the Up and Down buttons to move items in the list.\n" +
                            "Click the X button to remove an item from the list.\n\n" +
                            "If there are fewer than 4 items, and all return nothing when checked, will fall back to Self.\n\n" +
                            "These targets will only be considered valid if they are friendly and within 25y.\n" +
                            "These targets will be checked for being Dead or having a Cleansable Debuff\n" +
                            "when this Stack is applied to Raises or Esuna, respectively.\n" +
                            "(For Raises: the Stack will fall back to your Hard Target or any Dead Party Member)\n\n" +
                            "Default: Focus Target > Hard Target > Self"
                        );
                        ImGui.Unindent();
                    }

                    #endregion

                    ImGui.EndGroup();

                    // Get the max height of the section above
                    _healStackCustomizationHeight =
                        ImGui.GetItemRectSize().Y + collapsibleHeight + 5f.Scale();
                }

                ImGui.EndChild();

                if (_unCollapsed)
                    ImGuiEx.Spacing(new Vector2(0, 10));

                #endregion

                ImGuiEx.Spacing(new Vector2(0, 10));

                #region Raise Stack Manager

                ImGui.TextUnformatted("目前復活目標優先序：");

                ImGuiComponents.HelpMarker(
                    "This is the order in which Wrath will try to select a " +
                    "target to Raise,\nif Retargeting of any Raise Feature is enabled.\n\n" +
                    "You can find Raise Features under PvE>General,\n" +
                    "or under each caster that has a Raise.");

                ImGui.Indent();
                UserConfig.DrawCustomStackManager(
                    "CustomRaiseStack",
                    ref Service.Configuration.RaiseStack,
                    ["Enemy", "Attack", "MissingHP", "Lowest", "Chocobo", "Living"],
                    "The priority goes from top to bottom.\n" +
                    "Scroll down to see all of your items.\n" +
                    "Click the Up and Down buttons to move items in the list.\n" +
                    "Click the X button to remove an item from the list.\n\n" +
                    "If there are fewer than 5 items, and all return nothing when checked, will fall back to:\n" +
                    "your Hard Target if they're dead, or <Any Dead Party Member>.\n\n"+
                    "These targets will only be considered valid if they are friendly, dead, and within 30y.\n" +
                    "Default: Any Healer > Any Tank > Any Raiser > Any Dead Party Member",
                    true
                );
                ImGui.TextDisabled("（所有目標皆會檢查是否可復活）");
                ImGui.Unindent();

                #endregion

                #endregion

                #region Troubleshooting Options

                ImGuiEx.Spacing(new Vector2(0, 20));
                ImGuiEx.TextUnderlined("疑難排解／分析設定");

                #region Combat Log

                bool showCombatLog = Service.Configuration.EnabledOutputLog;

                if (ImGui.Checkbox("將紀錄輸出至聊天視窗", ref showCombatLog))
                {
                    Service.Configuration.EnabledOutputLog = showCombatLog;
                    Service.Configuration.Save();
                }

                ImGuiComponents.HelpMarker("Every time you use an action, the plugin will print it to the chat.");
                #endregion

                #region Opener Log

                if (ImGui.Checkbox("將起手式狀態輸出至聊天視窗", ref Service.Configuration.OutputOpenerLogs))
                    Service.Configuration.Save();

                ImGuiComponents.HelpMarker("Every time your class's opener is ready, fails, or finishes as expected, it will print to the chat.");
                #endregion

                #region Debug File

                if (ImGui.Button("建立偵錯檔案"))
                {
                    if (Player.Available)
                        DebugFile.MakeDebugFile();
                    else
                        DebugFile.MakeDebugFile(allJobs: true);
                }

                ImGuiComponents.HelpMarker("Will generate a debug file on your desktop.\nUseful to give developers to help troubleshoot issues.\nThe same as using the following command: /wrath debug");

                #endregion

                #endregion
            }
        }

        #region Custom Heal Stack Manager Methods

        private static bool _unCollapsed;
        private static float _healStackCustomizationHeight = 0;

        #endregion
    }
}
