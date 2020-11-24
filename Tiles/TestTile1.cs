using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Text;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;
using System.IO;
using Terraria.ModLoader.IO;
using static PlanetMod.Ship.ShipWorld;
using static PlanetMod.PlanetMod;
using static PlanetMod.Handlers.ShipBuildingHandler;
using static PlanetMod.Helpers.Helper;
using SubworldLibrary;

namespace PlanetMod.Tiles
{
	public class TestTile : ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "PlanetMod/Tiles/BarrierWallOverlay";
			return true;
		}
		public override void SetDefaults()
		{
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PlasmaEntity>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileNoSunLight[Type] = false;
			//Main.tileLighted[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
		}	

		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
		}

		//public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		//{
		//	r = 0.2f;
		//	g = 0.2f;
		//	b = 0.2f;
		//}

		public override bool Slope(int i, int j) => false;
		public override bool CanExplode(int i, int j) => false;
		public override bool KillSound(int i, int j) => false;
		public override bool CreateDust(int i, int j, ref int type) => false;
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) => true;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail)
			{
				ModContent.GetInstance<PlasmaEntity>().Kill(i, j);
			}
		}
	}
}