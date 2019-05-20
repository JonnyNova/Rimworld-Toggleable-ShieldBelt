using Verse;

namespace FrontierDevelopments.UtilityBelts.AvoidFriendlyFire
{
    // Harmony patch for AvoidFriendlyFire.FireManager
    public static class Harmony_FireManager
    {
        // Prefix for IsPawnWearingUsefulShield
        public static bool IsPawnWearingUsefulToggledShield(out bool __result, Pawn pawn)
        {
            __result = false;
            // run normal method is shield is enabled;
            return CompShieldToggle.IsEnabled(pawn);
        }
    }
}