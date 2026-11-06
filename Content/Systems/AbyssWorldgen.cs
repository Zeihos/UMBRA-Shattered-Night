using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class AbyssWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index = tasks.FindIndex(task => task.Name == "Shinies");
  if (index < 0) index = tasks.Count - 1;
  tasks.Insert(index+1, new PassLegacy("UMBRA: Fractured Abyss", (progress, config) => {
   progress.Message = "Carving the Fractured Abyss";
   int minX = Math.Max(220, Main.maxTilesX/6);
   int maxX = Math.Min(Main.maxTilesX-220, Main.maxTilesX/3);
   int x = WorldGen.genRand.Next(minX, maxX);
   int minY = (int)Main.rockLayer + 90;
   int maxY = Main.maxTilesY - 260;
   if (maxY <= minY) return;
   int y = WorldGen.genRand.Next(minY, maxY);
   UmbraWorld.AbyssX=x; UmbraWorld.AbyssY=y;
   int rock = ModContent.TileType<FracturedStone>();
   int ore = ModContent.TileType<UmbralOre>();
   // Dense stone veins create a subterranean pocket without removing dungeon or altar tiles.
   for (int n=0;n<95;n++) {
    int xx=x+WorldGen.genRand.Next(-48,49);
    int yy=y+WorldGen.genRand.Next(-29,30);
    if (!WorldGen.InWorld(xx,yy,80)) continue;
    Tile tile=Main.tile[xx,yy];
    if (!tile.HasTile || (tile.TileType!=TileID.Stone && tile.TileType!=TileID.Dirt)) continue;
    WorldGen.TileRunner(xx,yy,WorldGen.genRand.Next(4,9),WorldGen.genRand.Next(3,7),rock);
   }
   for (int n=0;n<25;n++) {
    int xx=x+WorldGen.genRand.Next(-38,39);
    int yy=y+WorldGen.genRand.Next(-24,25);
    if (!WorldGen.InWorld(xx,yy,80)) continue;
    if (Main.tile[xx,yy].HasTile && Main.tile[xx,yy].TileType==rock)
      WorldGen.TileRunner(xx,yy,WorldGen.genRand.Next(2,5),WorldGen.genRand.Next(2,4),ore);
   }
  }));
 }
}
