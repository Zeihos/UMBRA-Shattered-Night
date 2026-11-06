using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class AstralRiftWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index = tasks.FindIndex(task => task.Name == "Shinies");
  if(index<0) index=tasks.Count-1;
  tasks.Insert(index+1,new PassLegacy("UMBRA: Astral Rift",(progress,config)=> {
   progress.Message="Crystallizing the Astral Rift";
   int minX=Main.maxTilesX*2/3;
   int maxX=Math.Min(Main.maxTilesX-160,Main.maxTilesX*5/6);
   int minY=(int)Main.rockLayer+100;
   int maxY=Main.maxTilesY-290;
   if(maxX<=minX || maxY<=minY) return;
   int x=WorldGen.genRand.Next(minX,maxX);
   int y=WorldGen.genRand.Next(minY,maxY);
   UmbraWorld.RiftX=x;UmbraWorld.RiftY=y;
   int glass=ModContent.TileType<AstralGlass>();
   for(int n=0;n<125;n++) {
    int xx=x+WorldGen.genRand.Next(-52,53);
    int yy=y+WorldGen.genRand.Next(-33,34);
    if(!WorldGen.InWorld(xx,yy,90)) continue;
    Tile t=Main.tile[xx,yy];
    if(!t.HasTile || (t.TileType!=TileID.Stone && t.TileType!=TileID.Dirt)) continue;
    WorldGen.TileRunner(xx,yy,WorldGen.genRand.Next(3,8),WorldGen.genRand.Next(3,7),glass);
   }
  }));
 }
}
