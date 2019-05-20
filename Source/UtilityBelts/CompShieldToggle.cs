using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace FrontierDevelopments.UtilityBelts
{
    public class CompPropertiesShieldToggle : CompProperties
    {
        public CompPropertiesShieldToggle()
        {
            compClass = typeof(CompShieldToggle);
        }
    }

    public class CompShieldToggle : ThingComp
    {
        public static bool IsEnabled(ThingWithComps shieldBelt)
        {
            var comp = shieldBelt.GetComp<CompShieldToggle>();
            return comp == null || comp.enabled;
        }

        public static bool IsEnabled(Pawn pawn)
        {
            return pawn.apparel.WornApparel
                .SelectMany(apparel => apparel.AllComps)
                .OfType<CompShieldToggle>()
                .Any(toggle => toggle.enabled);
        }

        public bool enabled = true;

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