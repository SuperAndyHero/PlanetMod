using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using Terraria.UI;
using Terraria.Graphics.Effects;

namespace PlanetMod.Items
{
    public class Pyrite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyrite sample");
            Tooltip.SetDefault("A shiny golden mineral \nCrude Semiconductor");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 22;
            item.maxStack = 999;
            item.rare = ItemRarityID.White;
        }
    }

    public class Galena : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Galena sample");
            Tooltip.SetDefault("A shiny silver mineral \nCrude Semiconductor");
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 22;
            item.maxStack = 999;
            item.rare = ItemRarityID.White;
        }

    }

    public class Diode : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crude diode");
            //Tooltip.SetDefault("A simple diode");
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("PlanetMod:Semiconductors");
            recipe.AddRecipeGroup("PlanetMod:Copper", 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();

            //recipe = new ModRecipe(mod);
            //recipe.AddRecipeGroup("PlanetMod:Semiconductors");
            //recipe.AddRecipeGroup("PlanetMod:Copper");
            //recipe.AddTile(TileID.Furnaces);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
    }
}