using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using PlanetMod.Utils;
using System;

namespace PlanetMod.Helpers
{
	public static class NoiseHelper //helpers are just for methods and should not store any values
	{
		public static int[] Get2dDisplacements(int displacementCount, float frequency, int maxLimit, float multiplier, int seed, FastNoise.NoiseType noiseType)//libvaxy method
		{
			FastNoise noise = new FastNoise(seed);
			noise.SetNoiseType(noiseType);
			noise.SetFrequency(frequency);

			int[] displacements = new int[displacementCount];

			for (int x = 0; x < displacementCount; x++)
				displacements[x] = (int)Math.Floor(noise.GetNoise(x, x) * maxLimit * multiplier);

			return displacements;
		}
	}
}