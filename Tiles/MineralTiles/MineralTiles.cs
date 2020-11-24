using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static PlanetMod.Helpers.Helper;

namespace PlanetMod.Tiles.MineralTiles
{
	public class Basalt : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<Items.MineralItems.BasaltItem>();
			AddMapEntry(new Color(60, 60, 70));
			soundType = SoundID.Tink;
			dustType = 268;
		}
	}
}