using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.UndeadLair {
	internal class NormalRoom : Room {
		readonly int w;
		readonly int h;

		public NormalRoom(int w, int h) {
			this.w = w;
			this.h = h;
		}

		public override RoomType Type { get { return RoomType.Normal; } }

		public override int Width { get { return w; } }

		public override int Height { get { return h; } }

		public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand) {
			rasterizer.FillRect(Bounds, new DungeonTile {
				TileType = UndeadLairTemplate.BrownLines
			});

			int numBoss = new Range(0, 1).Random(rand);
			int numMinion = new Range(3, 5).Random(rand);
			int numPet = new Range(0, 2).Random(rand);

			var buf = rasterizer.Bitmap;
			var bounds = Bounds;
			while (numBoss > 0 || numMinion > 0 || numPet > 0) {
				int x = rand.Next(bounds.X, bounds.MaxX);
				int y = rand.Next(bounds.Y, bounds.MaxY);
				if (buf[x, y].Object != null)
					continue;

				switch (rand.Next(3)) {
					case 0:
						if (numBoss > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.Boss[rand.Next(UndeadLairTemplate.Boss.Length)]
							};
							numBoss--;
						}
						break;
					case 1:
						if (numMinion > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.Minion[rand.Next(UndeadLairTemplate.Minion.Length)]
							};
							numMinion--;
						}
						break;
					case 2:
						if (numPet > 0) {
							buf[x, y].Object = new DungeonObject {
								ObjectType = UndeadLairTemplate.Pet[rand.Next(UndeadLairTemplate.Pet.Length)]
							};
							numPet--;
						}
						break;
				}
			}
		}
	}
}