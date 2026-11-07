using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
namespace UmbraShatteredNight.Content.Systems;
public class CinderOreWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index=tasks.FindIndex(pass=>pass.Name=="UMBRA: Ashen Wastes");
  if(index<0) return;
  tasks.Insert(index+1,new PassLegacy("UMBRA: Cinder deposits",(progress,config)=> {
   progress.Message="Seeding cinder deposits";
   if(UmbraWorld.AshenX<=0 || UmbraWorld.AshenY<=0) return;
   int ore=ModContent.TileType<CinderOre>();
   for(int n=0;n<95;n++) {
    int x=UmbraWorld.AshenX+WorldGen.genRand.Next(-55,56);
    int y=UmbraWorld.AshenY+WorldGen.genRand.Next(-37,38);
    if(!WorldGen.InWorld(x,y,100)) continue;
    Tile cell=Main.tile[x,y];
    if(!cell.HasTile || cell.TileType!=ModContent.TileType<AshenStone>()) continue;
    SafeVeinWorldgen.Patch(x, y, WorldGen.genRand.Next(2,5), WorldGen.genRand.Next(2,6), ore);
   }
  }));
 }
}
