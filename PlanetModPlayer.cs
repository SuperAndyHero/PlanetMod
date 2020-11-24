using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using SubworldLibrary;
using static PlanetMod.PlanetMod;
using static PlanetMod.Planets.PlanetWorld;
using static PlanetMod.Handlers.PlanetHandler;
using static PlanetMod.Handlers.TeleportHandler;
using static Terraria.ModLoader.ModContent;
using PlanetMod.Planets;

namespace PlanetMod
{
    public class PlanetModPlayer : ModPlayer
	{
        //SYNC: these values
        public bool cantPlace = false;
        public ushort shipPlaceType = 0;

        public Point16 oldDoorPos;

        private bool placeGuiShow = false;
        private int placeGuiTimer = 0;

        #region custom methods
        public void ActivatePlaceGui()
        {
            placeGuiShow = true;
            placeGuiTimer = 0;
            cantPlace = false;
        }

        public void DeactivatePlaceGui()
        {
            placeGuiShow = false;
            placeGuiTimer = 0;
            cantPlace = false;
        }
        #endregion


        public override void PreUpdate()
        {
            if (placeGuiShow)
            {
                placeGuiTimer++;
                if (placeGuiTimer > 150 || player.dead)
                {
                    DeactivatePlaceGui();
                }
            }

            if (IsTeleporting)
            {
                //Main.NewText(" tele timer: " + TeleportTimer); //debug

                player.immuneAlpha = 255;
                player.noKnockback = true;
                player.noFallDmg = true;
                player.noThrow = 255;
                player.frozen = true;
                player.velocity = new Vector2(0, -player.gravity);
                Lighting.AddLight(player.Center, 0.4f, 0.4f, 0.6f);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if(IsTeleporting)
            {
                return false;
            }
            else
            {
                return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
            }
        }

        public override void UpdateBiomeVisuals()
        {
            int worldID = GetCurrentWorldID();

            player.ManageSpecialBiomeVisuals("PlanetMod:ShipSky", worldID == -1, player.Center);

            player.ManageSpecialBiomeVisuals("PlanetMod:PlanetSky", worldID == (ushort)PlanetID.Moon, player.Center);
        }

        public static readonly PlayerLayer PlayerBehind = new PlayerLayer("VariedVanity", "PlayerBehind", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            PlanetModPlayer modPlayer = drawPlayer.GetModPlayer<PlanetModPlayer>();

            if (modPlayer.placeGuiShow)
            {
                if (drawInfo.drawPlayer.dead)
                        return;

                Texture2D texture = ModContent.GetTexture("PlanetMod/Tiles/Preview");

                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;

                int drawX = (int)(drawPlayer.position.X + (drawPlayer.width / 2f) - (texture.Width / 2f) - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y + (drawPlayer.height / 2f) - texture.Height - Main.screenPosition.Y);
                Rectangle drawRect = new Rectangle(drawX + 10, drawY + 10, 60, 44);//just for preview image

                //Ship Image
                DrawData data2 = new DrawData(overlayArray[modPlayer.shipPlaceType], drawRect, overlayArray[modPlayer.shipPlaceType].Frame(), new Color(255, 255, 255, alpha))
                {
                    shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.MirageDye)
                };
                Main.playerDrawData.Add(data2);

                //Red tint
                if (modPlayer.cantPlace)
                {
                    DrawData data3 = new DrawData(Main.blackTileTexture, drawRect, Main.blackTileTexture.Frame(), new Color(80, 0, 0, 10))
                    {
                        shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.MirageDye)
                    };
                    Main.playerDrawData.Add(data3);
                }

                //Border
                DrawData data = new DrawData(texture, new Rectangle(drawX, drawY, texture.Width, texture.Height), texture.Frame(), new Color(255, 255, 255, alpha / 2))
                {
                    shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.MirageDye)
                };
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PlayerInFront = new PlayerLayer("VariedVanity", "PlayerInFront", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            PlanetModPlayer modPlayer = drawPlayer.GetModPlayer<PlanetModPlayer>();
            if (IsTeleporting)
            {
                //doesn't check if dead

                Texture2D texture = ModContent.GetTexture("PlanetMod/Planets/TeleportFrames");
                const int frameCount = 10;
                int frameHeight = texture.Height / frameCount;

                int drawX = (int)(drawPlayer.position.X + (drawPlayer.width / 2f) - (texture.Width / 2f) - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y + drawPlayer.height - frameHeight - Main.screenPosition.Y);
                int frame = (int)(((float)TeleportTimer / (float)TeleportTimerMax) * frameCount);

                DrawData data = new DrawData(texture, new Rectangle(drawX, drawY, texture.Width, frameHeight), new Rectangle(0, frame * frameHeight, texture.Width, frameHeight), Color.White);
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            PlayerBehind.visible = true;
            layers.Insert(0, PlayerBehind);
            layers.Add(PlayerInFront);
        }
    }
}