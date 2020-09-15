using Verse;

namespace FrontierDevelopments.UtilityBelts.AvoidFriendlyFire
{
    public class Module : Verse.Mod
    {
        public Module(ModContentPack content) : base(content)
        {
            var harmony = new HarmonyLib.Harmony("FrontierDevelopments.UtilityBelts.AvoidFriendlyFire");
            harmony.PatchAll();
        }
    }
}