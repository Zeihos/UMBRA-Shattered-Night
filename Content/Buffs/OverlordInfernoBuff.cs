using Terraria;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Projectiles;

namespace UmbraShatteredNight.Content.Buffs;

public class OverlordInfernoBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
    public override void Update(Player player, ref int buffIndex) {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<OverlordInfernoMinion>()] > 0)
            player.buffTime[buffIndex] = 18000;
        else player.DelBuff(buffIndex--);
    }
}
