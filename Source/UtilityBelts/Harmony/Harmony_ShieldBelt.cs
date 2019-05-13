using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Harmony;
using Verse;

namespace FrontierDevelopments.UtilityBelts.Harmony
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
            public static bool Postfix(
                bool __result, 
                RimWorld.ShieldBelt __instance, 
                IntVec3 root, 
                Map map,
                LocalTargetInfo targ, 
                Verb verb)
            {
                return !IsEnabled(__instance) || __result;
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
            public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, RimWorld.ShieldBelt __instance)
            {
                foreach (var gizmo in __result)
                {
                    yield return gizmo;
                }

                var comp = __instance.GetComp<CompShieldToggle>();
                if (comp != null)
                {
                    foreach (var gizmo in comp.CompGetGizmosExtra())
                    {
                        yield return gizmo;
                    }
                }
            }
        }
    }
}
