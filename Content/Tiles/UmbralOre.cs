using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class UmbralOre : ModTile {
 public override void SetStaticDefaults() {
  RegisterItemDrop(ModContent.ItemType<UmbralOreItem>());
  Main.tileSolid[Type] = true;
  Main.tileBlockLight[Type] = true;
  Main.tileSpelunker[Type] = true;
  Main.tileOreFinderPriority[Type] = 350;
  AddMapEntry(new Color(158, 77, 227));
  DustType = DustID.PurpleTorch;
  HitSound = SoundID.Tink;
  MineResist = 2f;
  MinPick = 55;
 }
}
