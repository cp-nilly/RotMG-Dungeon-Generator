using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.DeathArena
{
	internal class NormalRoom : Room {
		readonly int _w;
		readonly int _h;

		public NormalRoom(int w, int h) {
			_w = w;
			_h = h;
		}

		public override RoomType Type { get { return RoomType.Normal; } }

		public override int Width { get { return _w; } }

		public override int Height { get { return _h; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			rasterizer.FillRect(Bounds, new DungeonTile {
				TileType = DeathArenaTemplate.LightSand
			});
		}
	}
}