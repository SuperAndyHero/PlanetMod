using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
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
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using PlanetMod.Handlers;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.Planets.PlanetWorld;
using static PlanetMod.Handlers.ShipBuildingHandler;
using static PlanetMod.Handlers.PlanetHandler;
using static PlanetMod.Handlers.TeleportHandler;
using static Terraria.ModLoader.ModContent;
using IL.Terraria.Graphics;
using PlanetMod.Skies;
using PlanetMod.Planets;
using IL.Terraria.GameContent.Events;
using Terraria.GameContent.Events;

namespace PlanetMod
{
	public class PlanetMod : Mod
	{
		public const int starArraySize = 8;  //amount of stars
		public static Texture2D[] starTexArray;

		public const int galaxySmallArraySize = 14;  //amount of small galaxies
		public static Texture2D[] galaxySmallTexArray;

		public const int galaxyLargeArraySize = 6;  //amount of large galaxies
		public static Texture2D[] galaxyLargeTexArray;

		public static Texture2D spaceBackground;

		public const int shipPartArraySize = 11; //amount of foreground assets
		public static Texture2D[] shipPartArray;

		public const int shipWallArraySize = 5;  //amount of backround assets
		public static Texture2D[] shipWallArray;

		public const int overlayArraySize = 3;  //amount of room previews
		public static Texture2D[] overlayArray;

		public const int animArraySize = 14;  //amount of animations  
		public static Texture2D[] animArray;
		public static int[] animFrameCount = new int[animArraySize] { 2,  7,  154, 68, 14, 6,  72, 94, 60, 375, 128, 12, 63, 91};//frame count
		public static int[] animFrameDelay = new int[animArraySize] { 8,  9,  0,   1,  5,  10, 0,  0,  0,  0,   0,   1,  0,  4 };//frame delay (60th of a second)

		#region thrusters
		public const int thrusterStylesSize = 2;    //styles:	blue, orange
		public const int thrusterModesSize = 3;     //modes:		off, idle, active (each mode needs a frame number in the below array)
		public static Texture2D[] smallThrusterIdle;
		public static Texture2D[] smallThrusterActive;

		public static int[] thrusterFrameCount = new int[thrusterModesSize] { 0, 2, 5 };//For per texture frames: may have to use a different solution, or use a 2D array

		public static ref Texture2D GetThrusterTexture()//this may need to accept a input for styles-per-thruster
		{
			if(thrusterStyle < thrusterStylesSize)
			switch (thrusterMode)//replace thrusterMode with method input for styles-per-thruster
				{
				case 1:
					return ref smallThrusterIdle[thrusterStyle];
				case 2:
					return ref smallThrusterActive[thrusterStyle];
			}
			return ref Terraria.Graphics.TextureManager.BlankTexture;
		}
		#endregion

		public static int[] validPlasmaTiles;
		private void FillTileCasesLists()//this is called on load since on load `ModContent.TileType<>` returns 0 (replace with a property or something PLEASE -future me)
		{
			validPlasmaTiles = new int[] { ModContent.TileType<Tiles.PlasmaGlobeSmall>(), ModContent.TileType<Tiles.PlasmaGlobeMedium>(), ModContent.TileType<Tiles.PlasmaGlobeLarge>() };		
		}//replace with property...

		public void LoadImageArray( ref Texture2D[] array, int arraySize, string path)
		{
			array = new Texture2D[arraySize];
			for (int i = 0; i < arraySize; i++)
			{
				string strPath = path + i;
				array[i] = ModContent.GetTexture(strPath);
			}
		}

		public static PlanetMod Instance { get; set; }//No clue, was needed for structure lib
		public PlanetMod()
		{
			Instance = this;
		}
		
		public override void Load()
		{
			//may make a blank shader for this to help with performace
			Filter blankfilter = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(1f, 1f, 1f).UseOpacity(0f), EffectPriority.High);

			Filters.Scene["PlanetMod:ShipSky"] = blankfilter;
			SkyManager.Instance["PlanetMod:ShipSky"] = new ShipSky();

			Filters.Scene["PlanetMod:PlanetSky"] = blankfilter;
			SkyManager.Instance["PlanetMod:PlanetSky"] = new PlanetSky();

			IL.Terraria.Main.DoDraw += DrawMoonlordLayer;

			FillTileCasesLists();
			LoadFiles();
			LoadFileStream();
		}

