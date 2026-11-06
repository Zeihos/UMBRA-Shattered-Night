using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Items;
namespace UmbraShatteredNight.Content.Tiles;
public class AstralOre : ModTile {
 public override void SetStaticDefaults() {
  Main.tileSolid[Type]=true; Main.tileBlockLight[Type]=true;
  Main.tileSpelunker[Type]=true; Main.tileOreFinderPriority[Type]=455;
  RegisterItemDrop(ModContent.ItemType<AstralOreItem>());
  AddMapEntry(new Color(176,135,255)); DustType=DustID.PurpleTorch;
  HitSound=SoundID.Tink; MineResist=3.2f; MinPick=110;
 }
}
