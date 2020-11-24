using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static PlanetMod.Helpers.Helper;

namespace PlanetMod.Tiles.GemTiles
{
	public class CalciteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.GemItems.CalciteOreItem>();
			AddMapEntry(new Color(60, 60, 70));
			soundType = SoundID.Tink;
			dustType = 268;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D tex = ModContent.GetTexture("PlanetMod/Tiles/GemTiles/CalciteOre_glow");
			DrawSlopedGlowLayer(i, j, tex, GetFluorescenceColor(i, j), spriteBatch);
		}
	}

	public class FluoriteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.GemItems.FluoriteOreItem>();
			AddMapEntry(new Color(60, 60, 70));
			soundType = SoundID.Tink;
			dustType = 268;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D tex = ModContent.GetTexture("PlanetMod/Tiles/GemTiles/FluoriteOre_glow");
			DrawSlopedGlowLayer(i, j, tex, GetFluorescenceColor(i, j), spriteBatch);
		}
	}

	public class AragoniteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.GemItems.AragoniteOreItem>();
			AddMapEntry(new Color(60, 60, 70));
			soundType = SoundID.Tink;
			dustType = 268;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D tex = ModContent.GetTexture("PlanetMod/Tiles/GemTiles/AragoniteOre_glow");
			DrawSlopedGlowLayer(i, j, tex, GetFluorescenceColor(i, j), spriteBatch);
		}
	}

	public class AmethystSolid : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMergeDirt[Type] = false;
			drop = ModContent.ItemType<Items.GemItems.AmethystSolidItem>();//TODO
			AddMapEntry(new Color(30, 120, 120));
			soundType = SoundID.Tink;
			//dustType = 268;
		}
	}
}