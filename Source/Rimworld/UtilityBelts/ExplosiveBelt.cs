using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class ExplosiveBelt : Apparel
    {
        private Boolean _deadmanSwitch = false;
        
        private float Radius => this.GetStatValue(UtilityBeltsDefOf.ExplosiveBeltRadius);
        
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def == DamageDefOf.Bullet)
            {
                Trigger(dinfo.Instigator);
            }
            return base.CheckPreAbsorbDamage(dinfo);
        }

        public void Trigger(Thing instigator)
        {
            var wearer = Wearer;
            GenExplosion.DoExplosion(
                wearer?.Position ?? Position,
                wearer?.Map ?? Map,
                Radius,
                DamageDefOf.Bomb,
                instigator);
            // some extra damage just for the wearer
            GenExplosion.DoExplosion(
                wearer?.Position ?? Position,
                wearer?.Map ?? Map,
                0,
                DamageDefOf.Bomb,
                instigator,
                100);
        }

        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            _deadmanSwitch = false;
        }

        public void OnWearerDeath()
        {
            if(_deadmanSwitch) 
                Trigger(Wearer);
        }

        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            foreach (var gizmo in base.GetWornGizmos())
            {
                yield return gizmo;
            }

            if (Wearer.Faction == Faction.OfPlayer)
            {
                yield return new Command_Toggle
                {
                    icon = (Texture2D)Graphic.MatSouth.mainTexture,
                    defaultLabel = "FrontierDevelopment.ExplosiveBelt.Switch.Label".Translate(),
                    defaultDesc = "FrontierDevelopment.ExplosiveBelt.Switch.Desc".Translate(),
                    isActive = () => _deadmanSwitch,
                    toggleAction = () => _deadmanSwitch = !_deadmanSwitch
                };
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _deadmanSwitch, "deadmanSwitch", false);
        }
    }
}