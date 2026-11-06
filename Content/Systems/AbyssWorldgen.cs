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
   // Two buried cache rooms with a vanilla chest. Only replace ordinary stone/dirt,
   // and keep structures far from world boundaries.
   for (int chamber=0; chamber<2; chamber++) {
    int cx=x+(chamber==0?-26:26);
    int cy=y+WorldGen.genRand.Next(-9,10);
    if(!WorldGen.InWorld(cx,cy,90)) continue;
    bool clear=true;
    for(int xx=cx-8;xx<=cx+8 && clear;xx++) for(int yy=cy-5;yy<=cy+5;yy++) {
     Tile t=Main.tile[xx,yy];
     if(!t.HasTile || (t.TileType!=TileID.Stone && t.TileType!=TileID.Dirt && t.TileType!=rock)) {clear=false;break;}
    }
    if(!clear) continue;
    for(int xx=cx-8;xx<=cx+8;xx++) for(int yy=cy-5;yy<=cy+5;yy++) {
     if(xx==cx-8 || xx==cx+8 || yy==cy-5 || yy==cy+5)
      WorldGen.PlaceTile(xx,yy,rock,true,true);
     else WorldGen.KillTile(xx,yy,noItem:true);
    }
    // A guaranteed floor for the chest and a shared-loot discovery reward.
    int chestIndex=WorldGen.PlaceChest(cx,cy+4,TileID.Containers,notNearOtherChests:false,style:0);
    if(chestIndex>=0) {
     Chest chest=Main.chest[chestIndex];
     chest.item[0].SetDefaults(ModContent.ItemType<UmbraShatteredNight.Content.Items.VoidShard>());
     chest.item[0].stack=WorldGen.genRand.Next(6,13);
     chest.item[1].SetDefaults(ModContent.ItemType<UmbraShatteredNight.Content.Items.UmbralOreItem>());
     chest.item[1].stack=WorldGen.genRand.Next(9,18);
    }
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
