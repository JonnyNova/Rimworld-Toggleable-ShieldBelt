
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace FrontierDevelopments.ShieldBelt
{
    public class CompShieldToggle : ThingComp
    {
        public bool enabled;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            
            if (ShouldShowGizmo())
            {
                yield return new Command_Toggle
                {
                    icon = Resources.ToggleShield,
                    defaultDesc = "FrontierDevelopments.ShieldBelt.Toggle.Desc".Translate(),
                    defaultLabel = "FrontierDevelopments.ShieldBelt.Toggle.Label".Translate(),
                    isActive = () => enabled,
                    toggleAction = () => enabled = !enabled
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref enabled, "enabled", true);
        }

        private bool ShouldShowGizmo()
        {
            var apparel = parent as Apparel;
            if (apparel != null)
            {
                var wearer = apparel.Wearer;
                if (wearer != null)
                {
                    return wearer.Faction == Faction.OfPlayer;
                }
                return false;

            }
            return false;
        }
    }
}