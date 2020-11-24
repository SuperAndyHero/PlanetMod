using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;

namespace PlanetMod.Backgrounds.MoonEarthBackgrounds
{
	public class MoonSurfaceBg : ModSurfaceBgStyle
	{
		public override bool ChooseBgStyle() {
			return Subworld.IsActive<Planets.MoonEarth>();
		}

		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed) {
			for (int i = 0; i < fades.Length; i++) {
				if (i == Slot) {
					fades[i] += transitionSpeed;
					if (fades[i] > 1f) {
						fades[i] = 1f;
					}
				}
				else {
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f) {
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture()
		{
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonSurfaceFar");
		}

		public override int ChooseMiddleTexture()
		{
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonSurfaceMid");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonSurfaceClose");
		}
	}
}