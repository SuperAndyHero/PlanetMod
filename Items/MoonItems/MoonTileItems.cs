using Terraria.ID;
using Terraria.ModLoader;

namespace PlanetMod.Items.MoonItems
{
	public class MoonTurfItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Regolith");
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
			item.createTile = ModContent.TileType<Tiles.MoonTiles.MoonTurf>();
		}
	}

	public class MoonRockItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Rock");
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
			item.createTile = ModContent.TileType<Tiles.MoonTiles.MoonRock>();
		}
	}
}
