using DungeonGenerator.Dungeon;

namespace DungeonGenerator.Templates.UndeadLair {
	internal class Overlay : MapRender {
		public override void Rasterize() {
			var wall = new DungeonTile {
				TileType = UndeadLairTemplate.Composite,
				Object = new DungeonObject {
					ObjectType = UndeadLairTemplate.CaveWall
				}
			};
			var water = new DungeonTile {
				TileType = UndeadLairTemplate.ShallowWater
			};
			var space = new DungeonTile {
				TileType = UndeadLairTemplate.Space
			};

			int w = Rasterizer.Width, h = Rasterizer.Height;
			var buf = Rasterizer.Bitmap;
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != UndeadLairTemplate.ShallowWater)
						continue;

					bool notWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						notWall = false;
					else if (buf[x + 1, y].TileType == UndeadLairTemplate.BrownLines ||
					         buf[x - 1, y].TileType == UndeadLairTemplate.BrownLines ||
					         buf[x, y + 1].TileType == UndeadLairTemplate.BrownLines ||
					         buf[x, y - 1].TileType == UndeadLairTemplate.BrownLines) {
						notWall = true;
					}
					if (!notWall)
						buf[x, y] = wall;
				}

			var tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != UndeadLairTemplate.Composite)
						continue;

					bool nearWater = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						nearWater = false;
					else if (tmp[x + 1, y].TileType == UndeadLairTemplate.ShallowWater ||
					         tmp[x - 1, y].TileType == UndeadLairTemplate.ShallowWater ||
					         tmp[x, y + 1].TileType == UndeadLairTemplate.ShallowWater ||
					         tmp[x, y - 1].TileType == UndeadLairTemplate.ShallowWater) {
						nearWater = true;
					}
					if (nearWater && Rand.NextDouble() > 0.4)
						buf[x, y] = water;
				}

			tmp = (DungeonTile[,])buf.Clone();
			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++) {
					if (buf[x, y].TileType != UndeadLairTemplate.Composite)
						continue;

					bool allWall = false;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						allWall = true;
					else {
						allWall = true;
						for (int dx = -1; dx <= 1 && allWall; dx++)
							for (int dy = -1; dy <= 1 && allWall; dy++) {
								if (tmp[x + dx, y + dy].TileType != UndeadLairTemplate.Composite) {
									allWall = false;
									break;
								}
							}
					}
					if (allWall)
						buf[x, y] = space;
				}
		}
	}
}