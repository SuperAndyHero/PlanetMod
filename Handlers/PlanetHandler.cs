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
using static PlanetMod.Planets.PlanetWorld;

namespace PlanetMod.Handlers
{
    public static class PlanetHandler
    {
        const ushort PlanetsMax = 3;//How many planets are there

        public enum PlanetID : ushort
        {
            Earth = 0,
            Moon = 1,
            RockyPlanet = 2
        }

        //SYNC: this may need to get synced
        public static ushort SelectedPlanet = 0;//which planet is selected, used for the ship BG, as well as what subPlanets to show in BG

        //Where planets are defined
        public static Planet[] planetArray = new Planet[PlanetsMax]//Main planet array, could be a list, but I dont see it as needed
        {
            new Planet{ ShownName = "Terra",

                EntryString = string.Empty,//This is empty because its not a subworld (there will be a check for if its empty before its used)
                ShipBackroundTexture = ModContent.GetTexture("PlanetMod/Planets/Globes/Earth_Corrupt_1"),
                BgClouds = true,
                BgRotationMult = 2,
                BgPlanetSize = 3f,
                BgPlanetPosY = 1.8f,
                BgSunSize = 1.5f,
                subPlanets = new ushort[1]{ (ushort)PlanetID.Moon },

                subOrbitTimeOffset = 1.04f,
                subFarSize = 1.5f,//large in the sky
                subOrbitSpeedMult = 2,//twice the suns speed
                subRotationMult = 2,//tidally locked
                subFarBackroundTexture = ModContent.GetTexture("PlanetMod/Planets/FarTexture/EarthFar")
            },

            new Planet{ ShownName = "Luna",

                EntryString = "PlanetMod_MoonEarth",
                ShipBackroundTexture = ModContent.GetTexture("PlanetMod/Planets/Globes/MoonEarth"),
                BgClouds = false,
                BgRotationMult = 2,
                BgPlanetSize = 3f,
                BgPlanetPosY = 1.9f,
                BgSunSize = 1.5f,
                subPlanets = new ushort[1]{ (ushort)PlanetID.Earth },

                subOrbitTimeOffset = 3.14f,//opposite the sun's side
                subFarSize = 1f,//small in the sky
                subOrbitSpeedMult = 1,//same speed as sun
                subRotationMult = 1,//tidally locked
                subFarBackroundTexture = ModContent.GetTexture("PlanetMod/Planets/FarTexture/MoonEarthFar")
            },

            new Planet{ ShownName = "Rocky Place",

                EntryString = "PlanetMod_RockyWorld",
                ShipBackroundTexture = ModContent.GetTexture("PlanetMod/Planets/Globes/RockyWorld"),
                BgClouds = true,
                BgRotationMult = 2,
                BgPlanetSize = 3.4f,
                BgPlanetPosY = 2.0f,
                BgSunSize = 1.0f,
                subPlanets = new ushort[0]
            }
        };

        public static int GetCurrentWorldID()//Main way to get the current world ID, -1 is ship, zero is earth/other mod's subworld
        {
            if (Subworld.AnyActive<PlanetMod>())
            {
                if (!Subworld.IsActive<ShipSubworld>())
                {
                    return SelectedPlanet;
                }
                else
                {
                    return -1;//ship ID
                }
            }
            return 0;//earth (or another mod's subworld)
        }
    }

    public struct Planet
    {
        public string ShownName;//display name
        public string EntryString;//This is the string that gets used to enter the subworld

        //public Texture2D glowmask
        public Texture2D ShipBackroundTexture;//texture to be used when looking down at it from the ship
        public bool BgClouds;//if the ShipBackroundTexture should have clouds layered on it
        public int BgRotationMult;
        public float BgPlanetSize;
        public float BgPlanetPosY;
        public float BgSunSize;//this mat get replaced with sunDistance
        //public float dayspeed
        public ushort[] subPlanets;//this array has any references to sub planets, if none initalize it with zero length

        //sub planet values, these do not have to be set if the planet has no subplanets or isnt a subplanet
        public float subOrbitTimeOffset;//these only get used if this planet is a subplanet to another planet, can be left out if not a subplanet (leaving this out may get changed for solar system view)
        public float subFarSize;
        public int subOrbitSpeedMult;
        public int subRotationMult;//if this is the same as orbit speed the subplanet will be tidally locked
        public Texture2D subFarBackroundTexture;//if this is a subplanet all must be set
        //public Texture2D subGlowmask
    }
}