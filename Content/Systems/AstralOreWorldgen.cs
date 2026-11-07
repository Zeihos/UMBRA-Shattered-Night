using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class AstralOreWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index=tasks.FindIndex(pass=>pass.Name=="UMBRA: Astral Rift");
  if(index<0) return;
  tasks.Insert(index+1,new PassLegacy("UMBRA: Astral deposits",(progress,config)=> {
   progress.Message="Seeding astral deposits";
   if(UmbraWorld.RiftX<=0||UmbraWorld.RiftY<=0) return;
   int ore=ModContent.TileType<AstralOre>();
   for(int n=0;n<75;n++) {
    int x=UmbraWorld.RiftX+WorldGen.genRand.Next(-58,59);
    int y=UmbraWorld.RiftY+WorldGen.genRand.Next(-38,39);
    if(!WorldGen.InWorld(x,y,100)) continue;
    // Do not seed ore from furniture, chests, dungeon bricks or other structures.
    Tile source = Main.tile[x,y];
    ushort glass = (ushort)ModContent.TileType<AstralGlass>();
    if (!source.HasTile || source.TileType != glass) continue;
    SafeVeinWorldgen.Patch(x, y, WorldGen.genRand.Next(2,5), WorldGen.genRand.Next(2,6), ore);
   }
  }));
 }
}
