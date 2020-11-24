using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.PlanetMod;
using static PlanetMod.Handlers.ShipBuildingHandler;
using SubworldLibrary;

namespace PlanetMod.Tiles
{
	public class Barrier : ModTile //Filler tile, this tile is frame important, and it's frame x / y is used to store which texture to draw and it's offset
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "PlanetMod/Null";
			return true;
		}
		public override void SetDefaults()//maybe make a base class for this?
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileNoSunLight[Type] = false;
			//Main.tileLighted[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
		}

		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
		}

		//public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		//{
		//	r = 0.2f;
		//	g = 0.2f;
		//	b = 0.2f;
		//}

		public override bool Slope(int i, int j) => false;
		public override bool CanExplode(int i, int j) => false;
		public override bool KillSound(int i, int j) => false;
		public override bool CreateDust(int i, int j, ref int type) => false;
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) => false;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			fail = true;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if ((Main.tile[i, j].frameY > 0 && Main.tile[i, j].frameY <= PlanetMod.shipWallArraySize) || (Main.tile[i, j].frameX > 0 && Main.tile[i, j].frameX <= PlanetMod.shipPartArraySize))
			{
				if (!(Main.npc.Any(index => index.modNPC is Npcs.ShipDummyNpc && (index.modNPC as Npcs.ShipDummyNpc).parent == Main.tile[i, j] && index.active)))
				{
					int index = NPC.NewNPC((i * 16) + 8, (j * 16) + 16, ModContent.NPCType<Npcs.ShipDummyNpc>());
					if(index < Main.maxNPCs)
					{
						(Main.npc[index].modNPC as Npcs.ShipDummyNpc).parent = Main.tile[i, j];
					}
				}
			}
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)//debug drawing
		{
			if (Main.tile[i, j].frameX >= 0 && !Subworld.IsActive<Ship.ShipSubworld>())// to be completely commented out when done
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				Texture2D texture2 = ModContent.GetTexture("PlanetMod/Tiles/BarrierOverlay"); //Overlay
				Main.spriteBatch.Draw(texture2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(0, 0, texture2.Width, texture2.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public class SmallThruster : ModTile //Draws the small thrusters
	{
		public override void SetDefaults()//maybe make a base class for this?
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileNoSunLight[Type] = false;
			Main.tileLighted[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.4f;
			g = 0.4f;
			b = 0.4f;
		}

		public override bool Slope(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if(tile.frameX < 3) tile.frameX++;
			else tile.frameX = 0;
			return false;
		}
		public override bool CanExplode(int i, int j) => false;
		public override bool KillSound(int i, int j) => false;
		public override bool CreateDust(int i, int j, ref int type) => false;
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) => false;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			fail = true;
		}

		//public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		//{

		//}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if(thrusterMode > 0 && thrusterMode < thrusterModesSize)
			{
				frameCounter += 1;//replace 1 with amount
				if ((int)(frameCounter * 0.1) >= thrusterFrameCount[thrusterMode])
				{
					frameCounter = 0;
				}
				frame = (int)(frameCounter * 0.1);
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)//debug drawing
		{	
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			Tile tile = Main.tile[i, j];

			Texture2D texture;
			if (Main.canDrawColorTile(i, j))	texture = Main.tileAltTexture[Type, (int)tile.color()];
			else    texture = Main.tileTexture[Type];

			Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X + 8, j * 16 - (int)Main.screenPosition.Y + 8) + zero, texture.Frame(), Color.White, tile.frameX * 1.57f, new Vector2(texture.Width / 2, texture.Height / 2 + 2), 1f, SpriteEffects.None, 0f);


			if (thrusterMode > 0 && thrusterMode < thrusterModesSize)
			{
				Texture2D flameTex = GetThrusterTexture();
				int texFrame = Main.tileFrame[Type] * flameTex.Height / thrusterFrameCount[thrusterMode];
				Rectangle texRect = new Rectangle(0, texFrame, flameTex.Width, flameTex.Height / thrusterFrameCount[thrusterMode]);
				Main.spriteBatch.Draw(flameTex, new Vector2(i * 16 - (int)Main.screenPosition.X + 8, j * 16 - (int)Main.screenPosition.Y + 8) + zero, texRect, Color.White, tile.frameX * 1.57f, new Vector2(flameTex.Width / 2, -4), 1f, SpriteEffects.None, 0f);
			}



			//if (true)// to be completely commented out when done
			//{
			//	Texture2D texture2 = ModContent.GetTexture("PlanetMod/Tiles/BarrierWallOverlay"); //Overlay
			//	Main.spriteBatch.Draw(texture2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(0, 0, texture2.Width, texture2.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			//}
		}
	}

	//public class BarrierWall : ModWall//this wall when placed behind the barrier tile draws the textures
	//{
	//	public override bool Autoload(ref string name, ref string texture)
	//	{
	//		texture = "PlanetMod/Tiles/Barrier";
	//		return true;
	//	}
	//	public override bool CanExplode(int i, int j)
	//	{
	//		return false;
	//	}
	//	public override bool KillSound(int i, int j)
	//	{
	//		return false;
	//	}
	//	public override bool CreateDust(int i, int j, ref int type)
	//	{
	//		return false;
	//	}
	//	public override void KillWall(int i, int j, ref bool fail)
	//	{
	//		fail = true;
	//	}

	//	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)//texture drawing, may move back to tile
	//	{
	//		//Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
	//		//int frameX = Main.tile[i, j].frameX;
	//		//int frameY = Main.tile[i, j].frameY;

	//		//if (frameX > 0 && frameX <= PlanetMod.shipPartArrayMax)
	//		//{
	//		//	Main.spriteBatch.Draw(PlanetMod.shipPartArray[frameX - 1], new Vector2(i * 16 - (int)Main.screenPosition.X, (j + frameY) * 16 - (int)Main.screenPosition.Y) + zero, PlanetMod.shipPartArray[frameX - 1].Frame(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
	//		//}

	//		return false;
	//	}

	//	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)//debug drawing
	//	{
	//		if (true)
	//		{
	//			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
	//			Texture2D texture2 = ModContent.GetTexture("PlanetMod/Tiles/BarrierWallOverlay"); //Overlay
	//			Main.spriteBatch.Draw(texture2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(0, 0, texture2.Width, texture2.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
	//		}
	//	}
	//}
}