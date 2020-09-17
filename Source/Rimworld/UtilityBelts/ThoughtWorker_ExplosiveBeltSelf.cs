using System.Linq;
using RimWorld;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class ThoughtWorker_ExplosiveBeltSelf : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            return pawn.apparel.WornApparel
                .OfType<ExplosiveBelt>()
                .Any();
        }
    }
}