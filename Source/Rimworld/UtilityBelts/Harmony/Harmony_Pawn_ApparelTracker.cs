using System.Linq;
using HarmonyLib;
using RimWorld;

namespace FrontierDevelopments.UtilityBelts.Harmony
{
    public class Harmony_Pawn
    {
        [HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Notify_PawnKilled))]
        static class PawnDeathListener
        {
            [HarmonyPrefix]
            static void ExplodeBeltOnDeath(Pawn_ApparelTracker __instance)
            {
                __instance.WornApparel
                    .OfType<ExplosiveBelt>()
                    .ToList()
                    .Do(belt => belt.OnWearerDeath());
            }
        }
    }
}