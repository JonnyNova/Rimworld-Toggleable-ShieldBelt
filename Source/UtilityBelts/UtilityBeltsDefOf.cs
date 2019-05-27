using RimWorld;

namespace FrontierDevelopment
{
    [DefOf]
    public static class UtilityBeltsDefOf
    {
        public static StatDef FireFoamBeltRadius;
        
        static UtilityBeltsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof (ThingDefOf));
        }
    }
}