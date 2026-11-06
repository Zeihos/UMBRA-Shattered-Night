using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Systems;
namespace UmbraShatteredNight.Content.Biomes;
public class AstralRift : ModBiome {
 public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
 public override bool IsBiomeActive(Player player) {
  if (UmbraWorld.RiftX <= 0 || UmbraWorld.RiftY <= 0) return false;
  Vector2 center = new Vector2(UmbraWorld.RiftX*16f, UmbraWorld.RiftY*16f);
  return Vector2.DistanceSquared(player.Center,center) < 1000f*1000f;
 }
}
