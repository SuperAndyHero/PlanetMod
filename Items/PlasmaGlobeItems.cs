using PlanetMod.Ship;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PlanetMod.Items
{
	public class PlasmaGlobeSmallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Globe Small");
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
			item.value = 2000;
			item.createTile = ModContent.TileType<Tiles.PlasmaGlobeSmall>();
		}
	}

	public class PlasmaGlobeMediumItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Globe Medium");
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
			item.value = 2000;
			item.createTile = ModContent.TileType<Tiles.PlasmaGlobeMedium>();
		}
	}

	public class PlasmaGlobeLargeItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Globe Large");
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
			item.value = 2000;
			item.createTile = ModContent.TileType<Tiles.PlasmaGlobeLarge>();
		}
	}
}
