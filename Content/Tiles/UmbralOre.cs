using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class UmbralOre : ModTile {
 public override void SetStaticDefaults() {
  RegisterItemDrop(ModContent.ItemType<UmbralOreItem>());
  Main.tileLighted[Type]=true; Main.tileSolid[Type] = true;
  Main.tileBlockLight[Type] = true;
  Main.tileSpelunker[Type] = true;
  Main.tileOreFinderPriority[Type] = 350;
  AddMapEntry(new Color(158, 77, 227));
  DustType = DustID.PurpleTorch;
  HitSound = SoundID.Tink;
  MineResist = 2f;
  MinPick = 55;
 }
 public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
  r=0.220f; g=0.050f; b=0.350f;
 }
}
