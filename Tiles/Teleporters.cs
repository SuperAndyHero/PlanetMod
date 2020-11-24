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
	public class Teleporter : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.Origin = new Point16(1, 7);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new[] {16, 16, 16, 16, 16, 16, 16, 16 };
			TileObjectData.newTile.HookPlaceOverride = new PlacementHook(PostPlace, -1, 0, true);
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.Teleporter };
			dustType = DustID.t_SteampunkMetal;
		}

		public override bool Slope(int i, int j) => false;

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Texture2D texture;
			if (Main.canDrawColorTile(i, j))
			{
				texture = Main.tileAltTexture[Type, (int)tile.color()];
			}
			else
			{
				texture = Main.tileTexture[Type];
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, ((j * 16) + 2) - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
			return false;
		}

		private int PostPlace(int x, int y, int type, int style, int dir)
		{
			StructureHelper.StructureHelper.GenerateStructure("Tiles/MultiTileStructures/Teleporter", new Point16(x, y), PlanetMod.Instance);
			return 0;
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
			Item.NewItem(i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Teleporter>());
		}

		public override bool NewRightClick(int i, int j)
		{
			if (Main.tile[(int)(Main.LocalPlayer.Bottom.X / 16), (int)(Main.LocalPlayer.Bottom.Y / 16)].type == this.Type)
			{
				Main.PlaySound(SoundID.Item6, i * 16, j * 16);
				TeleportHandler.MainTeleport();
			}
			return true;
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameY == 126 && (tile.frameX == 18 || tile.frameX == 36))
			{
				if(Main.GameUpdateCount % 100 > 50)
				{
					Lighting.AddLight(new Vector2(i, j) * 16, 0.35f, 0.30f, 0.0f);
				}
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<Items.Teleporter>();//todo, also check if player is on telepad here too
		}
	}
}