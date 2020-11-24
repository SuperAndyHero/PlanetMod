using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;

namespace PlanetMod.Items
{
	public class BarrierItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Barrier Item");
			Tooltip.SetDefault("Unbreakable");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 5;
			item.useTime = 5;
			item.useStyle = 1;
			item.value = 0;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		private int cooldown = 0;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (cooldown >= 4)
				{
					if (player.controlDown && item.value > 0)
					{
						item.value--;
					}
					if (player.controlUp && item.value < 64)
					{
						item.value++;
					}
					Main.NewText("Selected Frame " + item.value);
					cooldown = 0;
				}
				cooldown++;
				//return false;
			}
			else
			{
				//if (player.controlTorch && player.controlUp && player.controlUp)//todo: fix this, and setup npc dummies
				//{
				//	//WorldGen.KillWall((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
				//	//Main.NewText("removed wall");
				//}
				//else if (player.controlUp && player.controlUp)
				//{
				//	//WorldGen.PlaceWall((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16), ModContent.WallType<Tiles.BarrierWall>());
				//	//Main.NewText("placed wall");
				//}
				//else if (player.controlTorch)
				//{
				//	//WorldGen.KillTile((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
				//}
				if (player.controlDown && player.controlUp)
				{
					Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameY = short.MaxValue;
					WorldGen.TileFrame((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
					Main.NewText("Frame Y: " + Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameY);
				}
				else if (player.controlDown)
				{
					Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameY = (short)item.value;
					WorldGen.TileFrame((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
					Main.NewText("Frame Y: " + Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameY);
				}
				else if (player.controlUp)
				{
					Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameX = (short)item.value;
					WorldGen.TileFrame((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
					Main.NewText("Frame X: " + Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].frameX);
				}
				else
				{
					//WorldGen.PlaceTile((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16), ModContent.TileType<Tiles.SmallThruster>(), false, false);

					WorldGen.PlaceTile((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16), ModContent.TileType<Tiles.Barrier>(), false, false);
				}
			}
			return base.CanUseItem(player);
		}
	}
}
