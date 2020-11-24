using Terraria;
using Terraria.ModLoader;

namespace PlanetMod
{
	public class NpcDebug : ModCommand
	{
		public override CommandType Type => CommandType.Chat;

		public override string Command => "npcs";

		public override string Usage => "/npcs";

		public override string Description => "gives ship npc info";

		public override void Action(CommandCaller caller, string input, string[] args) {
			int npcs = 0;
			int shipNpcs = 0;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active)
				{
					npcs++;

					if(Main.npc[i].type == ModContent.NPCType<Npcs.ShipDummyNpc>())
					{
						shipNpcs++;
					}
				}
			}
			Main.NewText("All npcs: " + npcs + " / " + Main.maxNPCs);
			Main.NewText("Ship npcs: " + shipNpcs + " / " + Main.maxNPCs);
			if (npcs >= Main.maxNPCs - 10)
			{
				Main.NewText("Warning: close to npc cap, dont place more ship pieces");
			}
		}
	}

	//public class ToggleCommand : ModCommand
	//{
	//	public override CommandType Type => CommandType.Chat;

	//	public override string Command => "toggle";

	//	public override string Usage => "/toggle";

	//	public override string Description => "Toggles green screen effect";

	//	public override void Action(CommandCaller caller, string input, string[] args)
	//	{
	//		GreenScreenPlayer.ToggleShader();
	//	}
	//}
}