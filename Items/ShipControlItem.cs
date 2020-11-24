using PlanetMod.Ship;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PlanetMod.Items
{
	public class ShipControlItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ship Control Item");
			Tooltip.SetDefault("Unbreakable");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 5;
			item.useTime = 5;
			item.useStyle = 1;
			item.value = 0;
			//item.createTile = ModContent.TileType<Tiles.SmallConsole>();
			//item.createTile = ModContent.TileType<Tiles.GemTiles.AmethystSolid>();
			item.createTile = ModContent.TileType<Tiles.Projector>();
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (player.controlDown)
				{
					if (player.controlSmart)
					{
						ShipWorld.shipViewRotation -= 0.05f;
					}
					else
					{
						ShipWorld.shipViewRotation -= 0.005f;
					}
				}
				if (player.controlUp)
				{
					if (player.controlSmart)
					{
						ShipWorld.shipViewRotation += 0.05f;
					}
					else
					{
						ShipWorld.shipViewRotation += 0.005f;
					}
				}
				//ShipWorld.thrusterMode = 2;
				//Main.NewText("rotation amount " + PlanetMod.shipViewRotationAmount);
				return false;//this makes it update every tick instead of every swing
			}
			else
			{
				Main.NewText("Frame X: " + Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameX + " Frame Y: " + Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameY);

				/*
				//Main.NewText(ShipWorld.ShipAreaClear(new Point16((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)), 0, true));

				//checks tile frame
				//debug? //Tile selectedTile = Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)];
				//int npcs = 0;
				//for (int i = 0; i < Main.maxNPCs; i++)
				//{
				//	if (Main.npc[i].active)
				//	{
				//		Main.NewText(Main.npc[i].position);
				//		npcs++;
				//	}
				//}
				//Main.NewText("npcs: " + npcs);
				//Main.NewText("player pos: " + player.position);
				debug shit */
			}
			return base.CanUseItem(player);
		}
	}
}
