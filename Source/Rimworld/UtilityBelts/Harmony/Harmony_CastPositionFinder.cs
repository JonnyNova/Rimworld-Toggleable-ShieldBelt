using HarmonyLib;
using Verse;
using Verse.AI;

namespace FrontierDevelopment.UtilityBelts.Harmony
{
    [HarmonyPatch(typeof(CastPositionFinder), nameof(CastPositionFinder.TryFindCastPosition))]
    static class Harmony_CastPositionFinder
    {
        // prefix instead of postfix for performance reasons
        [HarmonyPrefix]
        static bool FailIfToggleOnWithRangedVerb(ref bool __result, CastPositionRequest newReq, ref IntVec3 dest)
        {
            if (ShieldUtility.HasActiveShield(newReq.caster)
                && (newReq.maxRangeFromTarget > 1.41f || newReq.maxRangeFromLocus > 1.41f))
            {
                dest = IntVec3.Invalid;
                __result = false;
                return false;
            }

            return true;
        }
    }
}