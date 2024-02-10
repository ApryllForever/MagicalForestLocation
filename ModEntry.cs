using HarmonyLib;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley;

namespace MagicalForestLocation
{




    // Public, just to be on the safe side. Internal classes drive me insane.
    public class ModEntry : StardewModdingAPI.Mod                             // Behold!!! This is where SMAPI looks to find out what's up. The : StardewModdingAPI.Mod means it is inheriting SMAPI
    {

        public static Mod instance;     //This does stuff. Like, it lets the other parts of the program grab an instance of this, maybe. I am a little vague on that part. But it is super needed for like some things, like getting stuff for textures, I think. You might not need it.

        internal static IMonitor ModMonitor { get; set; }  //The Monitor. The Monitor and I are desperate enemies. Rare to never have I really had the monitor work. But others swear by it. Probably do better than me, and use the monitor! :) 

        internal static IModHelper ModHelper { get; set; } // Helper ummm... Does stuff. Like, a lot of the SMAPI stuff, I don't recall all what, but like, this thingie gets called to do stuff like load textures or what-not.




        public override void Entry(IModHelper helper)
        {

            instance = this; //Used to call an instance of this Mod class

            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;

            //I left the stuff for Harmony in here, should you ever so desire it.

            //var harmony = new Harmony(ModManifest.UniqueID);       
            //harmony.PatchAll();
            //harmony.Patch(AccessTools.Method("StardewModdingAPI.Framework.SGame:DrawImpl"), transpiler: new HarmonyMethod(typeof(Patches.Game1CatchLightingRenderPatch).GetMethod("Transpiler")));
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            //This serialized the stuff. If they aren't serialized, SMAPI becomes very angry when trying to serialize, and the Red Wall BLEEDS.

            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            sc.RegisterSerializerType(typeof(MagicForestLocation));
            sc.RegisterSerializerType(typeof(MagicForest));

        }

        private void OnLoadStageChanged(object sender, LoadStageChangedEventArgs e)
        {
            //This like adds the new location.

            if (e.NewStage == LoadStage.CreatedInitialLocations || e.NewStage == LoadStage.SaveAddedLocations)
            {
                Game1.locations.Add(new MagicForest(Helper.ModContent));
            }
        }
    }
}
