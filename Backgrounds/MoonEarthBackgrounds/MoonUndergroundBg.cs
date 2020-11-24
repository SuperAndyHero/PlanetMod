using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;

namespace PlanetMod.Backgrounds.MoonEarthBackgrounds
{
	public class MoonUndergroundBg : ModUgBgStyle
	{
		public override bool ChooseBgStyle() {
			return Subworld.IsActive<Planets.MoonEarth>();
		}

		public override void FillTextureArray(int[] textureSlots) {
			textureSlots[0] = ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonUg0");
			textureSlots[1] = ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonUg1");
			textureSlots[2] = ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonUg2");
			textureSlots[3] = ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/MoonEarthBackgrounds/MoonUg3");
		}
	}
}