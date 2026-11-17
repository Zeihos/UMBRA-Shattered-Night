#!/usr/bin/env python3
"""Static event spawn and sprite checks. Does not simulate Terraria."""
import re
from pathlib import Path
root=Path(__file__).resolve().parents[1]
event=(root/'Content/Systems/NightInvasionSystem.cs').read_text(encoding='utf-8')
beacon=(root/'Content/Items/NightInvasionBeacon.cs').read_text(encoding='utf-8')
cinder=(root/'Content/Projectiles/ColossusCinder.cs').read_text(encoding='utf-8')
abyss=(root/'Content/Systems/AbyssWorldgen.cs').read_text(encoding='utf-8')
for token in ('TryFindOpenSpawn(', 'IsOpenSpawnArea(', 'WorldGen.InWorld(left, top, 10)',
              'Main.tileSolid[tile.TileType]', 'tile.LiquidAmount > 96',
              'spawnTimer = 90;', 'player.ZoneOverworldHeight', 'SpawnInvasionNPC('):
    assert token in event, f'Missing event spawn guard: {token}'
assert 'player.ZoneOverworldHeight' in beacon
for name, code, path in (
 ('NightInvasionBeacon', beacon,root/'Content/Items/NightInvasionBeacon.png'),
 ('ColossusCinder',cinder,root/'Content/Projectiles/ColossusCinder.png')):
    assert path.is_file(), f'Missing original {name} sprite'
    assert 'Terraria/Images/' not in code, f'{name} uses vanilla sprite'
assert 'ShrinePlacementSafety.TryFindCarvableSite(' in abyss
assert 'chestIndex>=0 && Main.chest[chestIndex]!=null' in abyss
assert re.search(r'^version\s*=\s*\d+\.\d+\.\d+\s*$',(root/'build.txt').read_text(encoding='utf-8'),re.M)
print('Spawn, chest and sprite checks passed')
