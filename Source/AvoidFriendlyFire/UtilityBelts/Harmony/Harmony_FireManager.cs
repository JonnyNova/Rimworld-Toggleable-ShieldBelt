using AvoidFriendlyFire;
using FrontierDevelopment.UtilityBelts;
using HarmonyLib;
using Verse;

namespace FrontierDevelopments.UtilityBelts.AvoidFriendlyFire.Harmony
{
    [HarmonyPatch(typeof(FireManager), "IsPawnWearingUsefulShield")]
    public static class Harmony_FireManager
    {
        [HarmonyPostfix]
        public static bool IsPawnWearingUsefulToggledShield(bool result, Pawn pawn)
        {
            if (result)
            {
                return ShieldUtility.HasActiveShield(pawn);
            }

            return result;
        }
    }
}