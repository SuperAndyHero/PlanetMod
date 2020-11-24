using PlanetMod.Ship;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.Handlers.ShipBuildingHandler;

namespace PlanetMod.Items
{
	public class Wrench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrench");
			Tooltip.SetDefault("Used to add modules onto the ship \n+5 Range");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.useTurn = true;
			item.autoReuse = false;
			item.useAnimation = 12;
			item.useTime = 12;
			item.useStyle = 1;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		private bool ChangeStructureType(Player player, PlanetModPlayer modPlayer)
		{
			modPlayer.ActivatePlaceGui();
			if (player.controlDown || player.controlUp)//structure select
			{
				if (player.controlDown && modPlayer.shipPlaceType > 0)
				{
					modPlayer.shipPlaceType--;
				}
				if (player.controlUp && modPlayer.shipPlaceType < ShipStructuresMax - 1)
				{
					modPlayer.shipPlaceType++;
				}
				return true;
			}
			return false;
		}

		private void ClearAll(PlanetModPlayer modPlayer)
		{
			ClearSelectedDoor();
			ClearSelectedStruct();
			modPlayer.oldDoorPos = Point16.Zero;
			modPlayer.DeactivatePlaceGui();
		}

		public override bool CanUseItem(Player player)
		{
			PlanetModPlayer modPlayer = player.GetModPlayer<PlanetModPlayer>();

			bool changeStructBool = ChangeStructureType(player, modPlayer);

			bool isWithinRange = Vector2.Distance(Main.MouseWorld, player.Center) / 16 < Player.tileRangeX + 5 ? true : false;


			if (isWithinRange)
			{
				if (player.altFunctionUse == 2)				//rightclick (right click is basically identical to left click but without the placing)
				{
					if (FindDoor(new Point16((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)), player.Center))
					{
						if (!StructureMark(modPlayer.shipPlaceType))
						{
							modPlayer.cantPlace = true;
							ClearSelectedStruct();
						}
						//placing is here in leftclick
						modPlayer.oldDoorPos = selectedDoorPosition;
					}
					else
					{
						ClearAll(modPlayer);
					}
				}
				else										//leftclick (same as right click but can place)
				{
					if (FindDoor(new Point16((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)), player.Center))
					{
						if (!StructureMark(modPlayer.shipPlaceType))
						{
							modPlayer.cantPlace = true;
							ClearSelectedStruct();
						}
						else if (!changeStructBool && selectedDoorPosition.X == modPlayer.oldDoorPos.X && selectedDoorPosition.Y == modPlayer.oldDoorPos.Y)
						{
							CreateStructure();
						}
						modPlayer.oldDoorPos = selectedDoorPosition;
					}
					else
					{
						ClearAll(modPlayer);
					}
				}
			}
			return base.CanUseItem(player);
		}
	}
}
