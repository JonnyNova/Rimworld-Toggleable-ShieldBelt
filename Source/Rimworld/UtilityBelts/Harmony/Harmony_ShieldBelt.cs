using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    public class Harmony_ShieldBelt
    {
        private static bool IsOnline(ShieldBelt belt)
        {
            return CompShieldToggle.IsEnabled(belt) && belt.ShieldState == ShieldState.Active;
        }
        
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.AllowVerbCast))]
        public class Harmony_ShieldBelt_AllowVerbCast
        {
            [HarmonyPostfix]
            public static bool EnableShootingOutOnDisabledShields(
                bool __result, 
                RimWorld.ShieldBelt __instance, 
                IntVec3 root, 
                Map map,
                LocalTargetInfo targ, 
                Verb verb)
            {
                return !IsOnline(__instance) || __result;
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
    }
}
