using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.UndeadLair
{
	internal class StartRoom : Room {
		readonly int radius;

		public StartRoom(int radius) {
			this.radius = radius;
		}

		public override RoomType Type { get { return RoomType.Start; } }

		public override int Width { get { return radius * 2 + 1; } }

		public override int Height { get { return radius * 2 + 1; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var tile = new DungeonTile {
				TileType = UndeadLairTemplate.LightSand
			};

			var cX = Pos.X + radius + 0.5;
			var cY = Pos.Y + radius + 0.5;
			var bounds = Bounds;
			var r2 = radius * radius;
			var buf = rasterizer.Bitmap;

			double pR = rand.NextDouble() * (radius - 2), pA = rand.NextDouble() * 2 * Math.PI;
			int pX = (int)(cX + Math.Cos(pR) * pR);
			int pY = (int)(cY + Math.Sin(pR) * pR);

			for (int x = bounds.X; x < bounds.MaxX; x++)
				for (int y = bounds.Y; y < bounds.MaxY; y++) {
					if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2) {
						buf[x, y] = tile;
						if (rand.NextDouble() > 0.95) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.PalmTree
							};
						}
					}
					if (x == pX && y == pY) {
						buf[x, y].Region = "Spawn";
						buf[x, y].Object = new DungeonObject {
							ObjectType = UndeadLairTemplate.CowardicePortal
						};
					}
				}
		}
	}
}