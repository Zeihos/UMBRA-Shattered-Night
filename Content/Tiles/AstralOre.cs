using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class AstralOre : ModTile {
 public override void SetStaticDefaults() {
  Main.tileLighted[Type]=true; Main.tileSolid[Type]=true; Main.tileBlockLight[Type]=true;
  Main.tileSpelunker[Type]=true; Main.tileOreFinderPriority[Type]=455;
  RegisterItemDrop(ModContent.ItemType<AstralOreItem>());
  AddMapEntry(new Color(176,135,255)); DustType=DustID.PurpleTorch;
  HitSound=SoundID.Tink; MineResist=3.2f; MinPick=110;
 }
 public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
  r=0.200f; g=0.230f; b=0.450f;
 }
}
