using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class AshenWastesWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index = tasks.FindIndex(task => task.Name == "Shinies");
  if (index < 0) index = tasks.Count - 1;
  tasks.Insert(index + 1, new PassLegacy("UMBRA: Ashen Wastes", (progress, config) => {
   progress.Message = "Forming the Ashen Wastes";
   // Keep Ashen Wastes separate from the western Fractured Abyss
   // and the eastern Astral Rift, including on small worlds.
   int minX = Main.maxTilesX * 43 / 100;
   int maxX = Main.maxTilesX * 57 / 100;
   int minY = (int)Main.rockLayer + 130;
   int maxY = Main.maxTilesY - 300;
   if (maxX <= minX || maxY <= minY) return;
   int x = WorldGen.genRand.Next(minX, maxX);
   int y = WorldGen.genRand.Next(minY, maxY);
   UmbraWorld.AshenX = x; UmbraWorld.AshenY = y;
   int tileType = ModContent.TileType<AshenStone>();
   for (int n = 0; n < 140; n++) {
    int xx = x + WorldGen.genRand.Next(-47, 48);
    int yy = y + WorldGen.genRand.Next(-29, 30);
    if (!WorldGen.InWorld(xx, yy, 90)) continue;
    Tile tile = Main.tile[xx, yy];
    if (!tile.HasTile || (tile.TileType != TileID.Stone && tile.TileType != TileID.Dirt)) continue;
    SafeVeinWorldgen.Patch(xx, yy, WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 7), tileType);
   }
  }));
 }
}
