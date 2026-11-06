using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.Biomes;

namespace UmbraShatteredNight.Content.Items;

// Found in the Astral Shrine treasure chest during world generation.
public class AstralReliquaryCharm : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += 0.03f;
        if (player.InModBiome<AstralRift>())
        {
            player.statDefense += 4;
            player.moveSpeed += 0.08f;
        }
    }
}
