using RimWorld;

namespace FrontierDevelopments.UtilityBelts
{
    [DefOf]
    public static class UtilityBeltsDefOf
    {
        public static StatDef FoampopPackRadius;
        public static StatDef ExplosiveBeltRadius;
        
        static UtilityBeltsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof (ThingDefOf));
        }
    }
}