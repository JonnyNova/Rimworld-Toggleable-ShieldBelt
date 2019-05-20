using System.Collections.Generic;
using Harmony;
using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    public class Harmony_ShieldBelt
    {
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.AllowVerbCast))]
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
                return !CompShieldToggle.IsEnabled(__instance) || __result;
            }
        }

        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.CheckPreAbsorbDamage))]
        public class Harmony_ShieldBelt_CheckPreAbsorbDamage
        {
            [HarmonyPrefix]
            [HarmonyPriority(Priority.First + 200)]
            public static bool ShouldAbsorbDamageWithToggle(out bool __result, ShieldBelt __instance)
            {
                __result = CompShieldToggle.IsEnabled(__instance);
                return __result;
            }
        }

        [HarmonyPatch(typeof(ShieldBelt), "ShouldDisplay", MethodType.Getter)]
        public class Harmony_ShieldBelt_ShouldDisplay 
        {
            [HarmonyPostfix]
            public static bool ShouldDisplayWithToggle(bool __result, ShieldBelt __instance)
            {
                if (!CompShieldToggle.IsEnabled(__instance)) return false;
                return __result;
            }
        }

        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.GetWornGizmos))]
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
