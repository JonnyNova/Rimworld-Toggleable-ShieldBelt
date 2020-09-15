using RimWorld;

namespace FrontierDevelopment
{
    [DefOf]
    public static class UtilityBeltsDefOf
    {
        public static StatDef FoampopPackRadius;
        
        static UtilityBeltsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof (ThingDefOf));
        }
    }
}