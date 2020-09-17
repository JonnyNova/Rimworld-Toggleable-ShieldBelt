using RimWorld;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class Verb_ExplosiveBelt : Verb
    {
        protected override bool TryCastShot()
        {
            var equipment = EquipmentSource;
            (equipment as ExplosiveBelt)?.Trigger(equipment);
            return true;
        }
        
        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = false;
            return EquipmentSource.GetStatValue(UtilityBeltsDefOf.FoampopPackRadius);
        }

        public override void DrawHighlight(LocalTargetInfo target) => 
            DrawHighlightFieldRadiusAroundTarget(caster);
    }
}