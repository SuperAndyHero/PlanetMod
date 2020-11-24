using Terraria.ID;
using Terraria.ModLoader;

namespace PlanetMod.Items.GemItems
{
	public class CalciteOreItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Calcite Ore");
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
			item.createTile = ModContent.TileType<Tiles.GemTiles.CalciteOre>();
		}
	}
	public class FluoriteOreItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fluorite Ore");
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
			item.createTile = ModContent.TileType<Tiles.GemTiles.FluoriteOre>();
		}
	}

	public class AragoniteOreItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aragonite Ore");
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
			item.createTile = ModContent.TileType<Tiles.GemTiles.AragoniteOre>();
		}
	}

	public class AmethystSolidItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Tile");
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
			item.createTile = ModContent.TileType<Tiles.GemTiles.AmethystSolid>();
		}
	}
}
