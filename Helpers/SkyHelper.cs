using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.Planets.PlanetWorld;
using static PlanetMod.Handlers.PlanetHandler;


namespace PlanetMod.Helpers
{
    public static class SkyHelper
    {
        public static Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

        public static Vector2 planetBackgroundPoint = new Vector2(Main.screenWidth / 2, Main.screenHeight * 2);

        public static float TimeRotation()
        {
            return -(Main.dayTime ? ((float)Main.time / 86400) * 6.28f : (((float)Main.time + 54001) / 86400) * 6.28f);
        }

        public static float ZoomOffset()
        {
            return ((Main.GameZoomTarget - 1) * 0.1f);
        }

        public static Vector2 PlanetPosition()
        {
            return new Vector2(Main.screenWidth / 2, (Main.screenHeight / 2) + ((planetArray[SelectedPlanet].ShipBackroundTexture.Frame().Height) * (planetArray[SelectedPlanet].BgPlanetPosY - ZoomOffset())))
                    .RotatedBy(shipViewRotation, screenCenter);
        }

        public static Vector2 OrbitPosition(Vector2 origin, Vector2 offset, float timeOffset = 0, int orbitSpeedMult = 1)//orbiting planet position
        {
            return offset.RotatedBy((TimeRotation() * orbitSpeedMult) + timeOffset, origin);//stuff gets weird if the first vector is not the center of the screen, but this must get changed for variable distance
        }

        public static Color PlanetColor(int opacity = 255)//day cycle darkening/lightening
        {
            int brightness = (int)(128 * (((float)(Math.Sin(TimeRotation() - 0.628f) + 1f) * 0.9f) + 0.2f));
            return new Color(brightness, brightness, brightness, opacity);
        }
    }
}