		private void LoadFiles()//several for loops to load textures in bulk
		{
			//starTexArray = new Texture2D[allStarArray];
			//for (int i = 0; i < allStarArray; i++)
			//{
			//	string path = "PlanetMod/Skies/Stars/Star_" + i;
			//	starTexArray[i] = ModContent.GetTexture(path);
			//} //old example

			LoadImageArray(ref animArray, animArraySize, "PlanetMod/Animations/Anim_");

			LoadImageArray(ref starTexArray, starArraySize, "PlanetMod/Skies/Stars/Star_");

			LoadImageArray(ref galaxySmallTexArray, galaxySmallArraySize, "PlanetMod/Skies/Galaxy_Small/Galaxy_Small_");

			LoadImageArray(ref galaxyLargeTexArray, galaxyLargeArraySize, "PlanetMod/Skies/Galaxy_Large/Galaxy_Large_");

			LoadImageArray(ref shipPartArray, shipPartArraySize, "PlanetMod/Ship/ShipParts/ShipPart_");

			LoadImageArray(ref shipWallArray, shipWallArraySize, "PlanetMod/Ship/ShipWalls/ShipWall_");

			LoadImageArray(ref overlayArray, overlayArraySize, "PlanetMod/Ship/Overlays/ShipOverlay_");

			LoadImageArray(ref smallThrusterIdle, thrusterStylesSize, "PlanetMod/Ship/Thrusters/JetFlameIdle_");
			LoadImageArray(ref smallThrusterActive, thrusterStylesSize, "PlanetMod/Ship/Thrusters/JetFlameActive_");
		}

		private void LoadFileStream()//filestream loading stuff
		{
			byte[] file = ModContent.GetFileBytes("PlanetMod/Skies/MilkyWay.jpg");
			Stream stream = new MemoryStream(file);
			spaceBackground = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
		}

