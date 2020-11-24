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
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using System.Globalization;
using PlanetMod.Planets;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static PlanetMod.Planets.PlanetWorld;
using static PlanetMod.Handlers.PlanetHandler;

namespace PlanetMod.Handlers
{
    public static class TeleportHandler
    {
        public static bool IsTeleporting = false;
        public static int TeleportTimer = 0;//counter for teleporter
        public const int TeleportTimerMax = 18;//how many ticks the animation should be (setting too low (around 15, didn't test below 18) seems to cause issues)

        public static void StartTeleport()//starts teleporter animation and syncs value
        {
            IsTeleporting = true;
            
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }
        }

        public static void MainTeleport()
        {
            if (Main.netMode == NetmodeID.SinglePlayer) //singleplayer: start sequence
            {
                StartTeleport();//starts teleporter animation
            }
            else //multiplayer: Start Vote (TeleportTimer is increased in OnVotedFor())
            {
                TeleportCorrectWorld();
            }
        }

        public static void TeleportCorrectWorld(bool noVote = false)
        {
            if (Subworld.IsActive<ShipSubworld>())//based on active subworld, may change to use active subworld ID
            {
                //Main.NewText(planetArray[SelectedPlanet].EntryString);
                if (planetArray[SelectedPlanet].EntryString == string.Empty)
                    Subworld.Exit(noVote);
                else
                    Subworld.Enter(planetArray[SelectedPlanet].EntryString, noVote);
            }
            else
            {
                Subworld.Enter<ShipSubworld>(noVote);
            }
        }
    }
}