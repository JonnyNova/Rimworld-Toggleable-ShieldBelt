using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using RimWorld;
using Verse;

namespace FrontierDevelopments.ShieldBelt.Harmony
{
    public class Harmony_ShieldBelt
    {
        static bool IsEnabled(ThingWithComps shieldBelt)
        {
            var comp = shieldBelt.GetComp<CompShieldToggle>();
            return comp == null || comp.enabled;
        }

        private static IEnumerable<CodeInstruction> CheckFirst(IEnumerable<CodeInstruction> instructions, ILGenerator il, bool shieldActive, bool block)
        {
            var patchPhase = 0;
                
            foreach (var instruction in instructions)
            {
                // immediately check the comp if it should block damage / not display
                if (patchPhase == 0)
                {
                    var continueLabel = il.DefineLabel();
                    instruction.labels = new List<Label>(new []{ continueLabel });
                        
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Harmony_ShieldBelt), nameof(IsEnabled), new Type[]{ typeof(ThingWithComps) }));
                    yield return new CodeInstruction(block ? OpCodes.Brtrue : OpCodes.Brfalse, continueLabel);
                    yield return new CodeInstruction(shieldActive ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                    yield return new CodeInstruction(OpCodes.Ret);

                    patchPhase = -1;
                }
                    
                yield return instruction;
            }
        }

        [HarmonyPatch(typeof(RimWorld.ShieldBelt), nameof(RimWorld.ShieldBelt.AllowVerbCast))]
        public class Harmony_ShieldBelt_AllowVerbCast
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                return CheckFirst(instructions, il, true, true);
            }
        }
        
        [HarmonyPatch(typeof(RimWorld.ShieldBelt), nameof(RimWorld.ShieldBelt.CheckPreAbsorbDamage))]
        public class Harmony_ShieldBelt_CheckPreAbsorbDamage
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                return CheckFirst(instructions, il, false, true);
            }
        }

        [HarmonyPatch(typeof(RimWorld.ShieldBelt), "ShouldDisplay", MethodType.Getter)]
        public class Harmony_ShieldBelt_ShouldDisplay 
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                return CheckFirst(instructions, il, false, true);
            }
        }

        [HarmonyPatch(typeof(RimWorld.ShieldBelt), nameof(RimWorld.ShieldBelt.GetWornGizmos))]
        public class Harmony_ShieldBelt_GetWornGizmos
        {
            public static bool Prefix(RimWorld.ShieldBelt __instance, out IEnumerable<Gizmo> __result)
            {
                var result = new List<Gizmo>();
                
                // TODO use a friendlier patch
                if (Find.Selector.SingleSelectedThing == __instance.Wearer)
                {
                    result.Add(new Gizmo_EnergyShieldStatus
                    {
                        shield = __instance
                    });
                }
                
                foreach (var gizmo in __instance.GetComp<CompShieldToggle>().CompGetGizmosExtra())
                {
                    result.Add(gizmo);
                }

                __result = result.AsEnumerable();
                return false;
            }
        }
    }
}
