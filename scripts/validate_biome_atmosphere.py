#!/usr/bin/env python3
"""Static regression checks for cosmetic-only biome ambience (not an in-game test)."""
from pathlib import Path
root = Path(__file__).resolve().parents[1]
s = (root / 'Content/Systems/UMBRABiomeAtmosphere.cs').read_text(encoding='utf-8')
for token in (
    'class UMBRABiomeAtmosphere : ModPlayer',
    'override void PostUpdate()',
    'Main.dedServ',
    'Player.whoAmI != Main.myPlayer',
    'Player.InModBiome<FracturedAbyss>()',
    'Player.InModBiome<AstralRift>()',
    'Player.InModBiome<AshenWastes>()',
    'Main.rand.NextBool(18)',
    'WorldGen.InWorld(',
    'Main.tileSolid[tile.TileType]',
    'tile.LiquidAmount > 0',
    'Dust.NewDust(',
    'Lighting.AddLight(',
):
    assert token in s, f'Missing ambient requirement: {token}'
for banned in ('Projectile.NewProjectile', 'NPC.NewNPC', 'NetMessage.SendData',
               'WorldGen.PlaceTile', 'WorldGen.KillTile', 'Player.AddBuff'):
    assert banned not in s, f'Cosmetic system must not alter gameplay: {banned}'
import re
metadata = (root / 'build.txt').read_text(encoding='utf-8')
match = re.search(r'version\s*=\s*(\d+)\.(\d+)\.(\d+)', metadata)
assert match and tuple(map(int, match.groups())) >= (0, 67, 0), 'Unexpected build.txt version'
print('UMBRA ambience source guards: OK')
