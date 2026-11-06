using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class AshenStone : ModTile {
 public override void SetStaticDefaults() {
  Main.tileSolid[Type] = true;
  Main.tileBlockLight[Type] = true;
  Main.tileMergeDirt[Type] = true;
  RegisterItemDrop(ModContent.ItemType<AshenStoneItem>());
  AddMapEntry(new Color(109, 72, 69));
  DustType = DustID.Ash;
  HitSound = SoundID.Tink;
  MineResist = 1.5f;
 }
}
