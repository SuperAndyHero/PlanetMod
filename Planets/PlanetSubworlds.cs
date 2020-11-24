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
using static PlanetMod.Helpers.WorldgenHelper;
using static PlanetMod.Handlers.TeleportHandler;
using PlanetMod.Utils;
using PlanetMod.Helpers;

namespace PlanetMod.Planets
{
	public class MoonEarth : Subworld
	{
		public override int width => 3500;//3DS small world size
		public override int height => 1800;
		public override bool saveSubworld => false;
		//public override bool saveSubworld => true; //disabled for testing
		public override bool saveModData => false;
		//public override bool saveModData => true; //disabled for testing
		public override bool noWorldUpdate => false;


		public override void Load() 
		{ 
			SLWorld.drawUnderworldBackground = true;
			//SLWorld.noReturn = true;
		}
		public override void Unload() 
		{

		}
		public override void OnVotedFor()
		{
			StartTeleport();//starts teleporter animation
		}

		#region worldgen methods
		public override List<GenPass> tasks => new List<GenPass>()
		{
			new SubworldGenPass(GenSetup), new SubworldGenPass(GenFillWorld), new SubworldGenPass(SurfaceDetails), new SubworldGenPass(BelowGround), new SubworldGenPass(GenCleanup)
		};

		private void GenSetup(GenerationProgress progress)
		{
			progress.Message = "Initializing";

			Main.worldSurface = Main.maxTilesY * 0.32f;
			Main.rockLayer = Main.maxTilesY * 0.5f;
		}

		private void GenFillWorld(GenerationProgress progress)
		{
			progress.Message = "Filling World";
			//FlatFill(ModContent.TileType<Tiles.MoonTiles.MoonTurf>(), (int)(Main.maxTilesY * 0.3));
			//SineFill(ModContent.TileType<Tiles.MoonTiles.MoonTurf>(), (int)(Main.maxTilesY * 0.3), 12, 50);

			DuneWorldFill(ModContent.TileType<Tiles.MoonTiles.MoonTurf>(), ModContent.TileType<Tiles.MoonTiles.MoonRock>(), (int)(Main.maxTilesY * 0.3), 10, 20, 35);
		}

		private void SurfaceDetails(GenerationProgress progress)
		{
			progress.Message = "Adding Craters";
			CraterGen(ModContent.TileType<Tiles.MoonTiles.MoonTurf>(), 1f, 5, 20);
		}

		private void BelowGround(GenerationProgress progress)
		{
			progress.Message = "Geology is happening";
			SnakeChunks(ModContent.TileType<Tiles.MineralTiles.Basalt>(), (int)(Main.maxTilesY * 0.3), 0.2f, 15, 10, ModContent.TileType<Tiles.MoonTiles.MoonRock>());
			ChasmGen((int)(Main.maxTilesY * 0.35), 0.05f, 0.3f, 3, 10, 10, 50);

			GenCaveStyle1((int)(Main.maxTilesY * 0.35),
				caveChancePercent: 0.07f,
				caveMinSize: 2,
				caveMaxSize: 6,
				caveMinCount: 5,
				caveMaxCount: 25,
				tendrilChancePercent: 10f,
				tendrilMinCount: 8,
				tendrilMaxCount: 32,
				tendrilMinCurve: 1f,
				tendrilMaxCurve: 4f,
				tendrilEndCurveMult: 3f);

			//TubeCaves((int)(Main.maxTilesX * 0.5), 12, 200, 3f, 0.8f, 0.35f, 0.60f);
		}

		private void GenCleanup(GenerationProgress progress)
		{
			progress.Message = "Finalizing";

			PlaceSpawnTileOnSurface((int)(Main.maxTilesX / 2));//places spawn tile on the topmost block
			WorldFill(AloneBlockCleanup);
			Main.tile[Main.spawnTileX, Main.spawnTileY].type = TileID.DiamondGemspark;//DEBUG spawn tile marker

			ReframeWorldSideFix();
		}
		#endregion
	}

	public class RockyWorld : Subworld
	{
		public override int width => 1600;
		public override int height => 800;
		public override bool saveSubworld => false;
		//public override bool saveSubworld => true; //disabled for testing
		public override bool saveModData => false;
		//public override bool saveModData => true; //disabled for testing
		public override bool noWorldUpdate => false;


		public override void Load() 
		{
			SLWorld.drawUnderworldBackground = true;
			SLWorld.noReturn = true;
		}
		public override void Unload() 
		{

		}
		public override void OnVotedFor()
		{
			StartTeleport();
		}

		#region worldgen methods
		public override List<GenPass> tasks => new List<GenPass>()
		{
			new SubworldGenPass(GenSetup), new SubworldGenPass(GenFillWorld)
		};

		private void GenSetup(GenerationProgress progress)
		{
			progress.Message = "Initializing";
			Main.spawnTileX = (int)(Main.maxTilesX / 2);
			Main.spawnTileY = (int)(Main.maxTilesY / 2);

			Main.worldSurface = Main.maxTilesY * 0.75f;
			Main.rockLayer = Main.maxTilesY * 0.9f;
		}

		private void GenFillWorld(GenerationProgress progress)
		{
			progress.Message = "Filling World";
			NoiseFill((int)(Main.maxTilesY / 2), FastNoise.NoiseType.ValueFractal, (x, y) => ReplaceTile(x, y, TileID.Stone));
			SineFill((int)(Main.maxTilesY / 2) + 25, WorldGen.genRand.Next(8, 12), 10, (x, y) => PlaceLiquid(x, y, 0, 128));
			WorldFill(AloneBlockCleanup);
			//Main.tile[Main.spawnTileX, Main.spawnTileY].type = TileID.DiamondGemspark;//spawn tile marker
		}

		private void TestGen(int i, int j)
		{
			if (WorldGen.genRand.Next(1000) == 0)
			{
				
			}
		}
		#endregion
	}
}