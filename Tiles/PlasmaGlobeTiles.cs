using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using SubworldLibrary;
using PlanetMod.Handlers;
using static PlanetMod.Planets.PlanetWorld;

namespace PlanetMod.Tiles
{
	public class PlasmaGlobeSmall : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PlasmaEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			//TileObjectData.newTile.HookPlaceOverride = new PlacementHook(PostPlace, -1, 0, true);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			dustType = DustID.t_SteampunkMetal;//TODO
		}

		//public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		//{
		//	Tile tile = Main.tile[i, j];
		//	Texture2D texture;
		//	if (Main.canDrawColorTile(i, j))
		//	{
		//		texture = Main.tileAltTexture[Type, (int)tile.color()];
		//	}
		//	else
		//	{
		//		texture = Main.tileTexture[Type];
		//	}
		//	Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		//	if (Main.drawToScreen)
		//	{
		//		zero = Vector2.Zero;
		//	}
		//	Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, ((j * 16) + 2) - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
		//	return false;
		//}

		public override bool HasSmartInteract()
		{
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<PlasmaEntity>().Kill(i, j);
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.PlasmaGlobeSmallItem>());
		}

		public override bool NewRightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			const int tileWidth = 2;
			const int tileHeight = 3;
			Tile tile = Main.tile[i, j];
			int topX = i - tile.frameX / 18 % tileWidth;//2 = width
			int topY = j - tile.frameY / 18 % tileHeight;//3 = height
			short frameAdjustment = (short)(tile.frameX > 18 ? -36 : 36);

			for (int l = topX; l < topX + tileWidth; l++)
			{
				for (int m = topY; m < topY + tileHeight; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						Main.tile[l, m].frameX += frameAdjustment;
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(topX, topY);
				Wiring.SkipWire(topX, topY + 1);
				Wiring.SkipWire(topX, topY + 2);
				Wiring.SkipWire(topX + 1, topY);
				Wiring.SkipWire(topX + 1, topY + 1);
				Wiring.SkipWire(topX + 1, topY + 2);
			}
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<Items.PlasmaGlobeSmallItem>();
		}
	}

	public class PlasmaGlobeMedium : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.Origin = new Point16(1, 4);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PlasmaEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 20 };
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			dustType = DustID.t_SteampunkMetal;//TODO
		}

		public override bool HasSmartInteract()
		{
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<PlasmaEntity>().Kill(i, j);
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.PlasmaGlobeMediumItem>());
		}

		public override bool NewRightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			const int tileWidth = 3;
			const int tileHeight = 5;
			Tile tile = Main.tile[i, j];
			int topX = i - tile.frameX / 18 % tileWidth;
			int topY = j - tile.frameY / 18 % tileHeight;
			short frameAdjustment = (short)(tile.frameX > 36 ? -54 : 54);

			for (int l = topX; l < topX + tileWidth; l++)
			{
				for (int m = topY; m < topY + tileHeight; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						Main.tile[l, m].frameX += frameAdjustment;
					}
				}
			}
			if (Wiring.running)//-=-=-=======================================================
			{
				Wiring.SkipWire(topX, topY);
				Wiring.SkipWire(topX, topY + 1);
				Wiring.SkipWire(topX, topY + 2);
				Wiring.SkipWire(topX + 1, topY);
				Wiring.SkipWire(topX + 1, topY + 1);
				Wiring.SkipWire(topX + 1, topY + 2);
			}
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<Items.PlasmaGlobeMediumItem>();
		}
	}

	public class PlasmaGlobeLarge : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 7;
			TileObjectData.newTile.Origin = new Point16(2, 6);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PlasmaEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 20 };
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			dustType = DustID.t_SteampunkMetal;//TODO
		}

		public override bool HasSmartInteract()
		{
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<PlasmaEntity>().Kill(i, j);
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.PlasmaGlobeLargeItem>());
		}

		public override bool NewRightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			const int tileWidth = 4;
			const int tileHeight = 7;
			Tile tile = Main.tile[i, j];
			int topX = i - tile.frameX / 18 % tileWidth;//2 = width
			int topY = j - tile.frameY / 18 % tileHeight;//3 = height
			short frameAdjustment = (short)(tile.frameX > 54 ? -72 : 72);

			for (int l = topX; l < topX + tileWidth; l++)
			{
				for (int m = topY; m < topY + tileHeight; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						Main.tile[l, m].frameX += frameAdjustment;
					}
				}
			}
			if (Wiring.running)//-=-=-=======================================================
			{
				Wiring.SkipWire(topX, topY);
				Wiring.SkipWire(topX, topY + 1);
				Wiring.SkipWire(topX, topY + 2);
				Wiring.SkipWire(topX + 1, topY);
				Wiring.SkipWire(topX + 1, topY + 1);
				Wiring.SkipWire(topX + 1, topY + 2);
			}
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<Items.PlasmaGlobeLargeItem>();
		}
	}

	public class PlasmaEntity : ModTileEntity
	{
		public PlasmaGlobeInstance PlasmaInstance = new PlasmaGlobeInstance();

		//custom thing for getting the tile entity's position easily from elsewhere, no clue where this is from
		internal Point16 EntityPosition()
		{
			return new Point16(Position.X, Position.Y);
		}

		public override bool ValidTile(int i, int j)
		{
			//Old Comment:  Main.NewText("ValidTile" + i + j);
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.frameX == 0 && tile.frameY == 0 && PlanetMod.validPlasmaTiles.Contains(tile.type);
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			//Old Comment:  Main.NewText("place" + Position.X + Position.Y);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
	}
}