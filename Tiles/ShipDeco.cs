using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using SubworldLibrary;
using PlanetMod.Handlers;
using static PlanetMod.Planets.PlanetWorld;
using PlanetMod.Helpers;

namespace PlanetMod.Tiles
{
	public class SmallConsole : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;


			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);

			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };

			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
			TileObjectData.newTile.StyleMultiplier = 2; //same as above
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
			TileObjectData.addAlternate(1); //facing right will use the second texture style

			TileObjectData.addTile(Type);


			disableSmartCursor = true;
			dustType = DustID.t_SteampunkMetal;
		}

		public override bool HasSmartInteract() => true;

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<PlasmaEntity>().Kill(i, j);
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.SmallConsoleItem>());
		}

		public override bool NewRightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void HitWire(int i, int j)//may wanna make the file frame stuff to rightclick since this may not want to receive wire pulses
		{
			const int tileWidth = 2;
			const int tileHeight = 2;
			Tile tile = Main.tile[i, j];
			int topX = i - tile.frameX / 18 % tileWidth;
			int topY = j - tile.frameY / 18 % tileHeight;
			short frameAdjustment = (short)(tile.frameY > 18 ? -38 : 38);

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
						Main.tile[l, m].frameY += frameAdjustment;
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(topX, topY);
				Wiring.SkipWire(topX, topY + 1);
				Wiring.SkipWire(topX + 1, topY);
				Wiring.SkipWire(topX + 1, topY + 1);
			}
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if(Main.tile[i, j].frameY < 38)
			{
				Texture2D glowTexture = ModContent.GetTexture("PlanetMod/Tiles/SmallConsole_Glow");
				Helper.DrawGlowLayer(i, j, glowTexture, Helper.GetBrighterColor(new Color(100, 100, 100), Lighting.GetColor(i, j)), spriteBatch);
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<Items.SmallConsoleItem>();
		}
	}
}