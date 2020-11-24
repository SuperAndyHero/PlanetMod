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
using System;
using static System.Math;
using SubworldLibrary;
using PlanetMod.Ship;
using Terraria.Graphics;
using System.Globalization;

namespace PlanetMod.Handlers
{
    public static class StarHandler //handlers are helpers that need to do more than hold methods
    {
		//public static bool starInit = false;
		public static int starCount = 1500;
		public static Star[] starArray;

		public static int smallGalaxyCount = 100;
		public static SpaceElement[] smallGalaxyArray;

		//public static int largeGalaxyCount = 1; //uses each galaxy once
		public static SpaceElement[] largeGalaxyArray;

		public static void SpawnStars()
		{
			//starCount = 1500;//debug


			//minor todo: replace main.rand uses with custom rand based on seed
			Main.NewText("Stars Reset");
			starArray = new Star[starCount];
			smallGalaxyArray = new SpaceElement[smallGalaxyCount];
			largeGalaxyArray = new SpaceElement[PlanetMod.galaxyLargeArraySize];

			for (int i = 0; i < starCount; i++)//stars
			{
				starArray[i] = new Star();
				starArray[i].position.X = (float)Main.rand.Next(0, (int)(Main.screenWidth * 0.7f));//distance
				starArray[i].position.Y = Main.rand.NextFloat(0, 6.28f);//rotation
				starArray[i].rotation = (float)Main.rand.Next(628) * 0.01f;
				starArray[i].scale = (float)Main.rand.Next(20, 45) * 0.01f;
				starArray[i].type = Main.rand.Next(0, PlanetMod.starArraySize);
				starArray[i].twinkle = (float)Main.rand.Next(101) * 0.01f;
				starArray[i].twinkleSpeed = (float)Main.rand.Next(5, 20) * 0.0001f;
				if (Main.rand.Next(4) == 0)
				{
					starArray[i].twinkleSpeed *= -1f;
				}
				starArray[i].rotationSpeed = (float)Main.rand.Next(5, 20) * 0.0001f;
				if (Main.rand.Next(4) == 0)
				{
					starArray[i].rotationSpeed *= -1f;
				}
			}

			for (int i = 0; i < smallGalaxyCount; i++)//small galaxies
			{
				smallGalaxyArray[i] = new SpaceElement();
				smallGalaxyArray[i].position.X = (float)Main.rand.Next(0, (int)(Main.screenWidth * 0.7f));//distance
				smallGalaxyArray[i].position.Y = Main.rand.NextFloat(0, 6.28f);//rotation
				smallGalaxyArray[i].rotation = (float)Main.rand.Next(628) * 0.01f;
				smallGalaxyArray[i].scale = (float)Main.rand.Next(20, 40) * 0.01f;
				smallGalaxyArray[i].type = Main.rand.Next(0, PlanetMod.galaxySmallArraySize);
			}

			for (int i = 0; i < PlanetMod.galaxyLargeArraySize; i++)//small galaxies
			{
				largeGalaxyArray[i] = new SpaceElement();
				largeGalaxyArray[i].position.X = (float)Main.rand.Next(0, (int)(Main.screenWidth * 0.7f));//distance
				largeGalaxyArray[i].position.Y = Main.rand.NextFloat(0, 6.28f);//rotation
				largeGalaxyArray[i].rotation = (float)Main.rand.Next(628) * 0.01f;
				largeGalaxyArray[i].scale = (float)Main.rand.Next(30, 45) * 0.01f;
				largeGalaxyArray[i].type = i;
			}

			//starInit = true;
		}

		public static void UpdateStars()
		{
			//if (starInit)
			//{
			for (int i = 0; i < Main.numStars; i++) //only stars need to be updated since spaceElements dont change
			{
				starArray[i].twinkle += starArray[i].twinkleSpeed;
				if (starArray[i].twinkle > 1f)
				{
					starArray[i].twinkle = 1f;
					starArray[i].twinkleSpeed *= -1f;
				}
				else if ((double)starArray[i].twinkle < 0.5)
				{
					starArray[i].twinkle = 0.5f;
					starArray[i].twinkleSpeed *= -1f;
				}
				starArray[i].rotation += starArray[i].rotationSpeed;
				if ((double)starArray[i].rotation > 6.28)
				{
					starArray[i].rotation -= 6.28f;
				}
				if (starArray[i].rotation < 0f)
				{
					starArray[i].rotation += 6.28f;
				}
			}
			//}
			//else
			//{
			//	SpawnStars();
			//}
		}
	}

	public class SpaceElement
	{
		public Vector2 position;

		public float scale;

		public float rotation;

		public int type;
	}
}