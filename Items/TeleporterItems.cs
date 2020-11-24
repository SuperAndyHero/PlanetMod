using PlanetMod.Ship;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PlanetMod.Items
{
	public class Teleporter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Teleporter Item");
			Tooltip.SetDefault("mk1");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 64;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 2000;//TODO
			item.createTile = ModContent.TileType<Tiles.Teleporter>();
			//item.createTile = TileID.Stone;
		}
	}
}
