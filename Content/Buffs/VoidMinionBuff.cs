using Terraria;
using Terraria.ModLoader;
namespace UmbraShatteredNight.Content.Buffs;
public class VoidMinionBuff : ModBuff {
 public override void SetStaticDefaults() { Main.buffNoSave[Type]=true; Main.buffNoTimeDisplay[Type]=true; }
 public override void Update(Player player, ref int buffIndex) {
  if(player.ownedProjectileCounts[ModContent.ProjectileType<global::UmbraShatteredNight.Content.Projectiles.VoidMinion>()]>0) player.buffTime[buffIndex]=18000;
  else player.DelBuff(buffIndex--);
 }
}
