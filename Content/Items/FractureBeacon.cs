using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;

namespace UmbraShatteredNight.Content.Items;
public class FractureBeacon : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 30; Item.height = 30; Item.maxStack = 20;
        Item.consumable = true; Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 40; Item.useTime = 40;
        Item.UseSound = SoundID.Roar; Item.rare = ItemRarityID.Green;
    }
    public override bool CanUseItem(Player player) =>
        player.ZoneRockLayerHeight && !NPC.AnyNPCs(ModContent.NPCType<ShardDevourer>());

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return true;
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)global::UmbraShatteredNight.UmbraShatteredNight.PacketType.RequestDevourerSummon);
            packet.Send();
        }
        else if (Main.netMode == NetmodeID.SinglePlayer)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ShardDevourer>());
        }
        return true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(10).AddIngredient<UmbralBar>(5)
        .AddTile(TileID.Anvils).Register();
}
