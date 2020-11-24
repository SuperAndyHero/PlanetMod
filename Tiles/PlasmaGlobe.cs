using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PlanetMod.Tiles
{
	public class PlasmaGlobeInstance
	{
		//public vars, to be changed in init method
		public int tendrilCount = 20;
		public float mainLength = 200f;
		public float pointDistance = 4.5f;
		public float pointSize = 0.35f;
		public Vector2 offset = Vector2.Zero;//in world coords, not tile coords
		public float centerSize = 1f;


		//variables moved here
		public float mainSwerveStrength = 16f;//likely const //swerve on main sine wave
		public float secondSwerveStrength = 4f;//likely const //swerve on secondary sine wave

		public float mainSwerveFreq = ((float)System.Math.PI / 2) * 0.1f;//likely const //swerve on main sine wave
		public float secondSwerveFreq = 1.2f;//likely const //swerve on secondary sine wave

		public float mainSwerveSpeed = 0.9f;//likely const //swerve on main sine wave
		public float secondSwerveSpeed = 0.3f;//likely const //swerve on secondary sine wave

		//private var, can be made public
		private readonly float rotSpeed = 0.015f;
		//init vars
		private PlasmaTendril[] tendrils;
		private bool Init = false;

		public void Initialize(int x, int y)
		{
			int h = 0;
			foreach (int tileType in PlanetMod.validPlasmaTiles)//move this to init on instance side
			{
				if (tileType == Main.tile[x, y].type)
				{
					SetValues(h);
					break;
				}
				h++;
			}

			tendrils = new PlasmaTendril[tendrilCount];
			for (int i = 0; i < tendrilCount; i++)
			{
				tendrils[i] = new PlasmaTendril((((float)i / (float)tendrilCount) * 6.28f) + Main.rand.NextFloat(0.5f), Main.rand.NextBool() ? 1 : -1, Main.rand.NextFloat(100f, 225f));
			}
			Init = true;
		}

		private void SetValues(int type)//sets the values for this instance based on what tile this is
		{
			switch (type)
			{
				case 0://small
					tendrilCount = 12;
					mainLength = 13f;
					pointDistance = 1.0f;
					pointSize = 0.08f;
					offset = Vector2.One * 16;
					centerSize = 0.35f;

					mainSwerveStrength = 2f;
					secondSwerveStrength = 1f;

					mainSwerveFreq = ((float)System.Math.PI / 2) * 0.6f;
					secondSwerveFreq = 5f;

					mainSwerveSpeed = 0.6f;
					secondSwerveSpeed = 0.1f;
					//Main.NewText("correctTile ");
					break;

				case 1://medium
					tendrilCount = 16;
					mainLength = 20.5f;
					pointDistance = 1.1f;
					pointSize = 0.09f;
					offset = Vector2.One * 24;
					centerSize = 0.37f;

					mainSwerveStrength = 3f;
					secondSwerveStrength = 1f;

					mainSwerveFreq = ((float)System.Math.PI / 2) * 0.5f;
					secondSwerveFreq = 5f;

					mainSwerveSpeed = 0.6f;
					secondSwerveSpeed = 0.2f;
					//Main.NewText("correctTile ");
					break;

				case 2://large
					tendrilCount = 20;
					mainLength = 28.5f;
					pointDistance = 1.0f;
					pointSize = 0.1f;
					offset = Vector2.One * 32;
					centerSize = 0.39f;

					mainSwerveStrength = 4f;
					secondSwerveStrength = 1f;

					mainSwerveFreq = ((float)System.Math.PI / 2) * 0.4f;
					secondSwerveFreq = 5f;

					mainSwerveSpeed = 0.6f;
					secondSwerveSpeed = 0.3f;
					//Main.NewText("correctTile ");
					break;
			};
		}

		public void UpdatePlasma()
		{
			if (Init)
			{
				for (int i = 0; i < tendrilCount; i++)
				{
					if(Main.rand.Next(40) == 0)
					{
						tendrils[i].rotOffset = Main.rand.NextFloat(6.28f);
					}
					
					float rot = System.Math.Abs((((Main.GameUpdateCount * rotSpeed) + tendrils[i].rotOffset) * tendrils[i].speedMult) % 6.28f);
					//Main.NewText(rot);
					if(rot < 3.16f && rot > 3.12f)
					{
						tendrils[i].rotOffset = Main.rand.NextFloat(0, 6.28f);
						int mult = ((((Main.GameUpdateCount * rotSpeed) + tendrils[i].rotOffset) % 6.28f) < 3.14 ? 1 : -1);//direction based on side
						tendrils[i].speedMult = Main.rand.NextFloat(0.8f, 1.2f) * mult;
					}
				}
			}
			//else
			//{
			//	Initialize();
			//}
		}

		public void DrawPlasma(Point16 tilePosition, Texture2D glowTexture, Texture2D midTexture, Texture2D endTexture, SpriteBatch spriteBatch)
		{
			//Main.NewText("draw " + offset);
			if (Init)
			{
				Vector2 drawPos = ((tilePosition.ToVector2() * 16) + offset) - Main.screenPosition;
				//Main.NewText(drawPos);

				void DrawPlasmaEnd(Vector2 origin, Vector2 position)
				{
					float dir = ((origin + position) - drawPos).ToRotation() - 1.57f;
					spriteBatch.Draw(endTexture, drawPos + new Vector2(0, mainLength).RotatedBy(dir), //drawPos + (Vector2.Normalize(position) * mainLength)
								endTexture.Frame(), new Color(255, 130, 130, 192), dir, endTexture.Frame().Size() * 0.5f, pointSize * 2f, SpriteEffects.None, 0f);
				}
				
				for (int e = 0; e < tendrilCount; e++)
				{
					//where it splits
					float splitLength = ((float)System.Math.Sin((Main.GameUpdateCount + (tendrils[e].rotOffset * 50) * tendrils[e].speedMult) * 0.03f) * mainLength) + (mainLength * 1.5f);

					float primaryLength = splitLength < mainLength ? splitLength : mainLength;
					float secondaryLength = mainLength - primaryLength;

					int primaryPointCount = (int)(primaryLength / pointDistance);
					int secondaryPointCount = (int)(secondaryLength / pointDistance);

					float rotDirection = ((Main.GameUpdateCount * rotSpeed) + tendrils[e].rotOffset) * tendrils[e].speedMult;

					for (int k = 0; k < primaryPointCount; k++)
					{
						float swerveMult = (float)System.Math.Sin(((float)k / (float)primaryPointCount) * 3.14f);

						float sin1 = (float)System.Math.Sin((k + (Main.GameUpdateCount * mainSwerveSpeed * tendrils[e].speedMult)) * (pointDistance * 0.1f) * mainSwerveFreq);//last number is freq
						float sin2 = (float)System.Math.Sin((k + (Main.GameUpdateCount * secondSwerveSpeed * tendrils[e].speedMult)) * (pointDistance * 0.1f) * secondSwerveFreq);

						float offsetX = ((sin1 * mainSwerveStrength) + (sin2 * secondSwerveStrength)) * swerveMult;//(sin2 * secondSwerveStrength)
						Vector2 position = new Vector2(offsetX, k * (primaryLength / primaryPointCount)).RotatedBy(rotDirection);

						spriteBatch.Draw(glowTexture, drawPos + position, glowTexture.Frame(), new Color(100, 120, 255, 128), 0f, glowTexture.Frame().Size() * 0.5f, pointSize, SpriteEffects.None, 0f);
					
						if(primaryLength == mainLength && k == primaryPointCount - 1)
						{
							DrawPlasmaEnd(drawPos, position);
						}
					}

					if (splitLength < mainLength)
					{
						Vector2 secondaryPoint = drawPos + new Vector2(0, primaryLength).RotatedBy(rotDirection);
						for (int f = 1; f < 3; f++)
						{
							float offset = (((f + (Main.GameUpdateCount * rotSpeed)) + tendrils[e].rotOffset));
							for (int k = 0; k < secondaryPointCount; k++)
							{
								float swerveEndMult = (float)System.Math.Sin(((float)k / (float)secondaryPointCount) * 1.57f);

								float secondDirection = rotDirection + (((float)System.Math.Sin(offset) + (offset % 3.14f)) - 1.57f) * 0.5f;

								float sin1 = (float)System.Math.Sin((k + (Main.GameUpdateCount * secondSwerveSpeed * tendrils[e].speedMult)) * (pointDistance * 0.1f));
					
								float offsetX = sin1 * swerveEndMult * secondSwerveStrength;
								Vector2 position = new Vector2(offsetX, k * (secondaryLength / secondaryPointCount)).RotatedBy(secondDirection);

								spriteBatch.Draw(glowTexture, secondaryPoint + position, glowTexture.Frame(), new Color(110, 130, 255, 64), 0f, glowTexture.Frame().Size() * 0.5f, 0.9f * pointSize, SpriteEffects.None, 0f);
								
								if (k == secondaryPointCount - 1)
								{
									DrawPlasmaEnd(secondaryPoint, position);
								}
							}
						}
					}
				}

				//center, done here since draw additive
				spriteBatch.Draw(midTexture, drawPos, midTexture.Frame(), new Color(255, 130, 130, 255), 0, midTexture.Frame().Size() * 0.5f, centerSize, SpriteEffects.None, 0f);
			}
			else
			{
				Initialize(tilePosition.X, tilePosition.Y);
			}
		}

		public struct PlasmaTendril
		{
			public float rotOffset;
			public float speedMult;

			public PlasmaTendril(float offset, float speed, float split)
			{
				this.rotOffset = offset;
				this.speedMult = speed;
			}
		}
	}
}