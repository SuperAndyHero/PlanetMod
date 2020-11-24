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
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.Planets.PlanetWorld;
using static PlanetMod.Handlers.PlanetHandler;
using static PlanetMod.Handlers.ShipBuildingHandler;
using static PlanetMod.Handlers.TeleportHandler;
using PlanetMod.Planets;

namespace PlanetMod.Ship
{
	public class ShipSubworld : Subworld
	{

		public override int width => 600;
		public override int height => 800;//high height to hide background bug thats in subworld lib's side

		public override bool saveSubworld => false;
		//public override bool saveSubworld => true; //disabled for testing

		public override bool saveModData => false;
		//public override bool saveModData => true; //disabled for testing

		public override bool noWorldUpdate => false;

		public override void Load()
		{
			ClearSelectedAll();
			//SLWorld.noReturn = true;
			//ShipWorld.shipViewRotation = 0; //(1.57f is 90 dec, 3.14 is 180) default direction
		}
		public override void Unload()
		{
			ClearSelectedAll();
		}

		public override void OnVotedFor()
		{
			StartTeleport();
		}

		#region worldgen methods
		public override List<GenPass> tasks => new List<GenPass>()
		{
			new SubworldGenPass(GenSetup), new SubworldGenPass(GenFillWorld), new SubworldGenPass(GenShip)
		};

		private void GenSetup(GenerationProgress progress)
		{
			progress.Message = "Initializing"; //Sets the text above the worldgen progress bar

			//sets the spawnpoint to the center of the world
			Main.spawnTileX = (int)(Main.maxTilesX / 2);
			Main.spawnTileY = (int)(Main.maxTilesY / 2);

			//Sets the world layers to the buttom of the world, so everything is overworld
			Main.worldSurface = Main.maxTilesY;
			Main.rockLayer = Main.maxTilesY;
		}

		private void GenFillWorld(GenerationProgress progress)
		{
			progress.Message = "Filling World";

			for (int i = 0; i < Main.maxTilesX; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
			{
				for (int j = 0; j < Main.maxTilesY; j++)//TODO switch to worldgen method-passing
				{
					Tile targetTile = Main.tile[i, j];

					targetTile.active(true);
					targetTile.type = (ushort)ModContent.TileType<Tiles.Barrier>();
					targetTile.frameX = short.MaxValue;
				}
			}

			//Main.tile[Main.spawnTileX, Main.spawnTileY].type = TileID.DiamondGemspark;//spawn tile marker
		}

		private void GenShip(GenerationProgress progress)
		{
			progress.Message = "Creating Ship";
			PlaceShipStructure(new Point16(Main.spawnTileX, Main.spawnTileY - 4), (ushort)ShipID.Deck, true);
			//StructureHelper.StructureHelper.GenerateStructure("Ship/ShipStructures/ShipDeck", new Point16(Main.spawnTileX - 8, Main.spawnTileY - 12), PlanetMod.Instance);
		}
		#endregion
	}

	public class SubworldSpawns : GlobalNPC //(Not ship specific)
	{
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			int worldID = GetCurrentWorldID();

			if (worldID == -1 || worldID == 1)//disables all spawns while in world
			{
				pool.Clear();
			}
		}
	}
}