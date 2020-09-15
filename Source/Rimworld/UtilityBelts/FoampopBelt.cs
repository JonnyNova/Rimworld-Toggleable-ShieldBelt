using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public class FoampopBelt : Apparel
    {
        // same as smokepop belt
        private const float ApparelScorePerBeltRadius = 23f / 500f;

        private float Radius => this.GetStatValue(UtilityBeltsDefOf.FoampopPackRadius);

        private CompReloadable Reloadable => this.TryGetComp<CompReloadable>();

        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def == DamageDefOf.Flame)
            {
                Trigger(dinfo.Instigator);
            }
            return base.CheckPreAbsorbDamage(dinfo);
        }

        public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(ref dinfo, out absorbed);
            if (dinfo.Def == DamageDefOf.Flame)
            {
                Trigger(dinfo.Instigator);
            }
        }

        public override float GetSpecialApparelScoreOffset()
        {
            return Radius * ApparelScorePerBeltRadius;
        }

        public void Trigger(Thing instigator)
        {
            var reloadable = Reloadable;
            if (!reloadable.CanBeUsed) return;
            GenExplosion.DoExplosion(
                Wearer?.Position ?? Position,
                Wearer?.Map ?? Map,
                Radius,
                DamageDefOf.Extinguish,
                instigator,
                postExplosionSpawnChance: 1f,
                postExplosionSpawnThingDef: ThingDefOf.Filth_FireFoam,
                applyDamageToExplosionCellsNeighbors: true,
                postExplosionSpawnThingCount: 3);
            reloadable.UsedOnce();
        }
    }
}