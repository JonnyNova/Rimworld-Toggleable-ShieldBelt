using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public static class ShieldUtility
    {
        public static bool HasActiveShield(Pawn pawn)
        {
            foreach (var apparel in pawn.apparel.WornApparel)
            {
                var toggle = apparel.TryGetComp<CompShieldToggle>();
                switch (apparel)
                {
                    case ShieldBelt _:
                        if (toggle == null) return true;
                        break;
                }
                if (toggle != null && toggle.Enabled) return true;
            }
            return false;
        }

        public static List<Pawn> FilterOutToggledOffShieldUsers(List<Pawn> pawns)
        {
            return pawns.Where(HasActiveShield).ToList();
        }
    }
}