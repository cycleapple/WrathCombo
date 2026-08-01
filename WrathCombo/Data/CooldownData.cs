using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using ECommons.DalamudServices;
using System.Runtime.CompilerServices;

namespace WrathCombo.Data
{
    internal class CooldownData
    {
        /// <summary> Gets a value indicating whether the action is on cooldown. </summary>
        public bool IsCooldown
        {
            get
            {
                return CooldownRemaining > 0;
            }
        }

        /// <summary> Gets the action ID on cooldown. </summary>
        public uint ActionID;

        /// <summary>
        /// Gets the recast group data without calling ActionManager recast member functions.
        /// The API13 Taiwan client does not resolve the international GetRecastTime signatures.
        /// </summary>
        private unsafe RecastDetail* Recast
        {
            get
            {
                var manager = ActionManager.Instance();
                var action = Svc.Data.GetExcelSheet<Lumina.Excel.Sheets.Action>().GetRowOrDefault(ActionID);
                if (manager == null || action == null)
                    return null;

                // Excel cooldown groups are one-based; ActionManager.Cooldowns is zero-based.
                var group = action.Value.CooldownGroup - 1;
                if (group < 0 || group >= manager->Cooldowns.Length)
                    return null;

                return (RecastDetail*)Unsafe.AsPointer(ref manager->Cooldowns[group]);
            }
        }

        /// <summary> Gets the elapsed cooldown time. </summary>
        public unsafe float CooldownElapsed
        {
            get
            {
                var recast = Recast;
                return recast != null && recast->IsActive ? recast->Elapsed : 0;
            }
        }

        /// <summary> Gets the total cooldown time. </summary>
        public float CooldownTotal => BaseCooldownTotal * MaxCharges;

        /// <summary> Includes Skill/Spell Speed modifiers along with any job trait modifiers </summary>
        public unsafe float BaseCooldownTotal
        {
            get
            {
                var recast = Recast;
                if (recast != null && recast->Total > 0)
                    return recast->Total / Math.Max(1, (int)MaxCharges);

                // Safe fallback before the recast group has been initialized. This is unadjusted,
                // but prevents a failed signature from taking down the framework update loop.
                return Svc.Data.GetExcelSheet<Lumina.Excel.Sheets.Action>()
                    .GetRowOrDefault(ActionID)?.Recast100ms / 10f ?? 0;
            }
        }

        /// <summary> Gets the cooldown time remaining. </summary>
        public unsafe float CooldownRemaining => CooldownElapsed == 0 ? 0 : Math.Max(0, CooldownTotal - CooldownElapsed);

        /// <summary> Gets the maximum number of charges for an action at the current level. </summary>
        /// <returns> Number of charges. </returns>
        public ushort MaxCharges => ActionManager.GetMaxCharges(ActionID, 0);

        /// <summary> Gets a value indicating whether the action has charges, not charges available. </summary>
        public bool HasCharges => MaxCharges > 1;

        /// <summary> Gets the remaining number of charges for an action. </summary>
        public unsafe uint RemainingCharges
        {
            get
            {
                if (MaxCharges == 1)
                    return CooldownRemaining == 0 ? 1 : 0u;

                return ActionManager.Instance()->GetCurrentCharges(ActionID);
            }
        }

        /// <summary> Gets the cooldown time remaining until the next charge. </summary>
        public float ChargeCooldownRemaining
        {
            get
            {
                return CooldownRemaining % (CooldownTotal / MaxCharges);
            }
        }
    }
}
