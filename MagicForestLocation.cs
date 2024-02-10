using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System.Xml.Serialization;

namespace MagicalForestLocation
{
    [XmlType("Mods_Logophile_MagicalForestLocation_MagicForestLocation")]  //Save Serialization Stuff
    public class MagicForestLocation : GameLocation //Inherits GameLocation class
    {
        public MagicForestLocation() { } //The constructor if ever you want to call an instance of this class!
        public MagicForestLocation(IModContentHelper content, string mapPath, string mapName)
        : base(content.GetInternalAssetName("assets/maps/" + mapPath + ".tmx").BaseName, "Custom_" + mapName) //This loads the map
        {

        }
        protected override void initNetFields()
        {
            base.initNetFields();
        }

        protected override void resetLocalState()
        {
            base.resetLocalState();
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
        }

        //Those three are left here should you desire them for something later. They are pretty useful for adding custom stuff to maps. Feel free to look at my code and use whatever you want, it's MIT!!! :)
    }
}