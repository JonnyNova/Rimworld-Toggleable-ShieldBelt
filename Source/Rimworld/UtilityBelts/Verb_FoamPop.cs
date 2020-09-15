using FrontierDevelopment;
using FrontierDevelopment.UtilityBelts;
using RimWorld;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class Verb_FoamPop : Verb
    {
        protected override bool TryCastShot()
        {
            var equipment = EquipmentSource;
            (equipment as FoampopBelt)?.Trigger(equipment);
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