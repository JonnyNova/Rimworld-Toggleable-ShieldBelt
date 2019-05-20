using Harmony;
using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    [HarmonyPatch(typeof(WorkGiver_HunterHunt), nameof(WorkGiver_HunterHunt.HasShieldAndRangedWeapon))]
    static class Harmony_WorkGiver_HunterHunt
    {
        [HarmonyPostfix]
        static bool AllowHuntingWithToggledOffShield(bool __result, Pawn p)
        {
            if(__result) return ShieldUtility.HasActiveShield(p);
            return __result;
        }
    }
}