using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;

namespace PlanetMod.Items
{
    public class ShipGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            //if(item.type == ItemID.RodofDiscord && (Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].wall == ModContent.WallType<Tiles.ShipBackroundWall>() || Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].wall == ModContent.WallType<Tiles.BarrierWall>()))
            //{
            //    return false;
            //}

            if(item.createWall != -1 && Main.tile[(int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16)].type == ModContent.TileType<Tiles.Barrier>())
            {
                return false;
            }
            return base.CanUseItem(item, player);
        }

    }
}