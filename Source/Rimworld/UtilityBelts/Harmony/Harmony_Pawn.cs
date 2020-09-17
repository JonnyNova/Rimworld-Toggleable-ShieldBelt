using System.Linq;
using HarmonyLib;
using Verse;

namespace FrontierDevelopments.UtilityBelts.Harmony
{
    public class Harmony_Pawn
    {
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill))]
        static class PawnDeathListener
        {
            [HarmonyPrefix]
            static void ExplodeBeltOnDeath(Pawn __instance)
            {
                __instance.apparel.WornApparel
                    .OfType<ExplosiveBelt>()
                    .ToList()
                    .Do(belt => belt.OnWearerDeath());
            }
        }
    }
}