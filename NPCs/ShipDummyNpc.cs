using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace PlanetMod.Npcs
{
    internal class ShipDummyNpc : ModNPC
    {
        public Tile parent;
        public override string Texture => "PlanetMod/Null";
        public override void SetStaticDefaults() => DisplayName.SetDefault(string.Empty);
        public override bool CheckActive() => false;
        public override void SetDefaults()
        {
            npc.width = 16;
            npc.height = 16;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.immortal = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontCountMe = true;
        }

        public override void AI()
        {
            if (!parent.active() || ((parent.frameX <= 0 || parent.frameX > PlanetMod.shipPartArraySize) && (parent.frameY <= 0 || parent.frameY > PlanetMod.shipWallArraySize)))
            {
                //Main.NewText("x: " + parent.frameX);
                //Main.NewText("y: " + parent.frameY);
                //Main.NewText(PlanetMod.allShipPartArray + " 2: " + PlanetMod.allShipWallArray);
                npc.life = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int frameX = parent.frameX;

            if (frameX > 0 && frameX <= PlanetMod.shipPartArraySize)
            {
                Main.spriteBatch.Draw(PlanetMod.shipPartArray[frameX - 1], npc.position - Main.screenPosition, PlanetMod.shipPartArray[frameX - 1].Frame(), new Color(230, 230, 230, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
