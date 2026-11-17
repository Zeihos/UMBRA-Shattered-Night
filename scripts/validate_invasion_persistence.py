#!/usr/bin/env python3
"""Source-level guard for invasion save/load protocol (not an in-game test)."""
from pathlib import Path
import re

root = Path(__file__).resolve().parents[1]
world = (root/'Content/Systems/UmbraWorld.cs').read_text(encoding='utf-8')
inv = (root/'Content/Systems/NightInvasionSystem.cs').read_text(encoding='utf-8')

# Previous versions discarded the running invasion when loading a world.
for key in ('nightInvasionActive', 'nightInvasionWave', 'nightInvasionKills'):
    assert f'tag["{key}"]' in world, f'Not saved: {key}'
    getter = 'GetBool' if key == 'nightInvasionActive' else 'GetInt'
    assert f'tag.{getter}("{key}")' in world, f'Not loaded: {key}'
assert 'if (NightInvasionActive && NightInvasionWave >= 1 && NightInvasionWave <= 4)' in world
assert '!DownedSentinel || NightInvasionWave < 1 || NightInvasionWave > 4' in world
assert 'NightInvasionKills < 0 || NightInvasionKills >= required' in world
assert 'NightInvasionWave == 4' in world
assert 'NightInvasionSystem.ResetRuntime()' in world
assert 'spawnTimer = FinalRespawnDelay;' in inv

# NetSend/NetReceive fields must retain the same order and types.
net_send = re.search(r'public override void NetSend\(BinaryWriter writer\)\s*\{(.*?)\}', world, re.S)
net_recv = re.search(r'public override void NetReceive\(BinaryReader reader\)\s*\{(.*?)\}', world, re.S)
assert net_send and net_recv, 'Missing world synchronization methods'
write_fields = re.findall(r'writer\.Write\((\w+)\)', net_send.group(1))
read_fields = re.findall(r'(\w+)\s*=\s*reader\.Read(?:Boolean|Int32)\(\)', net_recv.group(1))
assert write_fields == read_fields, 'WorldNet field order changed'
assert len(write_fields) == 17, f'Unexpected NetSend field count: {len(write_fields)}'
print('Invasion persistence and world network field ordering: OK (source-level)')