		private delegate void DrawMoonlordDelegate();
		private void DrawMoonlordLayer(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.TryGotoNext(n => n.MatchLdfld<Main>("DrawCacheNPCsMoonMoon"));
			c.Index--;

			c.EmitDelegate<DrawMoonlordDelegate>(EmitMoonlordLayerDel);
		}
		private void EmitMoonlordLayerDel()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (!Main.npc[i].active)
				{
					continue;
				}
				if (Main.npc[i].type == ModContent.NPCType<Npcs.ShipDummyNpc>())
				{
					int frame = (Main.npc[i].modNPC as Npcs.ShipDummyNpc).parent.frameY;

					if(frame > 0 && frame <= PlanetMod.shipWallArraySize)
					{
						Main.spriteBatch.Draw(PlanetMod.shipWallArray[frame - 1], Main.npc[i].position - Main.screenPosition, PlanetMod.shipWallArray[frame - 1].Frame(), Color.DarkGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		//public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		//{
		//	//int worldID = GetCurrentWorldID();
		//	//if (worldID == -1)//ship
		//	//{
		//	//	backgroundColor = Color.Black;
		//	//}
		//	//else if (planetArray[worldID].blackSky)//else, if subworld has blacksky set to true
		//	//{
		//	//	backgroundColor = Color.Black;
		//	//};
		//}

		public override void MidUpdateTimeWorld()
		{
			if (Subworld.AnyActive<PlanetMod>())//lotta stuff copied from vanilla
			{
				Main.UpdateSundial();
				Main.time += Main.dayRate;
				Terraria.GameContent.Events.BirthdayParty.UpdateTime();
				Terraria.GameContent.Events.DD2Event.UpdateTime();

				if (Main.time > 54000 && Main.dayTime)//replace main.daytime with speed adjust (add/mult)
				{
					Main.time = 0;
					Main.dayTime = !Main.dayTime;
				}
				else if (Main.time > 32400 && !Main.dayTime)
				{
					if (Main.fastForwardTime)
					{
						Main.fastForwardTime = false;
						Main.UpdateSundial();
					}
					Main.checkXMas();
					Main.checkHalloween();
					Main.AnglerQuestSwap();
					Terraria.GameContent.Events.BirthdayParty.CheckMorning();
					Main.time = 0;
					Main.dayTime = !Main.dayTime;
					if (Main.sundialCooldown > 0)
					{
						Main.sundialCooldown--;
					}
					Main.moonPhase++;
					if (Main.moonPhase >= 8)
					{
						Main.moonPhase = 0;
					}
				}
				UpdateTime_SpawnTownNPCs();//not 100% sure if this is the right place
			}

			if (IsTeleporting)
			{
				if (TeleportTimer >= TeleportTimerMax - 1)
				{
					//planetWorld.IsTeleporting = false; //these are now set in Init() in modworld
					//TeleportTimer = 0;

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						TeleportCorrectWorld(true);//teleports after animation ends
					}
				}
				else
				{
					TeleportTimer++;
				}
			}
			
			//this is gonna desync, but I dont think it will be an issue, maybe only run on single or multi client
			int tileEntityType = ModContent.TileEntityType<Tiles.PlasmaEntity>();
			foreach (var item in TileEntity.ByID)
			{
				if (item.Value.type == tileEntityType)
				{
					var currentEntity = item.Value as Tiles.PlasmaEntity;

					Point16 curEntPos = currentEntity.EntityPosition();
					if (Main.tile[curEntPos.X, curEntPos.Y].frameX == 0)
					{
						currentEntity.PlasmaInstance.UpdatePlasma();
					}
				}
			}

			SelectedPlanet = (ushort)PlanetID.RockyPlanet;//debug
		}

		private static void UpdateTime_SpawnTownNPCs()//entire town npc check (why am I even doing this on planets...)
		{
			if (Main.netMode != 1 && Main.worldRate > 0)
			{
				Main.checkForSpawns++;
				if (Main.checkForSpawns >= 7200 / Main.worldRate)
				{
					int num = 0;
					for (int i = 0; i < 255; i++)
					{
						if (Main.player[i].active)
						{
							num++;
						}
					}
					for (int j = 0; j < Main.townNPCCanSpawn.Length; j++)
					{
						Main.townNPCCanSpawn[j] = false;
					}
					Main.checkForSpawns = 0;
					WorldGen.prioritizedTownNPC = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					int num12 = 0;
					int num13 = 0;
					int num14 = 0;
					int num15 = 0;
					int num16 = 0;
					int num17 = 0;
					int num18 = 0;
					int num19 = 0;
					int num20 = 0;
					int num21 = 0;
					int num22 = 0;
					int num23 = 0;
					int num24 = 0;
					int num25 = 0;
					int num26 = 0;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && Main.npc[k].townNPC)
						{
							if (Main.npc[k].type != 368 && Main.npc[k].type != 37 && Main.npc[k].type != 453 && !Main.npc[k].homeless)
							{
								WorldGen.QuickFindHome(k);
							}
							if (Main.npc[k].type == 37)
							{
								num7++;
							}
							if (Main.npc[k].type == 17)
							{
								num2++;
							}
							if (Main.npc[k].type == 18)
							{
								num3++;
							}
							if (Main.npc[k].type == 19)
							{
								num5++;
							}
							if (Main.npc[k].type == 20)
							{
								num4++;
							}
							if (Main.npc[k].type == 22)
							{
								num6++;
							}
							if (Main.npc[k].type == 38)
							{
								num8++;
							}
							if (Main.npc[k].type == 54)
							{
								num9++;
							}
							if (Main.npc[k].type == 107)
							{
								num11++;
							}
							if (Main.npc[k].type == 108)
							{
								num10++;
							}
							if (Main.npc[k].type == 124)
							{
								num12++;
							}
							if (Main.npc[k].type == 142)
							{
								num13++;
							}
							if (Main.npc[k].type == 160)
							{
								num14++;
							}
							if (Main.npc[k].type == 178)
							{
								num15++;
							}
							if (Main.npc[k].type == 207)
							{
								num16++;
							}
							if (Main.npc[k].type == 208)
							{
								num17++;
							}
							if (Main.npc[k].type == 209)
							{
								num18++;
							}
							if (Main.npc[k].type == 227)
							{
								num19++;
							}
							if (Main.npc[k].type == 228)
							{
								num20++;
							}
							if (Main.npc[k].type == 229)
							{
								num21++;
							}
							if (Main.npc[k].type == 353)
							{
								num22++;
							}
							if (Main.npc[k].type == 369)
							{
								num23++;
							}
							if (Main.npc[k].type == 441)
							{
								num24++;
							}
							if (Main.npc[k].type == 550)
							{
								num25++;
							}
							num26++;
						}
					}
					if (WorldGen.prioritizedTownNPC == 0)
					{
						int num27 = 0;
						bool flag = false;
						int num28 = 0;
						bool flag2 = false;
						bool flag3 = false;
						bool flag4 = false;
						bool flag5 = false;
						for (int l = 0; l < 255; l++)
						{
							if (Main.player[l].active)
							{
								for (int m = 0; m < 58; m++)
								{
									if ((Main.player[l].inventory[m] != null) & (Main.player[l].inventory[m].stack > 0))
									{
										if (num27 < 2000000000)
										{
											if (Main.player[l].inventory[m].type == 71)
											{
												num27 += Main.player[l].inventory[m].stack;
											}
											if (Main.player[l].inventory[m].type == 72)
											{
												num27 += Main.player[l].inventory[m].stack * 100;
											}
											if (Main.player[l].inventory[m].type == 73)
											{
												num27 += Main.player[l].inventory[m].stack * 10000;
											}
											if (Main.player[l].inventory[m].type == 74)
											{
												num27 += Main.player[l].inventory[m].stack * 1000000;
											}
										}
										if (Main.player[l].inventory[m].ammo == AmmoID.Bullet || Main.player[l].inventory[m].useAmmo == AmmoID.Bullet)
										{
											flag2 = true;
										}
										if (Main.player[l].inventory[m].type == 166 || Main.player[l].inventory[m].type == 167 || Main.player[l].inventory[m].type == 168 || Main.player[l].inventory[m].type == 235 || Main.player[l].inventory[m].type == 2896 || Main.player[l].inventory[m].type == 3547)
										{
											flag3 = true;
										}
										if (Main.player[l].inventory[m].dye > 0 || (Main.player[l].inventory[m].type >= 1107 && Main.player[l].inventory[m].type <= 1120) || (Main.player[l].inventory[m].type >= 3385 && Main.player[l].inventory[m].type <= 3388))
										{
											if (Main.player[l].inventory[m].type >= 3385 && Main.player[l].inventory[m].type <= 3388)
											{
												flag5 = true;
											}
											flag4 = true;
										}
									}
								}
								int num29 = Main.player[l].statLifeMax / 20;
								if (num29 > 5)
								{
									flag = true;
								}
								num28 += num29;
								if (!flag4)
								{
									for (int n = 0; n < 3; n++)
									{
										if (Main.player[l].dye[n] != null && Main.player[l].dye[n].stack > 0 && Main.player[l].dye[n].dye > 0)
										{
											flag4 = true;
										}
									}
								}
							}
						}
						if (!NPC.downedBoss3 && num7 == 0)
						{
							int num30 = NPC.NewNPC(Main.dungeonX * 16 + 8, Main.dungeonY * 16, 37, 0, 0f, 0f, 0f, 0f, 255);
							Main.npc[num30].homeless = false;
							Main.npc[num30].homeTileX = Main.dungeonX;
							Main.npc[num30].homeTileY = Main.dungeonY;
						}
						bool flag6 = false;
						if (Main.rand.Next(40) == 0)
						{
							flag6 = true;
						}
						if (num6 < 1)
						{
							Main.townNPCCanSpawn[22] = true;
						}
						if ((double)num27 > 5000.0 && num2 < 1)
						{
							Main.townNPCCanSpawn[17] = true;
						}
						if (flag && num3 < 1 && num2 > 0)
						{
							Main.townNPCCanSpawn[18] = true;
						}
						if (flag2 && num5 < 1)
						{
							Main.townNPCCanSpawn[19] = true;
						}
						if ((NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3) && num4 < 1)
						{
							Main.townNPCCanSpawn[20] = true;
						}
						if (flag3 && num2 > 0 && num8 < 1)
						{
							Main.townNPCCanSpawn[38] = true;
						}
						if (NPC.savedStylist && num22 < 1)
						{
							Main.townNPCCanSpawn[353] = true;
						}
						if (NPC.savedAngler && num23 < 1)
						{
							Main.townNPCCanSpawn[369] = true;
						}
						if (NPC.downedBoss3 && num9 < 1)
						{
							Main.townNPCCanSpawn[54] = true;
						}
						if (NPC.savedGoblin && num11 < 1)
						{
							Main.townNPCCanSpawn[107] = true;
						}
						if (NPC.savedTaxCollector && num24 < 1)
						{
							Main.townNPCCanSpawn[441] = true;
						}
						if (NPC.savedWizard && num10 < 1)
						{
							Main.townNPCCanSpawn[108] = true;
						}
						if (NPC.savedMech && num12 < 1)
						{
							Main.townNPCCanSpawn[124] = true;
						}
						if (NPC.downedFrost && num13 < 1 && Main.xMas)
						{
							Main.townNPCCanSpawn[142] = true;
						}
						if (NPC.downedMechBossAny && num15 < 1)
						{
							Main.townNPCCanSpawn[178] = true;
						}
						if (flag4 && num16 < 1 && ((NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3) | flag5))
						{
							Main.townNPCCanSpawn[207] = true;
						}
						if (NPC.downedQueenBee && num20 < 1)
						{
							Main.townNPCCanSpawn[228] = true;
						}
						if (NPC.downedPirates && num21 < 1)
						{
							Main.townNPCCanSpawn[229] = true;
						}
						if (num14 < 1 && Main.hardMode)
						{
							Main.townNPCCanSpawn[160] = true;
						}
						if (Main.hardMode && NPC.downedPlantBoss && num18 < 1)
						{
							Main.townNPCCanSpawn[209] = true;
						}
						if (num26 >= 8 && num19 < 1)
						{
							Main.townNPCCanSpawn[227] = true;
						}
						if (flag6 && num17 < 1 && num26 >= 14)
						{
							Main.townNPCCanSpawn[208] = true;
						}
						if (NPC.savedBartender && num25 < 1)
						{
							Main.townNPCCanSpawn[550] = true;
						}
						if (WorldGen.prioritizedTownNPC == 0 && num6 < 1)
						{
							WorldGen.prioritizedTownNPC = 22;
						}
						if (WorldGen.prioritizedTownNPC == 0 && (double)num27 > 5000.0 && num2 < 1)
						{
							WorldGen.prioritizedTownNPC = 17;
						}
						if (((WorldGen.prioritizedTownNPC == 0) & flag) && num3 < 1 && num2 > 0)
						{
							WorldGen.prioritizedTownNPC = 18;
						}
						if (((WorldGen.prioritizedTownNPC == 0) & flag2) && num5 < 1)
						{
							WorldGen.prioritizedTownNPC = 19;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedGoblin && num11 < 1)
						{
							WorldGen.prioritizedTownNPC = 107;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedTaxCollector && num24 < 1)
						{
							WorldGen.prioritizedTownNPC = 441;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedWizard && num10 < 1)
						{
							WorldGen.prioritizedTownNPC = 108;
						}
						if (WorldGen.prioritizedTownNPC == 0 && Main.hardMode && num14 < 1)
						{
							WorldGen.prioritizedTownNPC = 160;
						}
						if (WorldGen.prioritizedTownNPC == 0 && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3) && num4 < 1)
						{
							WorldGen.prioritizedTownNPC = 20;
						}
						if (((WorldGen.prioritizedTownNPC == 0) & flag3) && num2 > 0 && num8 < 1)
						{
							WorldGen.prioritizedTownNPC = 38;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.downedQueenBee && num20 < 1)
						{
							WorldGen.prioritizedTownNPC = 228;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.downedMechBossAny && num15 < 1)
						{
							WorldGen.prioritizedTownNPC = 178;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedMech && num12 < 1)
						{
							WorldGen.prioritizedTownNPC = 124;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedAngler && num23 < 1)
						{
							WorldGen.prioritizedTownNPC = 369;
						}
						if (WorldGen.prioritizedTownNPC == 0 && Main.hardMode && NPC.downedPlantBoss && num18 < 1)
						{
							WorldGen.prioritizedTownNPC = 209;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.downedPirates && num21 < 1)
						{
							WorldGen.prioritizedTownNPC = 229;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.downedBoss3 && num9 < 1)
						{
							WorldGen.prioritizedTownNPC = 54;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedStylist && num22 < 1)
						{
							WorldGen.prioritizedTownNPC = 353;
						}
						if (((WorldGen.prioritizedTownNPC == 0) & flag4) && num16 < 1)
						{
							WorldGen.prioritizedTownNPC = 207;
						}
						if (WorldGen.prioritizedTownNPC == 0 && num26 >= 8 && num19 < 1)
						{
							WorldGen.prioritizedTownNPC = 227;
						}
						if (((WorldGen.prioritizedTownNPC == 0) & flag6) && num26 >= 14 && num17 < 1)
						{
							WorldGen.prioritizedTownNPC = 208;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.downedFrost && num13 < 1 && Main.xMas)
						{
							WorldGen.prioritizedTownNPC = 142;
						}
						if (WorldGen.prioritizedTownNPC == 0 && NPC.savedBartender && num25 < 1)
						{
							WorldGen.prioritizedTownNPC = 550;
						}
						NPCLoader.CanTownNPCSpawn(num26, num27);
					}
				}
			}
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Crude Semiconductor", new int[]
			{
				ModContent.ItemType<Items.Pyrite>(),
				ModContent.ItemType<Items.Galena>()
			}); ;
			RecipeGroup.RegisterGroup("PlanetMod:Semiconductors", group);

			RecipeGroup group2 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Copper Bar", new int[]
			{
				ItemID.CopperBar,
				ItemID.TinBar
			}); ;
			RecipeGroup.RegisterGroup("PlanetMod:Copper", group2);
		}

		//disabled
		//public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		//{
		//	base.ModifyTransformMatrix(ref Transform);
		//}

		//public static void NewSubworld()
		//{
		//	PlanetList.Add(new PlanetSubworld(200, 200));
		//}
	}
}