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
using System;
using static System.Math;
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using System.Globalization;
using PlanetMod.Utils;

namespace PlanetMod.Helpers
{
    public static class WorldgenHelper
	{
		#region utils
		public static bool ValidLocation(int i, int j)
		{
			if (i >= Main.maxTilesX || i <= 0 || j >= Main.maxTilesY || j <= 0)
				return false;
			else
				return true;
		}

		public static void ReframeWorldSideFix()
		{
			for (int i = (int)(Main.maxTilesX * 0.95f); i < Main.maxTilesX; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
			{
				for (int j = (int)(Main.maxTilesY * 0.3f); j < Main.maxTilesY; j++)
				{
					WorldGen.TileFrame(i, j);
				}
			}
		}

		public static void PlaceSpawnTileOnSurface(int posX)
		{
			Main.spawnTileX = posX;
			Main.spawnTileY = GetSurfaceTile(posX);
		}

		public static int GetSurfaceTile(int posX)
		{
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				if (Main.tile[posX, j].active())
				{
					return j;
				}
			}
			return Main.maxTilesY;
		}
		#endregion

		#region iterate-world methods
		public static void WorldFill(Action<int, int> iterateMethod)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					iterateMethod(i, j);
				}
			}
		}

		public static void FlatFill(int surfaceHeight, Action<int, int> iterateMethod)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = surfaceHeight; j < Main.maxTilesY; j++)
				{
					iterateMethod(i, j);
				}
			}
		}

		public static void SineFill(int surfaceHeight, int amp, int freq, Action<int, int> iterateMethod)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int startLevel = surfaceHeight + (int)(Math.Sin(i / (float)freq) * amp);

				for (int j = startLevel; j < Main.maxTilesY; j++)
				{
					iterateMethod(i, j);
				}
			}
		}

		public static void NoiseFill(int surfaceHeight, FastNoise.NoiseType noiseType, Action<int, int> iterateMethod)
		{
			int[] randomOffsets = NoiseHelper.Get2dDisplacements(Main.maxTilesX, 0.02f, 75, 1, WorldGen._genRandSeed, noiseType);
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int startLevel = surfaceHeight + randomOffsets[i];
				for (int j = startLevel; j < Main.maxTilesY; j++)
				{
					iterateMethod(i, j);
				}
			}
		}
		#endregion

		//#region legacy simple worldfills
		//public static void FlatFill(int tileType, int worldHeight)
		//{
		//	for (int i = 0; i < Main.maxTilesX; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
		//	{
		//		for (int j = worldHeight; j < Main.maxTilesY; j++)
		//		{
		//			Tile targetTile = Main.tile[i, j];

		//			targetTile.active(true);
		//			targetTile.type = (ushort)tileType;
		//		}
		//	}
		//}

		//public static void SimpleSineFill(int tileType, int worldHeight, int amp, int freq)
		//{
		//	for (int i = 0; i < Main.maxTilesX; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
		//	{
		//		int startLevel = worldHeight + (int)(Math.Sin(i / (float)freq) * amp);

		//		for (int j = startLevel; j < Main.maxTilesY; j++)
		//		{
		//			Tile targetTile = Main.tile[i, j];

		//			targetTile.active(true);
		//			targetTile.type = (ushort)tileType;
		//		}
		//	}
		//}
		//#endregion

		#region main worldgen passes
		//to be replaced with noise and/or passed method for block types
		public static void DuneWorldFill(int surfaceTileType, int stoneTileType, int worldHeight, int stoneDepth, int amp, int freq) 
		{
			float randSeed = WorldGen.genRand.NextFloat(100); //find better seed
			for (int i = 0; i < Main.maxTilesX; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
			{
				float a = (float)((Sin(((float)i / freq) + randSeed) + (Pow(Sin(((float)i / freq) * 1.2f - randSeed), 2f) * 2f)) / 4f);
				float b = (float)(((a * 1.5f) + Sin(((float)i / freq) + 0.8f - randSeed)) / 2.5f);

				int heightA = (int)((a * amp) + worldHeight - 5);
				int heightB = (int)((b * amp) + worldHeight);

				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile targetTile = Main.tile[i, j];

					if (j > (heightB + stoneDepth))
					{
						targetTile.active(true);
						targetTile.type = (ushort)stoneTileType;
					}
					else if(j > heightA || j > heightB)
					{
						targetTile.active(true);
						targetTile.type = (ushort)surfaceTileType;
					}
				}
			}
		}

		public static void CraterGen(int tileType, float chancePercent, int minSize, int maxSize)
		{
			for (int i = maxSize; i < Main.maxTilesX - maxSize; i++)
			{
				if(WorldGen.genRand.NextFloat(100) <= chancePercent)
				{
					int size = WorldGen.genRand.Next(minSize, maxSize);

					for (int j = 0; j < Main.maxTilesY; j++)
					{
						Tile targetTile = Main.tile[i, j];

						if (targetTile.active())
						{
							CircleGen(i, j - (int)(size / 3), size, RemoveTile);
							CoverCrater(tileType, i, (size * 2) - 2, size / WorldGen.genRand.Next(5, 7));//size has -2 to stop the single block from being placed on each side of a crater (less at least), depth is a bit random
							break;
						}
					}


					i += size;
				}
			}
		}

		//advanced caves using tendril gen
		public static void GenCaveStyle1(int worldHeight, float caveChancePercent, int caveMinSize, int caveMaxSize, int caveMinCount, int caveMaxCount, float tendrilChancePercent, int tendrilMinCount, int tendrilMaxCount, float tendrilMinCurve, float tendrilMaxCurve, float tendrilEndCurveMult)//TODO, try a seperate tendril gen via circles (same but next point is the same each time, decreasing sizes)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = worldHeight; j < Main.maxTilesY; j++)
				{
					if (WorldGen.genRand.NextFloat(100) <= caveChancePercent)
					{
						Vector2 NextCirclePos = new Vector2(i, j);
						int randCount = WorldGen.genRand.Next(caveMinCount, caveMaxCount);
						for (int k = 0; k < randCount; k++)
						{
							int randSize = WorldGen.genRand.Next(caveMinSize, caveMaxSize);

							if (WorldGen.genRand.NextFloat(100) <= tendrilChancePercent)
							{
								TendrilCave((int)NextCirclePos.X, (int)NextCirclePos.Y, randSize, 1, tendrilMinCount, tendrilMaxCount, WorldGen.genRand.NextFloat(tendrilMinCurve, tendrilMaxCurve), WorldGen.genRand.NextFloat(tendrilMaxCurve, tendrilMaxCurve * tendrilEndCurveMult));
							}
							else
							{
								CircleGen((int)NextCirclePos.X, (int)NextCirclePos.Y, randSize, RemoveTile);
							}

							NextCirclePos = new Vector2(NextCirclePos.X, NextCirclePos.Y + randSize).RotatedBy(WorldGen.genRand.NextFloat(0f, 6.28f), NextCirclePos);
						}
					}
				}
			}
		}
		#endregion

		#region feature passes //make these passed methods
		public static void ChasmGen(int worldHeight, float chancePercent, float horizontalChancePercent, int minWidth, int minLength, int maxWidth, int maxLength)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = worldHeight; j < Main.maxTilesY; j++)
				{
					if (WorldGen.genRand.NextFloat(100) <= chancePercent)
					{
						int order = WorldGen.genRand.Next(3);
						int width = WorldGen.genRand.Next(minWidth, maxWidth);
						int length = WorldGen.genRand.Next(minLength, maxLength);
						bool hori = WorldGen.genRand.NextFloat(100) <= horizontalChancePercent;

						int originX = 0;
						for (int k = -(length / 2); k < length / 2; k++)
						{
							float chasmWidth = ((width + 0.50f) - ((Math.Abs(k * (width + 0.50f))) / (length * 0.9f)));

							for (int e = -(int)(chasmWidth / 2); e < (int)((chasmWidth / 2) - 0.50f); e++)
							{
								int placePosX = i + (!hori ? originX + e : k);
								int placePosY = j + (hori ? originX + e : k);

								//if (placePosX >= Main.maxTilesX || placePosX <= 0 || placePosY >= Main.maxTilesY || placePosY <= 0)
								//	break;

								if (ValidLocation(placePosX, placePosY))
								{
									Tile targetTile = Main.tile[placePosX, placePosY];
									targetTile.active(false);
								}
							}
							if (WorldGen.genRand.Next(order) == 0)
								originX += WorldGen.genRand.Next(-1, 2);
						}
					}
				}
			}
		}

		//may pass a method instead of a tile-type
		public static void SnakeChunks(int tileType, int worldHeight, float chancePercent, int clusterSize, int repetitions, int originTile = -1)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = worldHeight; j < Main.maxTilesY; j++)
				{
					if (WorldGen.genRand.NextFloat(100) <= chancePercent)
					{
						if (originTile > -1 && Main.tile[i, j].type != originTile)
							continue;

						for (int e = 0; e < repetitions; e++)
						{
							int posX = 0;
							int posY = 0;
							for (int k = 0; k < clusterSize; k++)
							{
								int placePosX = i + posX;
								int placePosY = j + posY;

								//if (placePosX >= Main.maxTilesX || placePosX <= 0 || placePosY >= Main.maxTilesY || placePosY <= 0)
								//	break; //old way, used to stop it immediately if outside world

								if (ValidLocation(placePosX, placePosY))
								{
									Tile placeTile = Main.tile[placePosX, placePosY];
									if (placeTile.active())
										placeTile.type = (ushort)tileType;

									if (WorldGen.genRand.NextBool())//gotta be a better way for this
										posX += WorldGen.genRand.NextBool() ? -1 : 1;
									else
										posY += WorldGen.genRand.NextBool() ? -1 : 1;
								}
							}
						}
					}
				}
			}
		}

		public static void TubeCaves(int startPosX, int startSize, int length, float branchChance, float angleDeviation, float curveFreq, float curveAmp)
		{
			//init
			int startPosY = GetSurfaceTile(startPosX);
			List<TubeCrawler> crawlers = new List<TubeCrawler> { new TubeCrawler { pos = new Vector2(startPosX, startPosY), curveMult = 0f, curveOffset = 0f, sizeOffset = 0 } };

			//main loop
			for (int k = 0; k < length; k++)
			{
				float size = MathHelper.Lerp(startSize, 0, (float)k / (float)length);

				for (int f = 0; f < crawlers.Count; f++)//for each tunnel crawler
				{
					if (size + crawlers[f].sizeOffset < 0)
					{
						crawlers.RemoveAt(f);
						continue;
					}

					CircleGen((int)crawlers[f].pos.X, (int)crawlers[f].pos.Y, size + crawlers[f].sizeOffset, RemoveTile);

					//sine
					float curveAmount = (float)Sin(((float)k - crawlers[f].curveOffset) * curveFreq * (crawlers[f].curveMult + 1)) * curveAmp * (crawlers[f].curveMult + 1);

					if (WorldGen.genRand.NextFloat(100) <= branchChance)//add new crawler
					{
						crawlers.Add(new TubeCrawler { pos = crawlers[f].pos, curveMult = crawlers[f].curveMult + WorldGen.genRand.NextFloat(-angleDeviation, angleDeviation), curveOffset = crawlers[f].curveOffset + k, sizeOffset = WorldGen.genRand.NextFloat(crawlers[f].sizeOffset - 2f, crawlers[f].sizeOffset - 0.5f) });
					}

					crawlers[f].pos = new Vector2(crawlers[f].pos.X, crawlers[f].pos.Y + size + crawlers[f].sizeOffset).RotatedBy(curveAmount + crawlers[f].curveMult, crawlers[f].pos);

					if (crawlers[f].curveMult > (crawlers[f].pos - new Vector2(crawlers[f].pos.X, Main.maxTilesY)).ToRotation())
					{
						crawlers[f].curveMult *= 0.97f;
					}
				}
			}
		}
		#endregion

		#region single details
		public static void TendrilCave(int i, int j, int startSize, int endSize, int minCount, int maxCount, float startCurve, float endCurve)//TODO, try a seperate tendril gen via circles (same but next point is the same each time, decreasing sizes)
		{
			int randCount = WorldGen.genRand.Next(minCount, maxCount);
			float randomDirection = WorldGen.genRand.NextFloat(0f, 6.28f);
			int curveDirection = WorldGen.genRand.NextBool() ? 1 : -1;

			Vector2 nextCirclePos = new Vector2(i, j);
			for (int k = 0; k < randCount; k++)
			{
				int size = (int)MathHelper.Lerp(startSize, endSize, (float)k / (float)randCount);
				float curve = MathHelper.Lerp(startCurve, endCurve, (float)k / (float)randCount);

				CircleGen((int)nextCirclePos.X, (int)nextCirclePos.Y, size, RemoveTile);

				nextCirclePos = new Vector2(nextCirclePos.X, nextCirclePos.Y + size).RotatedBy(randomDirection + (curve * curveDirection), nextCirclePos);
			}
		}

		public static void CircleGen(int posX, int posY, float size, Action<int, int> iterateMethod)
		{
			for (int i = -(int)size; i < (int)size; i++)
			{
				for (int j = -(int)size; j < (int)size; j++)
				{
					if (Vector2.Distance(new Vector2(i, j), Vector2.Zero) < size && ValidLocation(posX + i, posY + j))
					{
						iterateMethod(posX + i, posY + j);
					}
				}
			}
		}

		public static void CoverCrater(int tileType, int posX, int width, int depth)
		{
			for (int i = -(int)width/2; i < (int)width / 2; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile targetTile = Main.tile[posX + i, j];

					if (targetTile.active())
					{
						int slopeDepth = (int)((depth + 0.75f) - ((Math.Abs(i * (depth + 0.75f))) / (width * 0.9f)));//eek
						for (int k = 1; k < slopeDepth + 1; k++)
						{
							if(ValidLocation(posX + i, j - k))
							{
								Tile placeTile = Main.tile[posX + i, j - k];

								placeTile.active(true);
								placeTile.type = (ushort)tileType;
							}
						}
						break;
					}
				}
			}
		}
		#endregion

		#region passed action-methods
		public static void AloneBlockCleanup(int i, int j)
		{
			Tile targetTile = Main.tile[i, j];

			if (targetTile.active())
			{
				int loneliness = 0;

				for (int h = -1; h <= 1; h++)
				{
					for (int k = -1; k <= 1; k++)
					{
						if(ValidLocation(i + h, j + k) && !Main.tile[i + h, j + k].active())
						{
							loneliness++;
						}
					}
				}

				if(loneliness >= 6)
				{
					targetTile.active(false);
					//targetTile.type = TileID.RubyGemspark;
				}
			}
		}
		public static void PlaceLiquid(int i, int j, int liquidType)
		{
			Tile targetTile = Main.tile[i, j];

			if (!targetTile.active())
			{
				targetTile.liquid = 255;
				targetTile.liquidType(liquidType);
			}
		}
		public static void PlaceLiquid(int i, int j, int liquidType, byte liquidLevel)
		{
			Tile targetTile = Main.tile[i, j];

			if (!targetTile.active())
			{
				targetTile.liquid = liquidLevel;
				targetTile.liquidType(liquidType);
			}
		}
		public static void RemoveTile(int i, int j)
		{
			Tile targetTile = Main.tile[i, j];
			targetTile.active(false);
		}
		public static void RemoveTile(int i, int j, int replaceTileType)
		{
			Tile targetTile = Main.tile[i, j];
			if (targetTile.active() && targetTile.type == (ushort)replaceTileType)
			{
				targetTile.active(false);
			}
		}
		public static void PlaceTile(int i, int j, int tileType)//checks if empty (pass this to set the extra parameter: (x, y) => ReplaceTile(x, y, TileID.Stone) )
		{
			Tile targetTile = Main.tile[i, j];
			if (!targetTile.active())
			{
				targetTile.active(true);
				targetTile.type = (ushort)tileType;
			}
		}
		public static void ReplaceTile(int i, int j, int tileType)//does not check if empty
		{
			Tile targetTile = Main.tile[i, j];

			targetTile.active(true);
			targetTile.type = (ushort)tileType;
		}
		public static void ReplaceTile(int i, int j, int tileType, int replaceTileType)//only replaces specified type
		{
			Tile targetTile = Main.tile[i, j];
			if(targetTile.active() && targetTile.type == (ushort)replaceTileType)
			{
				targetTile.type = (ushort)tileType;
			}
		}
		#endregion
	}

    public class TubeCrawler
	{
		public Vector2 pos;
		public float curveMult;
		public float curveOffset;
		public float sizeOffset;
	}
}