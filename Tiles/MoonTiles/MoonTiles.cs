using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PlanetMod.Tiles.MoonTiles
{
	public class MoonTurf : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.MoonItems.MoonTurfItem>();
			AddMapEntry(new Color(200, 200, 200));
            soundType = SoundID.Dig;
            dustType = 262;
        }
	}

	public class MoonRock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.MoonItems.MoonRockItem>();
			AddMapEntry(new Color(180, 180, 180));
			soundType = SoundID.Tink;
			dustType = 268;

			Main.tileLighted[Type] = true;
		}
	}
}