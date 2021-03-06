using Terraria.ID;
using Terraria.ModLoader;

namespace PlanetMod.Items.MineralItems
{
	public class BasaltItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basalt");
			//Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.MineralTiles.Basalt>();
		}
	}
}
