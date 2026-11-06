using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class FracturedStone : ModTile {
 public override void SetStaticDefaults() {
  RegisterItemDrop(ModContent.ItemType<FracturedStoneItem>());
  Main.tileSolid[Type] = true;
  Main.tileMergeDirt[Type] = true;
  Main.tileBlockLight[Type] = true;
  AddMapEntry(new Color(69, 37, 110));
  DustType = DustID.PurpleTorch;
  HitSound = SoundID.Tink;
  MineResist = 1.5f;
 }
 public override bool CanExplode(int i, int j) => true;
}
