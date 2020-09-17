using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class ThoughtWorker_ExplosiveBeltOther : ThoughtWorker
    {
        private bool HasBeltEquipped(Pawn pawn)
        {
            return pawn.apparel?.WornApparel.OfType<ExplosiveBelt>().Any() ?? false;
        }

        private bool FactionPawnsOnMap(Pawn pawn)
        {
            return pawn.Spawned
                   && pawn.Map.mapPawns
                       .PawnsInFaction(pawn.Faction)
                       .Where(p => p != pawn)
                       .Any(HasBeltEquipped);
        }

        private bool PawnsInCaravan(Pawn pawn)
        {
            return pawn.GetCaravan()?.pawns.InnerListForReading
                .Where(p => p != pawn)
                .Any(HasBeltEquipped) ?? false;
        }

        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            return PawnsInCaravan(pawn) || FactionPawnsOnMap(pawn);
        }
    }
}