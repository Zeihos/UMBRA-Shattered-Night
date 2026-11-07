using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using UmbraShatteredNight.Content.Tiles;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Systems;
public class AshenShrineWorldgen : ModSystem {
 public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
  int index = tasks.FindIndex(pass => pass.Name == "UMBRA: Ashen Wastes");
  if (index < 0) return;
  tasks.Insert(index + 1, new PassLegacy("UMBRA: Ashen Shrine", (progress, config) => {
   progress.Message = "Building the Ashen Shrine";
   int cx = UmbraWorld.AshenX, cy = UmbraWorld.AshenY;
   if (!WorldGen.InWorld(cx, cy, 100)) return;
   int stone = ModContent.TileType<AshenStone>();
   if (!ShrinePlacementSafety.CanCarveNaturalArea(cx, cy, 10, 7, stone)) return;
   for (int dx=-10; dx<=10; dx++) for (int dy=-7; dy<=7; dy++) {
    int x=cx+dx,y=cy+dy;
    if(!WorldGen.InWorld(x,y,100))continue;
    WorldGen.KillTile(x,y,false,false,true);
    // Remove trapped lava/water/honey throughout the room, not only the chest cells.
    Main.tile[x,y].LiquidAmount=0;
    if(Math.Abs(dx)==10 || Math.Abs(dy)==7) WorldGen.PlaceTile(x,y,stone,true,true);
   }
   // PlaceChest uses bottom-left (cx-1, cy+2): chest footprint y=cy+1..cy+2,
   // and its solid support must be directly below at cy+3.
   // Clear existing cave tiles before placing the support platform. Tile placement
   // alone can fail when the pre-generated world has solid blocks in these cells.
   for(int dx=-4;dx<=4;dx++) {
    int fx=cx+dx, fy=cy+3;
    WorldGen.KillTile(fx,fy,false,false,true);
    Main.tile[fx,fy].LiquidAmount=0;
    WorldGen.PlaceTile(fx,fy,stone,true,true);
   }
   // Any leftover liquids in the chest footprint prevent vanilla chest placement.
   for(int dx=-1;dx<=0;dx++) for(int dy=1;dy<=2;dy++) {
    Main.tile[cx+dx,cy+dy].LiquidAmount=0;
   }
   int chestIndex=WorldGen.PlaceChest(cx-1,cy+2,(ushort)TileID.Containers,false,0);
   if(chestIndex>=0 && Main.chest[chestIndex]!=null) {
    Chest chest=Main.chest[chestIndex];
    chest.item[0].SetDefaults(ModContent.ItemType<AshenRelic>());
    chest.item[1].SetDefaults(ModContent.ItemType<CinderBar>()); chest.item[1].stack=6;
   }
  }));
 }
}
