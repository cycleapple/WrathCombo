using Dalamud.Interface.Colors;
using Dalamud.Bindings.ImGui;
using WrathCombo.CustomComboNS.Functions;

namespace WrathCombo.Window.MessagesNS
{
    internal static class Messages
    {
        internal static bool PrintBLUMessage(string jobName)
        {
            if (jobName == CustomComboFunctions.JobIDs.JobIDToName(36)) //Blue Mage ID
            {
                ImGui.TextColored(ImGuiColors.ParsedPink, "即使沒有啟用全部必要技能，仍可使用這些功能。\n未啟用的技能會被略過；若功能未如預期運作，\n請嘗試啟用更多必要技能。");
            }

            return true;
        }
    }
}
