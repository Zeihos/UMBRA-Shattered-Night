#!/usr/bin/env python3
"""Static links in UMBRA's boss progression (NOT an actual Terraria playthrough)."""
from pathlib import Path

root = Path(__file__).resolve().parents[1]
def read(path):
    return (root / path).read_text(encoding="utf-8")

def has(path, *needles):
    source = read(path)
    for needle in needles:
        assert needle in source, f"Progression regression in {path}: {needle}"

guide = "Content/Commands/UmbraGuideCommand.cs"
stages = [
    "DownedSentinel", "NightInvasionActive", "DownedNightInvasion",
    "DownedArchon", "DownedAstralHeart", "DownedAstralSovereign",
    "DownedAshenColossus", "DownedAshenOverlord", "DownedDevourer",
]
has(guide, 'Command => "umbraguide"', "Language.GetTextValue", *stages)

has("Content/Items/NightSigil.cs", ".AddIngredient<VoidShard>(12)", ".AddIngredient(ItemID.FallenStar, 2)")
has("Content/NPCs/NightWisp.cs", "ItemType<VoidShard>()")
has("Content/NPCs/EclipseSentinel.cs", "DownedSentinel = true", "ItemType<EclipseCore>()")
has("Content/Items/NightInvasionBeacon.cs", ".AddIngredient<EclipseCore>(1)", ".AddIngredient<VoidShard>(20)")
has("Content/Systems/NightInvasionSystem.cs", "UmbraWorld.DownedSentinel", "DownedNightInvasion = true", "ItemType<InvasionCrest>()")
has("Content/Items/ArchonBeacon.cs", ".AddIngredient<InvasionCrest>(1)", ".AddIngredient<EclipseCore>(3)", ".AddIngredient<VoidShard>(20)", "TileID.MythrilAnvil")
has("Content/Systems/BossSummoningSystem.cs", "UmbraWorld.DownedNightInvasion", "UmbraWorld.DownedAstralHeart", "UmbraWorld.DownedAshenColossus", "NPC.AnyNPCs(bossType)")
has("Content/NPCs/RiftArchon.cs", "DownedArchon = true")
has("Content/NPCs/AstralHeart.cs", "UmbraWorld.DownedArchon", "ItemType<AstralHeartFragment>()", "DownedAstralHeart = true")
has("Content/Items/SovereignBeacon.cs", ".AddIngredient<AstralHeartFragment>(10)", ".AddIngredient<AstralBar>(15)", "TileID.MythrilAnvil")
has("Content/NPCs/AstralSovereign.cs", "DownedAstralSovereign = true")
has("Content/NPCs/AshenColossus.cs", "UmbraWorld.DownedAstralSovereign", "ItemType<AshenCore>()", "DownedAshenColossus = true")
has("Content/Items/OverlordEmberSeal.cs", ".AddIngredient<AshenCore>(8)", ".AddIngredient<CinderBar>(18)", "TileID.MythrilAnvil")
has("Content/NPCs/AshenOverlord.cs", "DownedAshenOverlord = true", "ItemType<OverlordHeart>()")
has("Content/Items/FractureBeacon.cs", ".AddIngredient<VoidShard>(10)", ".AddIngredient<UmbralBar>(5)")

locale_keys = ["Title","Sentinel","Invasion","InvasionActive","Archon","AstralHeart","Sovereign","Colossus","Overlord","Completed","OptionalDevourer"]
for lang in ("ru-RU","en-US"):
    source = read(f"Localization/{lang}.hjson")
    assert "Guide: {" in source, f"Guide section missing in {lang}"
    for key in locale_keys:
        assert f"{key}:" in source, f"Guide translation {lang}.{key} missing"
assert "/umbraguide" in read("docs/PROGRESSION.md")
assert "/umbraguide" in read("README.md")
print("UMBRA boss progression, recipes, loot and two guide localizations: source-level checks passed.")
