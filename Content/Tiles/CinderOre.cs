using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class CinderOre : ModTile {
 public override void SetStaticDefaults() {
  Main.tileLighted[Type]=true; Main.tileSolid[Type]=true; Main.tileBlockLight[Type]=true;
  Main.tileSpelunker[Type]=true; Main.tileOreFinderPriority[Type]=470;
  RegisterItemDrop(ModContent.ItemType<CinderOreItem>());
  AddMapEntry(new Color(202,92,53));
  DustType=DustID.Torch; HitSound=SoundID.Tink; MineResist=3.5f; MinPick=150;
 }
 public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
  r=0.430f; g=0.130f; b=0.025f;
 }
}
