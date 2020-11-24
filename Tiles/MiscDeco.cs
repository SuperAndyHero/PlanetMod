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
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace PlanetMod.Tiles
{
	public class TvMedium : AnimScreen
	{
		public TvMedium() : base(ModContent.ItemType<Items.TvMediumItem>(), 3, 2, ModContent.GetTexture("PlanetMod/Tiles/TvMedium")) { }
	}

	public class TvLarge : AnimScreen
	{
		public TvLarge() : base(ModContent.ItemType<Items.TvLargeItem>(), 4, 3, ModContent.GetTexture("PlanetMod/Tiles/TvLarge")) { }
	}

	public class Projector : AnimScreen
	{
		private static void DrawProjectorBeam(int i, int j, SpriteBatch spritebatch)
		{
			spritebatch.Draw(ModContent.GetTexture("PlanetMod/Tiles/beam2"), new Vector2((i + 5) * 16, (j + 10) * 16) - Main.screenPosition, Color.White);
		}
		public Projector() : base(ModContent.ItemType<Items.ShipControlItem>(), 2, 2, ModContent.GetTexture("PlanetMod/Tiles/Projector"), 18, 18, DrawProjectorBeam) { }
	}

	public abstract class AnimScreen : ModTile
	{
		private readonly int ItemType;
		private readonly int TileWidth;
		private readonly int TileHeight;
		private readonly int FrameHeight;
		private readonly int FrameWidth;
		//private readonly int FramePadding;//removed because it would break
		private readonly Texture2D TileTexture;

		public delegate void DrawDelegate(int i, int j, SpriteBatch sb);//made public so it can be drawn via TE, if another layer is needed per-tile a copy of this can be made which is private and non-static
		public DrawDelegate ExtraDraw = null;//drawn once per TE, with blendstate additive

		protected AnimScreen(int item, int width, int height, Texture2D tex, int frameHeight = 18, int frameWidth = 18, Action<int, int, SpriteBatch> extraDraw = null)
		{
			ItemType = item;
			TileWidth = width;
			TileHeight = height;
			TileTexture = tex;//always needs padding, even for a 1x1
			FrameHeight = frameHeight;
			FrameWidth = frameWidth;
			if(extraDraw != null)
			{
				ExtraDraw = new DrawDelegate(extraDraw);
			}
			//int Dust = dust; //TODO
		}

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			//TileID.Sets.HasOutlines[Type] = true;


			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = TileWidth;//variable
			TileObjectData.newTile.Height = TileHeight;//variable

			TileObjectData.newTile.Origin = new Point16(TileWidth / 2, 0);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);//maybe variable

			TileObjectData.newTile.StyleHorizontal = true;
			//TileObjectData.newTile.CoordinatePadding = FramePadding;

			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TvEntity>().Hook_AfterPlacement, -1, 0, true);

			TileObjectData.newTile.CoordinateHeights = Enumerable.Repeat(16, TileHeight).ToArray();
			TileObjectData.addTile(Type);


			disableSmartCursor = true;
			//dustType = DustID.t_SteampunkMetal;//TODO
		}

		public override bool HasSmartInteract() => true;

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)//TODO : see if this drops in the right place
		{
			ModContent.GetInstance<TvEntity>().Kill(i, j);
			Item.NewItem(i * 16, j * 16, 64, 32, ItemType);
		}

		public override bool NewRightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);//TODO tv static sound
			HitWire(i, j);
			return true;
		}

		private int Height => TileHeight * FrameHeight;

		public override void HitWire(int i, int j)//may change this to hitwire shifts the X position so it keeps the current channel
		{

			Tile tile = Main.tile[i, j];
			int topX = i - tile.frameX / FrameWidth % TileWidth;
			int topY = j - tile.frameY / FrameHeight % TileHeight;
			short frameAdjustment = (short)(tile.frameY > (((PlanetMod.animArraySize - 1) * Height) + Height - FrameHeight) ? -(((PlanetMod.animArraySize - 1) * Height) + Height) : Height);// ? (this goes off the size of the array, but could be changed to be more stable and always go to default without the fixed array size, to correct for being outside channel range) :

			for (int l = topX; l < topX + TileWidth; l++)
			{
				for (int m = topY; m < topY + TileHeight; m++)
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
			if (Wiring.running)//TODO
			{
				for (int f = 0; f < TileWidth; f++)
				{
					for (int g = 0; g < TileHeight; g++)//TODO : these may run for one less iteration than they should.
					{
						Wiring.SkipWire(topX + f, topY + g);
					}
				}
			}
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];

			if (ExtraDraw != null && tile.frameX == 0 && tile.frameY % Height == 0 && tile.frameY != 0)//only if there is a method to draw, and the tile is top left
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

				ExtraDraw.Invoke(i, j, spriteBatch);

				spriteBatch.End();//reset to default 
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			}

			Helper.DrawGlowLayer(i, j, tile.frameX, tile.frameY % Height, TileTexture, Lighting.GetColor(i, j), spriteBatch);

			return false;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ItemType;
		}
	}

	public class TvEntity : ModTileEntity
	{
		//custom thing for getting the tile entity's position easily from elsewhere, no clue where this is from
		internal Point16 EntityPosition()
		{
			return new Point16(Position.X, Position.Y);
		}

		private Rectangle FrameSize;//x and y of the rect are just a offset
		internal Rectangle GetFrameSize()
		{
			if (FrameSize == Rectangle.Empty)
			{
				FrameSize = SetFrameSize();
			}
			return FrameSize;
		}

		private int TileFrameHeight;
		internal int GetTileFrameHeight()
		{
			if (TileFrameHeight == default)
			{
				TileFrameHeight = SetTileFrameHeight();
			}
			return TileFrameHeight;
		}

		private int[] ValidTvTiles => new int[] { ModContent.TileType<TvMedium>(), ModContent.TileType<TvLarge>(), ModContent.TileType<Projector>() };//see, properties are much easier
		private int SetTileFrameHeight()//it would be possible to piggyback off SetFrameSize() for this, but I decided not to.
		{
			int h = 0;
			foreach (int tileType in ValidTvTiles)//because switch statments have to use constants...
			{
				if (tileType == Main.tile[Position.X, Position.Y].type)//maybe find a way to access the tile's variables and use those
				{
					switch (h)
					{
						case 0:
							return 36;//TvMedium
						case 1:
							return 54;//TvLarge
						case 2:
							return 36;//Projector
					};
					break;
				}
				h++;
			}
			return 18;
		}
		private Rectangle SetFrameSize()
		{
			int h = 0;
			foreach (int tileType in ValidTvTiles)//because switch statments have to use constants...
			{
				if (tileType == Main.tile[Position.X, Position.Y].type)
				{
					switch (h)
					{
						case 0:
							return new Rectangle(2, 2, 44, 24);//TvMedium
						case 1:
							return new Rectangle(2, 10, 60, 32);//TvLarge
						case 2:
							return new Rectangle(-112, -160, 256, 128); ;//Projector
					};
					break;
				}
				h++;
			}
			return new Rectangle(0, 0, 16, 16);
		}

		public override bool ValidTile(int i, int j)
		{
			//Old Comment:  Main.NewText("ValidTile" + i + j);
			Tile tile = Main.tile[i, j];
			return tile.active() && tile.frameX == 0 && (tile.frameY % GetTileFrameHeight() == 0 || tile.frameY == 0) && PlanetMod.validPlasmaTiles.Contains(tile.type);
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