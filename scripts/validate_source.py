#!/usr/bin/env python3
"""Deterministic, dependency-free structural checks for UMBRA source assets.
This does not compile Terraria/tModLoader or validate in-game behavior.
"""
from __future__ import annotations

import pathlib
import re
import struct
import sys
import zlib
from collections import Counter

ROOT = pathlib.Path(__file__).resolve().parents[1]
SRC = ROOT / 'Content'
errors: list[str] = []


def fail(message: str) -> None:
    errors.append(message)


def validate_png(path: pathlib.Path) -> None:
    data = path.read_bytes()
    if not data.startswith(b'\x89PNG\r\n\x1a\n'):
        fail(f'Invalid PNG signature: {path.relative_to(ROOT)}')
        return
    index, ended, dimensions = 8, False, None
    while index + 12 <= len(data):
        length = struct.unpack('>I', data[index:index + 4])[0]
        name = data[index + 4:index + 8]
        body = data[index + 8:index + 8 + length]
        end = index + 12 + length
        if end > len(data):
            fail(f'Truncated PNG chunk: {path.relative_to(ROOT)}')
            return
        recorded = struct.unpack('>I', data[index + 8 + length:end])[0]
        if zlib.crc32(name + body) & 0xffffffff != recorded:
            fail(f'PNG CRC mismatch: {path.relative_to(ROOT)}')
            return
        if name == b'IHDR':
            if length != 13:
                fail(f'Invalid PNG header size: {path.relative_to(ROOT)}')
                return
            dimensions = struct.unpack('>II', body[:8])
        if name == b'IEND':
            ended = True
            if end != len(data):
                fail(f'Unexpected trailing PNG data: {path.relative_to(ROOT)}')
            break
        index = end
    if not ended or dimensions is None or min(dimensions) < 1:
        fail(f'Incomplete PNG: {path.relative_to(ROOT)}')


cs_files = sorted(SRC.rglob('*.cs'))
png_files = sorted(ROOT.rglob('*.png'))
classes: dict[str, pathlib.Path] = {}
for file in cs_files:
    source = file.read_text(encoding='utf-8')
    for name in re.findall(r'\bclass\s+(\w+)\b', source):
        if name in classes:
            fail(f'Duplicate C# class {name}: {file} and {classes[name]}')
        classes[name] = file
    # Every mod content class should have its own sprite, or override Texture.
    if re.search(r'\bclass\s+\w+\s*:\s*Mod(?:Item|NPC|Projectile|Buff|Tile)\b', source):
        if not file.with_suffix('.png').exists() and not re.search(r'override\s+string\s+Texture\b', source):
            fail(f'Missing content texture: {file.relative_to(ROOT)}')
# Resolve generic targets only after collecting ALL class declarations.
for file in cs_files:
    source = file.read_text(encoding='utf-8')
    for target in re.findall(r'\bModContent\.(?:ItemType|NPCType|ProjectileType|TileType|BuffType)<(?:[A-Za-z_][\w.]*\.)?(\w+)>', source):
        if target not in classes:
            fail(f'Undefined ModContent target {target} in {file.relative_to(ROOT)}')
for file in png_files:
    validate_png(file)

for locale in ('en-US', 'ru-RU'):
    file = ROOT / 'Localization' / (locale + '.hjson')
    if not file.exists():
        fail(f'Missing locale: {locale}')
        continue
    text = file.read_text(encoding='utf-8')
    for folder in ('Items', 'NPCs', 'Projectiles', 'Tiles', 'Buffs'):
        for source in (SRC / folder).rglob('*.cs'):
            if not re.search(r'\b' + re.escape(source.stem) + r'\s*:', text):
                fail(f'Missing {locale} translation for {source.stem} ({folder})')
    for biome in (SRC / 'Biomes').glob('*.cs'):
        if not re.search(r'\b' + re.escape(biome.stem) + r'\s*:', text):
            fail(f'Missing {locale} biome translation for {biome.stem}')
    for section in ('Items', 'Biomes', 'Tiles', 'Buffs', 'NPCs', 'Projectiles'):
        count = len(re.findall(r'^\s{4}' + section + r':\s*\{', text, flags=re.M))
        if count > 1:
            fail(f'Duplicate top-level {section} section in {locale}')

build = (ROOT / 'build.txt').read_text(encoding='utf-8')
if not re.search(r'^version\s*=\s*\d+\.\d+\.\d+\s*$', build, flags=re.M):
    fail('build.txt missing version')

print(f'Checked {len(cs_files)} C# files, {len(png_files)} PNG files and two locales.')
if errors:
    for error in errors:
        print('ERROR:', error, file=sys.stderr)
    sys.exit(1)
print('Static integrity checks passed (not a tModLoader compilation).')
