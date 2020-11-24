using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;

namespace PlanetMod.Backgrounds
{
	public class SpaceBg : ModSurfaceBgStyle
	{
		public override bool ChooseBgStyle() {
			return Subworld.IsActive<Ship.ShipSubworld>(); //biome backround is active if this dimension is active
			//return Subworld.AnyActive<PlanetMod>();
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

		public override int ChooseFarTexture() {
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/Blank");//sets all backrounds to blank
		}

		public override int ChooseMiddleTexture() {
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/Blank");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			return ModContent.GetModBackgroundSlot("PlanetMod/Backgrounds/Blank");
		}
	}
}