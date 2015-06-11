using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.UndeadLair
{
	internal class BossRoom : Room {
		readonly int radius;

		public BossRoom(int radius) {
			this.radius = radius;
		}

		public override RoomType Type { get { return RoomType.Target; } }

		public override int Width { get { return radius * 2 + 1; } }

		public override int Height { get { return radius * 2 + 1; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			var tile = new DungeonTile {
				TileType = UndeadLairTemplate.BrownLines
			};

			var cX = Pos.X + radius + 0.5;
			var cY = Pos.Y + radius + 0.5;
			var bounds = Bounds;
			var r2 = radius * radius;
			var buf = rasterizer.Bitmap;

			for (int x = bounds.X; x < bounds.MaxX; x++)
				for (int y = bounds.Y; y < bounds.MaxY; y++) {
					if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2)
						buf[x, y] = tile;
				}

			int numKing = 1;
			int numBoss = new Range(4, 7).Random(rand);
			int numMinion = new Range(4, 7).Random(rand);

			r2 = (radius - 2) * (radius - 2);
			while (numKing > 0 || numBoss > 0 || numMinion > 0) {
				int x = rand.Next(bounds.X, bounds.MaxX);
				int y = rand.Next(bounds.Y, bounds.MaxY);

				if ((x - cX) * (x - cX) + (y - cY) * (y - cY) > r2)
					continue;

				if (buf[x, y].Object != null || buf[x, y].TileType != UndeadLairTemplate.BrownLines)
					continue;

				switch (rand.Next(3)) {
					case 0:
						if (numKing > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.PirateKing
							};
							numKing--;
						}
						break;
					case 1:
						if (numBoss > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.Boss[rand.Next(UndeadLairTemplate.Boss.Length)]
							};
							numBoss--;
						}
						break;
					case 2:
						if (numMinion > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.Minion[rand.Next(UndeadLairTemplate.Minion.Length)]
							};
							numMinion--;
						}
						break;
				}
			}
		}
	}
}