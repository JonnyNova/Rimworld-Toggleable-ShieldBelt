using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using RimWorld;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    public class Harmony_ShieldAlerts
    {
        private static IEnumerable<CodeInstruction> ApplyPatch(IEnumerable<CodeInstruction> instructions, MethodInfo method)
        {
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Call && instruction.operand == method)
                {
                    yield return new CodeInstruction(
                        OpCodes.Call, 
                        AccessTools.Method(
                            typeof(ShieldUtility), 
                            nameof(ShieldUtility.FilterOutToggledOffShieldUsers)));
                }
            }
        }

        [HarmonyPatch(typeof(Alert_HunterHasShieldAndRangedWeapon), nameof(Alert_HunterHasShieldAndRangedWeapon.GetReport))]
        static class Hunters
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> ExcludeToggledOffShieldUsers(IEnumerable<CodeInstruction> instructions)
            {
                return ApplyPatch(
                    instructions, 
                    AccessTools.Property(
                        typeof(Alert_HunterHasShieldAndRangedWeapon), 
                        "BadHunters"
                        ).GetGetMethod(true));
            }
        }

        [HarmonyPatch(typeof(Alert_ShieldUserHasRangedWeapon), nameof(Alert_ShieldUserHasRangedWeapon.GetReport))]
        static class RangedUsers
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> lala(IEnumerable<CodeInstruction> instructions)
            {
                return ApplyPatch(
                    instructions, 
                    AccessTools.Property(
                        typeof(Alert_ShieldUserHasRangedWeapon), 
                        "ShieldUsersWithRangedWeapon"
                        ).GetGetMethod(true));
            }
        }
        
    }
}