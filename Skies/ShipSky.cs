using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.Planets.PlanetWorld;
using PlanetMod.Helpers;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SubworldLibrary;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using PlanetMod.Handlers;
using static PlanetMod.Handlers.PlanetHandler;

namespace PlanetMod.Skies
{
	public class ShipSky : CustomSky
	{
		private bool _isActive;

		public override void OnLoad()
		{
			StarHandler.SpawnStars();
		}

		public override void Update(GameTime gameTime)
		{
			StarHandler.UpdateStars();
		}

		public override Color OnTileColor(Color inColor)
		{
			return new Color((inColor.R / 2) + 32, (inColor.G / 2) + 32, (inColor.B / 2) + 32); //using floats really breaks stuff, use ints inside: new Color()
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if(maxDepth >= 0 && minDepth < 0)
			{
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				Main.spriteBatch.Draw(PlanetMod.spaceBackground, SkyHelper.PlanetPosition(), PlanetMod.spaceBackground.Frame(), new Color(30, 30, 30), SkyHelper.TimeRotation() + shipViewRotation, PlanetMod.spaceBackground.Frame().Size() / 2, 0.9f, SpriteEffects.None, 0f);


				#region stars
				//if (StarHandler.starInit)
				//{
				for (int k = 0; k < PlanetMod.galaxyLargeArraySize; k++)//star drawing, ship version
				{
					Main.spriteBatch.Draw(PlanetMod.galaxyLargeTexArray[StarHandler.largeGalaxyArray[k].type],

					new Vector2(0, (planetArray[SelectedPlanet].ShipBackroundTexture.Frame().Height * 1.5f) + StarHandler.largeGalaxyArray[k].position.X)
						.RotatedBy(StarHandler.largeGalaxyArray[k].position.Y + SkyHelper.TimeRotation() + shipViewRotation) + SkyHelper.PlanetPosition(),

					new Rectangle(0, 0, PlanetMod.galaxyLargeTexArray[StarHandler.largeGalaxyArray[k].type].Width, PlanetMod.galaxyLargeTexArray[StarHandler.largeGalaxyArray[k].type].Height),
					Color.White,
					StarHandler.largeGalaxyArray[k].rotation + shipViewRotation + SkyHelper.TimeRotation(),
					new Vector2((float)PlanetMod.galaxyLargeTexArray[StarHandler.largeGalaxyArray[k].type].Width * 0.5f,
						(float)PlanetMod.galaxyLargeTexArray[StarHandler.largeGalaxyArray[k].type].Height * 0.5f),
					StarHandler.largeGalaxyArray[k].scale,
					SpriteEffects.None,
					0f);
				}

				for (int k = 0; k < StarHandler.smallGalaxyCount; k++)//star drawing, ship version
				{
					Main.spriteBatch.Draw(PlanetMod.galaxySmallTexArray[StarHandler.smallGalaxyArray[k].type],

					new Vector2(0, (planetArray[SelectedPlanet].ShipBackroundTexture.Frame().Height * 1.5f) + StarHandler.smallGalaxyArray[k].position.X)
						.RotatedBy(StarHandler.smallGalaxyArray[k].position.Y + SkyHelper.TimeRotation() + shipViewRotation) + SkyHelper.PlanetPosition(),

					new Rectangle(0, 0, PlanetMod.galaxySmallTexArray[StarHandler.smallGalaxyArray[k].type].Width, PlanetMod.galaxySmallTexArray[StarHandler.smallGalaxyArray[k].type].Height),
					Color.White,
					StarHandler.smallGalaxyArray[k].rotation + shipViewRotation + SkyHelper.TimeRotation(),
					new Vector2((float)PlanetMod.galaxySmallTexArray[StarHandler.smallGalaxyArray[k].type].Width * 0.5f,
						(float)PlanetMod.galaxySmallTexArray[StarHandler.smallGalaxyArray[k].type].Height * 0.5f),
					StarHandler.smallGalaxyArray[k].scale,
					SpriteEffects.None,
					0f);
				}

				for (int k = 0; k < StarHandler.starCount; k++)//star drawing, ship version
					{
						Main.spriteBatch.Draw(PlanetMod.starTexArray[StarHandler.starArray[k].type],

						new Vector2(0, (planetArray[SelectedPlanet].ShipBackroundTexture.Frame().Height * 1.5f) + StarHandler.starArray[k].position.X)
							.RotatedBy(StarHandler.starArray[k].position.Y + SkyHelper.TimeRotation() + shipViewRotation) + SkyHelper.PlanetPosition(),

						new Rectangle(0, 0, PlanetMod.starTexArray[StarHandler.starArray[k].type].Width, PlanetMod.starTexArray[StarHandler.starArray[k].type].Height),
						Color.White,
						StarHandler.starArray[k].rotation + shipViewRotation + SkyHelper.TimeRotation(),
						new Vector2((float)PlanetMod.starTexArray[StarHandler.starArray[k].type].Width * 0.5f,
							(float)PlanetMod.starTexArray[StarHandler.starArray[k].type].Height * 0.5f),
						StarHandler.starArray[k].scale * StarHandler.starArray[k].twinkle,
						SpriteEffects.None,
						0f);
					}
				//}
				#endregion

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				#region orbits
				Main.spriteBatch.Draw(Main.sunTexture, SkyHelper.OrbitPosition(SkyHelper.PlanetPosition(), SkyHelper.screenCenter, 4.31f), Main.sunTexture.Frame(), new Color(230, 230, 255), SkyHelper.TimeRotation() + shipViewRotation + 1.175f, Main.sunTexture.Frame().Size() / 2, planetArray[SelectedPlanet].BgSunSize, SpriteEffects.None, 0f);
				if (planetArray[SelectedPlanet].subPlanets.Length > 0)//orbiting background planets
				{
					for (int i = 0; i < planetArray[SelectedPlanet].subPlanets.Length; i++)
					{
						ushort subPlanetType = planetArray[SelectedPlanet].subPlanets[i];
						Main.spriteBatch.Draw(planetArray[subPlanetType].subFarBackroundTexture, SkyHelper.OrbitPosition(SkyHelper.PlanetPosition(), SkyHelper.screenCenter, planetArray[subPlanetType].subOrbitTimeOffset, planetArray[subPlanetType].subOrbitSpeedMult), planetArray[subPlanetType].subFarBackroundTexture.Frame(), Color.Gray, (SkyHelper.TimeRotation() * planetArray[subPlanetType].subRotationMult) + shipViewRotation, planetArray[subPlanetType].subFarBackroundTexture.Frame().Size() / 2, planetArray[subPlanetType].subFarSize, SpriteEffects.None, 0f);
						//glowmask here, Color.White
					}
				}
				#endregion

				#region ship current planet
				Texture2D lowClouds = ModContent.GetTexture("PlanetMod/Planets/Globes/Cloud_Low");
				Texture2D highClouds = ModContent.GetTexture("PlanetMod/Planets/Globes/Cloud_High");

				//Main Planet
				Main.spriteBatch.Draw(planetArray[SelectedPlanet].ShipBackroundTexture, SkyHelper.PlanetPosition(), planetArray[SelectedPlanet].ShipBackroundTexture.Frame(), SkyHelper.PlanetColor(), (SkyHelper.TimeRotation() * planetArray[SelectedPlanet].BgRotationMult) + shipViewRotation, planetArray[SelectedPlanet].ShipBackroundTexture.Frame().Size() / 2, planetArray[SelectedPlanet].BgPlanetSize, SpriteEffects.None, 0f);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				//Medium TODO: glowmask (Color.White)

				if (planetArray[SelectedPlanet].BgClouds)
				{
					Main.spriteBatch.Draw(lowClouds, SkyHelper.PlanetPosition(), lowClouds.Frame(), SkyHelper.PlanetColor(200), (SkyHelper.TimeRotation() * 3) + shipViewRotation, lowClouds.Frame().Size() / 2, planetArray[SelectedPlanet].BgPlanetSize * 1.005f, SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(highClouds, SkyHelper.PlanetPosition(), highClouds.Frame(), SkyHelper.PlanetColor(100), (SkyHelper.TimeRotation() * 2) + shipViewRotation, highClouds.Frame().Size() / 2, planetArray[SelectedPlanet].BgPlanetSize * 1.01f, SpriteEffects.None, 0f);
				}
				#endregion

				spriteBatch.End();//reset to default
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.BackgroundViewMatrix.ZoomMatrix);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			_isActive = false;
		}

		public override void Reset()
		{
			_isActive = false;
		}

		public override bool IsActive()
		{
			return _isActive;
		}
	}
}
