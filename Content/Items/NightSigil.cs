using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UmbraShatteredNight.Content.NPCs;
namespace UmbraShatteredNight.Content.Items;
public class NightSigil : ModItem
{
    public override void SetDefaults() {
        Item.width = 28; Item.height = 32;
        Item.maxStack = 20; Item.consumable = true;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 45; Item.useTime = 45;
        Item.UseSound = SoundID.Roar;
        Item.rare = ItemRarityID.Green;
    }
    public override bool CanUseItem(Player player) => !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<EclipseSentinel>());
    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer) {
            int npcType = ModContent.NPCType<EclipseSentinel>();
            if (Main.netMode != NetmodeID.MultiplayerClient) NPC.SpawnOnPlayer(player.whoAmI, npcType);
            else NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: npcType);
        }
        return true;
    }
    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<VoidShard>(12)
        .AddIngredient(ItemID.FallenStar, 2)
        .AddTile(TileID.DemonAltar).Register();
}
