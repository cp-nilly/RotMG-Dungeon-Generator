using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.DeathArena
{
	internal class BossRoom : Room {
		readonly int _radius;

		public BossRoom(int radius) {
			_radius = radius;
		}

		public override RoomType Type { get { return RoomType.Target; } }

		public override int Width { get { return _radius * 2 + 1; } }

		public override int Height { get { return _radius * 2 + 1; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var tile = new DungeonTile {
				TileType = DeathArenaTemplate.LightSand
			};

			var cX = Pos.X + _radius + 0.5;
			var cY = Pos.Y + _radius + 0.5;
			var bounds = Bounds;
			var r2 = _radius * _radius;
			var buf = rasterizer.Bitmap;

			for (int x = bounds.X; x < bounds.MaxX; x++)
				for (int y = bounds.Y; y < bounds.MaxY; y++) {
                    var a = (int)(2 * rand.NextDouble());
                    if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2 + 2 * a * _radius + a * a)
						buf[x, y] = tile;
				}
		}
	}
}