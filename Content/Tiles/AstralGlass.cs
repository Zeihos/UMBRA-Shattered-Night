using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class AstralGlass : ModTile {
 public override void SetStaticDefaults() {
  Main.tileSolid[Type] = true;
  Main.tileBlockLight[Type] = true;
  Main.tileMergeDirt[Type] = true;
  RegisterItemDrop(ModContent.ItemType<AstralGlassItem>());
  AddMapEntry(new Color(83, 73, 151));
  DustType = DustID.PurpleTorch;
  HitSound = SoundID.Tink;
  MineResist = 1.4f;
 }
}
