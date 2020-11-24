using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using System;
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using PlanetMod.Handlers;
using static PlanetMod.Handlers.PlanetHandler;
using static PlanetMod.Handlers.TeleportHandler;
using static PlanetMod.Helpers.Helper;
using PlanetMod.Tiles;

namespace PlanetMod.Planets
{
    public class PlanetWorld : ModWorld
    {
		public override void Initialize()
		{
			IsTeleporting = false;//these are set here so the animation doesn't stop once the subworld is being loaded
			TeleportTimer = 0;
		}

		public override void NetReceive(BinaryReader reader)
		{
			IsTeleporting = reader.ReadBoolean();
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(IsTeleporting);
		}

        //public override void PreUpdate()
        //{

        //}
        //public override void PostUpdate()
        //{
            
        //}

        public override void PostDrawTiles()//TE drawing
        {
            Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.TransformationMatrix);

            //int tileEntityType = ModContent.TileEntityType<Tiles.PlasmaEntity>();
            foreach (var item in TileEntity.ByID)
            {
                //if this if-else chain gets too long replace with switch case
                if (item.Value.type == ModContent.TileEntityType<PlasmaEntity>())//just a bit of spaghetti~
                {
                    var currentEntity = item.Value as PlasmaEntity;
                    Point16 curEntPos = currentEntity.EntityPosition();


                    if (Main.tile[curEntPos.X, curEntPos.Y].frameX == 0)
                    {
                        Texture2D tex = ModContent.GetTexture("PlanetMod/ball");
                        Texture2D texEnd = ModContent.GetTexture("PlanetMod/end");
                        Texture2D texMid = ModContent.GetTexture("PlanetMod/mid");
                        currentEntity.PlasmaInstance.DrawPlasma(currentEntity.EntityPosition(), tex, texMid, texEnd, Main.spriteBatch);
                    }
                }
                else if (item.Value.type == ModContent.TileEntityType<TvEntity>())
                {
                    var currentEntity = item.Value as TvEntity;
                    Point16 curEntPos = currentEntity.EntityPosition();
                    Rectangle curEntRect = currentEntity.GetFrameSize();

                    Tile tile = Main.tile[curEntPos.X, curEntPos.Y];
                    int currentAnim = (tile.frameY / currentEntity.GetTileFrameHeight());//-1 before use as index  

                    if (currentAnim > 0 && currentAnim <= PlanetMod.animArraySize)
                    {
                        int currentFrame = ((Main.tileFrameCounter[tile.type] / (PlanetMod.animFrameDelay[currentAnim - 1] + 1)) % PlanetMod.animFrameCount[currentAnim - 1]) * (PlanetMod.animArray[currentAnim - 1].Height / PlanetMod.animFrameCount[currentAnim - 1]);
                        
                        Main.spriteBatch.Draw(PlanetMod.animArray[currentAnim - 1], new Rectangle((curEntPos.X * 16) + curEntRect.X - (int)(Main.screenPosition.X), (curEntPos.Y * 16) + curEntRect.Y - (int)(Main.screenPosition.Y), curEntRect.Width, curEntRect.Height), new Rectangle(0, currentFrame, PlanetMod.animArray[currentAnim - 1].Width, PlanetMod.animArray[currentAnim - 1].Height / PlanetMod.animFrameCount[currentAnim - 1]), Color.White);
                    }
                }
            }

            Main.spriteBatch.End();
        }
    }
}