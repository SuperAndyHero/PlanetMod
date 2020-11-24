using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using System.Globalization;
using PlanetMod.Planets;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;
using System;
using static PlanetMod.Planets.PlanetWorld;

namespace PlanetMod.Handlers
{
    public static class ShipBuildingHandler
    {
		//SYNC: the 6 below variables may need to be synced
		public static ushort thrusterMode; //For per side modes: could be easily changed to an array, with the index being the rotation (aka tile frame
		public static ushort thrusterStyle; //For per block styles: can be replaced with just using the tile frameY for changing it per block

		public static Point16 selectedDoorPosition;
		public static int selectedDoorDirection; //west, east, north, south

		public static Point16 selectedShipStructPosition;
		public static ushort selectedShipStructType;

        #region defining info for structures
        public const int ShipStructuresMax = 3;//each structure needs a size and door offset for each side below (no door: is -1), and a overlay texture
		public enum ShipID : ushort
		{
			Deck = 0,
			Normal2Way = 1,
			Normal4Way = 2
		}

		public static Point16[] ShipStructureSizes = new Point16[ShipStructuresMax]//needed for every structure
		{
			new Point16(19, 16),//deck
			new Point16(24, 12),//Normal 2-way
			new Point16(24, 12)//Normal 4-way
		};//width / height

		public static int[,] ShipDoorPositions = new int[ShipStructuresMax, 4]//needed for every structure
		{ //east, west, south, north (directions swapped because checked dimension is opposite door direction (west door: check next room east side) )
			{-1, 8, -1, -1},//deck
			{6, 6, -1, -1},//Normal 2-way
			{6, 6, 10, 10}//Normal 4-way
		};
		#endregion

