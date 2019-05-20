using Verse;

namespace FrontierDevelopment.UtilityBelts.AvoidFriendlyFire
{
    // Harmony patch for AvoidFriendlyFire.FireManager
    public static class Harmony_FireManager
    {
        // Prefix for IsPawnWearingUsefulShield
        public static bool IsPawnWearingUsefulToggledShield(out bool __result, Pawn pawn)
        {
            __result = false;
            // run normal method is shield is enabled;
            return ShieldUtility.HasActiveShield(pawn);
        }
    }
}