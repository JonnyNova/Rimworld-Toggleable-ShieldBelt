using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public class FireFoamBelt : Apparel
    {
        // same as smokepop belt
        private const float ApparelScorePerBeltRadius = 23f / 500f;

        private float Radius => this.GetStatValue(UtilityBeltsDefOf.FireFoamBeltRadius);

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

        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            foreach (var gizmo in base.GetWornGizmos())
            {
                yield return gizmo;
            }

            if (Wearer?.Faction == Faction.OfPlayer)
            {
                yield return new Command_Action
                {
                    icon = (Texture2D) Graphic.MatSouth.mainTexture ?? Resources.TriggerSmokepop,
                    defaultLabel = "FrontierDevelopment.SmokepopBelt.Toggle.Label".Translate(),
                    defaultDesc = "FrontierDevelopment.SmokepopBelt.Toggle.Desc".Translate().Replace("{0}", LabelShort),
                    activateSound = SoundDef.Named("Click"),
                    action = () => Trigger(Wearer)
                };
            }
        }

        public override float GetSpecialApparelScoreOffset()
        {
            return Radius * ApparelScorePerBeltRadius;
        }

        private void Trigger(Thing instigator)
        {
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
            
            Destroy();
        }
    }
}