		#region clearing/removal
		public static void RemoveSelectedDoor()//removes door at selected position
		{
			int width = 2;
			int height = 4;

			if (selectedDoorDirection >= 2)
			{
				width = 4;
				height = 2;
			}

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Tile targetTile = Main.tile[selectedDoorPosition.X + i, selectedDoorPosition.Y + j];
					targetTile.active(false);
				}
			}
		}

		public static void ClearSelectedDoor()//clears just selected door pos, and direction
		{
			selectedDoorPosition = Point16.Zero;
			selectedDoorDirection = 0;
		}

		public static void ClearSelectedStruct()//clears just selected ship pos & type.
		{
			selectedShipStructPosition = Point16.Zero;
			selectedShipStructType = 0;
		}

		public static void ClearSelectedAll()//clears selected ship pos, type, and selected door pos, and direction
		{
			selectedShipStructPosition = Point16.Zero;
			selectedShipStructType = 0;
			selectedDoorPosition = Point16.Zero;
			selectedDoorDirection = 0;
		}
		#endregion

		public static void CreateStructure()//places struct, removes selected door, then checks and removes the next door in range of old door pos, then clears all selected
		{
			if (selectedShipStructPosition.X > 0 || selectedShipStructPosition.Y > 0)
			{
				PlaceShipStructure(selectedShipStructPosition, selectedShipStructType);
				RemoveSelectedDoor();
				FindDoor(selectedDoorPosition, Vector2.Zero);
				RemoveSelectedDoor();
				ClearSelectedAll();
			}
		}

		public static void PlaceShipStructure(Point16 location, ushort structureID, bool centered = false)//places structure and updates it, does not check area
		{
			if (centered)
			{
				location -= new Point16(ShipStructureSizes[structureID].X / 2, ShipStructureSizes[structureID].Y / 2);
			}

			if (structureID < ShipStructuresMax)
			{
				StructureHelper.StructureHelper.GenerateStructure("Ship/ShipStructures/ShipStructure_" + structureID, location, PlanetMod.Instance);
			}

			for (int i = 0; i < ShipStructureSizes[structureID].X; i++)
			{
				for (int j = 0; j < ShipStructureSizes[structureID].Y; j++)
				{
					WorldGen.TileFrame(location.X + i, location.Y + j);
				}
			}
		}

		private static bool IsShipAreaClear(Point16 location, ushort structureID, bool centered = false)//used before PlaceShipStructure()
		{
			if (centered)
			{
				location -= new Point16(ShipStructureSizes[structureID].X / 2, ShipStructureSizes[structureID].Y / 2);
			}

			for (int i = 0; i < ShipStructureSizes[structureID].X; i++) //fills subworld with tile, this is here to stop escaping (example: RoD)
			{
				for (int j = 0; j < ShipStructureSizes[structureID].Y; j++) //fills subworld with tile, this is here to stop escaping (example: RoD)
				{
					//Dust.NewDustPerfect(new Vector2(location.X + i + 0.5f, location.Y + j + 0.5f) * 16, DustID.Fire);
					Tile tile = Main.tile[location.X + i, location.Y + j];
					if (tile.active() && (tile.type != ModContent.TileType<Tiles.Barrier>() || (tile.type == ModContent.TileType<Tiles.Barrier>() && (tile.frameX != short.MaxValue || tile.frameY != 0))))
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool FindDoor(Point16 location, Vector2 playerCenter)//checks for the first door in an area around the given location
		{
			if (Main.tile[location.X, location.Y].frameY == -1 || true)
			{
				for (int i = -4; i < 5; i++)
				{
					for (int j = -4; j < 5; j++)
					{
						//Dust.NewDustPerfect(new Vector2(location.X + i + 0.5f, location.Y + j + 0.5f) * 16, DustID.PinkFlame);

						Tile tile = Main.tile[location.X + i, location.Y + j];
						if (tile.active() && tile.type == ModContent.TileType<Tiles.Barrier>() && tile.frameY == short.MaxValue)
						{
							if (tile.frameX == 4)
							{
								selectedDoorPosition = new Point16(location.X + i, location.Y + j);
								selectedDoorDirection = (playerCenter.X - ((location.X + i) * 16) < 0) ? 1 : 0;
								//Main.NewText("Position: " + selectedDoorPosition);
								//Main.NewText("Direction: " + selectedDoorDirection);
								return true;
							}
							else if (tile.frameX == 5)
							{
								selectedDoorPosition = new Point16(location.X + i, location.Y + j);
								selectedDoorDirection = (playerCenter.Y - ((location.Y + j) * 16) < 0) ? 3 : 2;
								//Main.NewText("Position: " + selectedDoorPosition);
								//Main.NewText("Direction: " + selectedDoorDirection);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static bool StructureMark(ushort structureID)//sets selected struct, checks if the area offset from the selected door is empty, if isnt empty then clears selected struct
		{
			if ((selectedDoorPosition.X > 0 || selectedDoorPosition.Y > 0) && (structureID >= 0 && structureID < ShipStructuresMax))
			{
				int xOffset = selectedDoorDirection <= 1 ? ShipStructureSizes[structureID].X : ShipDoorPositions[structureID, selectedDoorDirection];
				int yOffset = selectedDoorDirection <= 1 ? ShipDoorPositions[structureID, selectedDoorDirection] : ShipStructureSizes[structureID].Y;

				if (yOffset == -1 || xOffset == -1)
				{
					//Main.NewText("No valid door on that side of selected structure");//TODO: show a mini display above here of the structure
					return false;
				}

				if (selectedDoorDirection == 1)//these are offsets for the width of the door
				{
					xOffset = -2;//hortizontal offset
				}
				else if (selectedDoorDirection == 3)
				{
					yOffset = -2;//vertical offset
				}

				selectedShipStructType = structureID;
				selectedShipStructPosition = new Point16(selectedDoorPosition.X - xOffset, selectedDoorPosition.Y - yOffset);

				//Main.NewText("offset: " + ShipDoorPositions[structureID, selectedDoorDirection]);

				if (IsShipAreaClear(selectedShipStructPosition, structureID))
				{
					return true;
				}
				else
				{
					Main.NewText("Not enough Space");
					ClearSelectedStruct();//TODO: instead of clear this do other thing to show red (static bool?)
					return false;
				}
			}
			Main.NewText("error: ID is outside accepted range, or called with no valid door");
			ClearSelectedStruct();//just in case since drawing doesn't have a check for if its over max, never had it happen but just in case.
			return false;
		}
	}
}