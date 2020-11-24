using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace PlanetMod.Helpers
{
	public static class Helper //helpers are just for methods and should not store any values
	{
        public static Color GetFluorescenceColor(int i, int j)
        {
            Color lightColor = Lighting.GetColor(i, j);
            int fluorescenceLevel = lightColor.B - ((lightColor.R / 2) + (lightColor.G / 2));
            return new Color(fluorescenceLevel, fluorescenceLevel, fluorescenceLevel, fluorescenceLevel);
        }

		public static void DrawSlopedGlowLayer(int i, int j, Texture2D glowTexture, Color glowColor, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 correction = Main.drawToScreen ? Vector2.Zero : Vector2.One * Main.offScreenRange; //better correction for lighting mode

			int height = tile.frameY == 36 ? 18 : 16;
			if (tile.slope() == 0 && !tile.halfBrick())
			{
				spriteBatch.Draw(glowTexture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + correction, new Rectangle(tile.frameX, tile.frameY, 16, height), glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else if (tile.halfBrick())
			{
				spriteBatch.Draw(glowTexture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 10) + correction, new Rectangle(tile.frameX, tile.frameY + 10, 16, 6), glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				byte b3 = tile.slope();
				for (int num226 = 0; num226 < 8; num226++)
				{
					int num227 = num226 << 1;
					Rectangle value5 = new Rectangle(tile.frameX, tile.frameY + num226 * 2, num227, 2);
					int num228 = 0;
					switch (b3)
					{
						case 2:
							value5.X = 16 - num227;
							num228 = 16 - num227;
							break;
						case 3:
							value5.Width = 16 - num227;
							break;
						case 4:
							value5.Width = 14 - num227;
							value5.X = num227 + 2;
							num228 = num227 + 2;
							break;
					}
					spriteBatch.Draw(glowTexture, new Vector2(i * 16 - (int)Main.screenPosition.X + (float)num228, j * 16 - (int)Main.screenPosition.Y + num226 * 2) + correction, value5, glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
		}

		public static void DrawGlowLayer(int i, int j, Texture2D glowTexture, Color glowColor, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 correction = Main.drawToScreen ? Vector2.Zero : Vector2.One * Main.offScreenRange; //better correction for lighting mode
			spriteBatch.Draw(glowTexture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + correction, new Rectangle(tile.frameX, tile.frameY, 16, 16), glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public static void DrawGlowLayer(int i, int j, int frameX, int frameY, Texture2D glowTexture, Color glowColor, SpriteBatch spriteBatch)//also works for custom drawing, not just glow layers
		{
			Vector2 correction = Main.drawToScreen ? Vector2.Zero : Vector2.One * Main.offScreenRange; //better correction for lighting mode
			spriteBatch.Draw(glowTexture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + correction, new Rectangle(frameX, frameY, 16, 16), glowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public static Color GetBrighterColor(Color colorA, Color colorB)
		{
			return new Color(Math.Max(colorA.R, colorB.R), Math.Max(colorA.G, colorB.G), Math.Max(colorA.B, colorB.B)); ;
		}
	}
}