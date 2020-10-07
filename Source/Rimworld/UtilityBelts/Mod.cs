using System.Linq;
using HugsLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopment.UtilityBelts
{
    public class Mod : ModBase
    {
        public override string ModIdentifier => "FrontierDevelopments-UtilityBelts";

        public const string ModName = "Frontier Developments Utility Belts";

        public override void DefsLoaded()
        {
            AddToggleComps();
            FrontierDevelopments.UtilityBelts.ModSettings.Init(Settings);
        }
        
        private static void AddToggleComps()
        {
            var typeShieldBelt = typeof(ShieldBelt);
            var names = DefDatabase<ThingDef>.AllDefs
                .Where(def => def?.thingClass == typeShieldBelt)
                .Select( def =>
                {
                    def.comps.Add(new CompPropertiesShieldToggle());
                    return def.defName;
                });
            Log.Message(ModName + " :: Loaded. Adding shield toggle to: " 
                                + names.Aggregate("", (current, next) => current + " " + next));
        }
    }
    
    [StaticConstructorOnStartup]
    public static class Resources
    {
        public static readonly Texture2D ToggleShield = ContentFinder<Texture2D>.Get("Things/Pawn/Humanlike/Apparel/ShieldBelt/ShieldBelt");
        public static readonly Texture2D TriggerSmokepop = ContentFinder<Texture2D>.Get("Things/Pawn/Humanlike/Apparel/SmokepopBelt/SmokepopBelt");
    } 
}