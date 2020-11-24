using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using Terraria.ModLoader.IO;
using System;
using SubworldLibrary;
using PlanetMod.Planets;
using static PlanetMod.Handlers.ShipBuildingHandler;
using static PlanetMod.Handlers.PlanetHandler;
using PlanetMod.Utils;
using PlanetMod.Helpers;

namespace PlanetMod.Ship
{
	public class ShipWorld : ModWorld
	{
		//SYNC: Not sure if needed to sync, since unused
		public static float shipViewRotation = 0f; //6.28 is a full rotation

		//public override void PreUpdate()
		//{
		//	//thrusterMode = 2;//debug
		//	//thrusterStyle = 0;//debug
		//}

		public override void PostUpdate()
		{

		}

		public override void PostDrawTiles()//drawing ship previews
		{
			//This method does not have it's own spritebatch, so it makes it a good place to begin one
			Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);
			Texture2D tex = ModContent.GetTexture("PlanetMod/Tiles/SelectedOverlay");

			if (selectedDoorPosition.X > 0 || selectedDoorPosition.Y > 0)//if door is not default position (outside world)
			{
				int width = (selectedDoorDirection >= 2) ? 64 : 32; //width and height here are just for drawing
				int height = (selectedDoorDirection >= 2) ? 32 : 64; 

				Main.spriteBatch.Draw(tex, new Rectangle((int)((selectedDoorPosition.X * 16) - Main.screenPosition.X), (int)((selectedDoorPosition.Y * 16) - Main.screenPosition.Y), width, height), new Color(255, 255, 255, 180));
			}

			if (selectedShipStructPosition.X > 0 || selectedShipStructPosition.Y > 0)//if preview is not default position (outside world, always offset from door pos if active)
			{   //ship drawing (there is protection before setting type to over the max, but protection may need to be added here in the future)
				Main.spriteBatch.Draw(tex, new Rectangle((int)((selectedShipStructPosition.X * 16) - Main.screenPosition.X), (int)((selectedShipStructPosition.Y * 16) - Main.screenPosition.Y), ShipStructureSizes[selectedShipStructType].X * 16, ShipStructureSizes[selectedShipStructType].Y * 16), new Color(255, 255, 255, 80));
				Main.spriteBatch.Draw(PlanetMod.overlayArray[selectedShipStructType], (new Vector2(selectedShipStructPosition.X, selectedShipStructPosition.Y) * 16) - Main.screenPosition, PlanetMod.overlayArray[selectedShipStructType].Frame(), Color.GreenYellow, 0f, default, 1, default, default);
			}

			Main.spriteBatch.End();
		}
	}
}