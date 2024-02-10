using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using System.Xml.Serialization;

namespace MagicalForestLocation
{
    [XmlType("Mods_Logophile_MagicalForestLocation_MagicForest")]  //Serialization
    public class MagicForest : MagicForestLocation
    {


        internal static IMonitor Monitor { get; set; }

        protected NetInt _currentState = new NetInt();

        protected Color _ambientLightColor = Color.White;

        protected Texture2D _rayTexture;

        protected Random _rayRandom;

        protected int _raySeed;

        public MagicForest()
        {
        }

        public MagicForest(IModContentHelper content)
        : base(content, "Tangy_SpiritGrotto", "Tangy_SpiritGrotto") //This must match the name of your .TMX file, to load it.
        {

        }

        protected override void resetLocalState()
        {
            _raySeed = (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
            _rayTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\LightRays");
            _ambientLightColor = new Color(150, 120, 50);
            ignoreOutdoorLighting.Value = false;

            base.resetLocalState();
            _updateWoodsLighting();
        }
         public override void draw(SpriteBatch b)  //This is just here if ever you should need it.
        {
            base.draw(b);

        }

        public override void UpdateWhenCurrentLocation(GameTime time) //This is from IslandForestLocation.cs
        {
            base.UpdateWhenCurrentLocation(time);
            _updateWoodsLighting();

            //GameTime updates like every tick. If something is supposed to move or do stuff or something, probably it's going to need an update like this in it.

        }
        protected void _updateWoodsLighting() //This is from IslandForestLocation.cs
        {
            if (Game1.currentLocation == this)
            {
                int fade_start_time = Utility.ConvertTimeToMinutes(Game1.getModeratelyDarkTime()) - 60;
                int fade_end_time = Utility.ConvertTimeToMinutes(Game1.getTrulyDarkTime());
                int light_fade_start_time = Utility.ConvertTimeToMinutes(Game1.getStartingToGetDarkTime());
                int light_fade_end_time = Utility.ConvertTimeToMinutes(Game1.getModeratelyDarkTime());
                float num = (float)Utility.ConvertTimeToMinutes(Game1.timeOfDay) + (float)Game1.gameTimeInterval / 7000f * 10f;
                float lerp = Utility.Clamp((num - (float)fade_start_time) / (float)(fade_end_time - fade_start_time), 0f, 1f);
                float light_lerp = Utility.Clamp((num - (float)light_fade_start_time) / (float)(light_fade_end_time - light_fade_start_time), 0f, 1f);
                Game1.ambientLight.R = (byte)Utility.Lerp((int)_ambientLightColor.R, (int)Game1.eveningColor.R, lerp);
                Game1.ambientLight.G = (byte)Utility.Lerp((int)_ambientLightColor.G, (int)Game1.eveningColor.G, lerp);
                Game1.ambientLight.B = (byte)Utility.Lerp((int)_ambientLightColor.B, (int)Game1.eveningColor.B, lerp);
                Game1.ambientLight.A = (byte)Utility.Lerp((int)_ambientLightColor.A, (int)Game1.eveningColor.A, lerp);
                Color light_color = Color.Black;
                light_color.A = (byte)Utility.Lerp(255f, 0f, light_lerp);
                foreach (LightSource light in Game1.currentLightSources)
                {
                    if (light.lightContext.Value == LightSource.LightContext.MapLight)
                    {
                        light.color.Value = light_color;
                    }
                }
            }
        }

        public virtual void DrawRays(SpriteBatch b)  //This is from IslandForestLocation.cs
        {
            Random random = new Random(_raySeed);
            float zoom = (float)Game1.graphics.GraphicsDevice.Viewport.Height * 0.6f / 128f;
            int num = -(int)(128f / zoom);
            int max = Game1.graphics.GraphicsDevice.Viewport.Width / (int)(32f * zoom);
            for (int i = num; i < max; i++)
            {
                Color color = Color.White;
                float deg2 = (float)Game1.viewport.X * Utility.RandomFloat(0.75f, 1f, random) + (float)Game1.viewport.Y * Utility.RandomFloat(0.2f, 0.5f, random) + (float)Game1.currentGameTime.TotalGameTime.TotalSeconds * 20f;
                _ = deg2 / 360f;
                deg2 %= 360f;
                float rad = deg2 * ((float)Math.PI / 180f);
                color *= Utility.Clamp((float)Math.Sin(rad), 0f, 1f) * Utility.RandomFloat(0.15f, 0.4f, random);
                float offset = Utility.Lerp(0f - Utility.RandomFloat(24f, 32f, random), 0f, deg2 / 360f);
                b.Draw(_rayTexture, new Vector2(((float)(i * 32) - offset) * zoom, Utility.RandomFloat(0f, -32f * zoom, random)), new Microsoft.Xna.Framework.Rectangle(128 * random.Next(0, 2), 0, 128, 128), color, 0f, Vector2.Zero, zoom, SpriteEffects.None, 1f);
            }
        }

        public override void drawAboveAlwaysFrontLayer(SpriteBatch b) //This is from IslandForestLocation.cs
        {
            base.drawAboveAlwaysFrontLayer(b);
            DrawRays(b);
        }


    }
}