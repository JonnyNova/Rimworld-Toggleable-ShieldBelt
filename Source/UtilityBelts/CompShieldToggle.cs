using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts
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
            return comp == null || comp._enabled;
        }

        public static bool IsEnabled(Pawn pawn)
        {
            return pawn.apparel.WornApparel
                .SelectMany(apparel => apparel.AllComps)
                .OfType<CompShieldToggle>()
                .Any(toggle => toggle._enabled);
        }

        private bool _enabled = true;

        public bool Enabled => _enabled;

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
                    defaultDesc = "FrontierDevelopment.ShieldBelt.Toggle.Desc".Translate(),
                    defaultLabel = "FrontierDevelopment.ShieldBelt.Toggle.Label".Translate(),
                    isActive = () => _enabled,
                    toggleAction = () => _enabled = !_enabled
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _enabled, "enabled", true);
        }

        private bool ShouldShowGizmo()
        {
            switch (parent)
            {
                case Apparel apparel:
                    return apparel.Wearer?.Faction == Faction.OfPlayer;
            }
            return false;
        }
    }
}