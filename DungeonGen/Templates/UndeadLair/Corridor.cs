using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.UndeadLair {
	internal class Corridor : MapCorridor {
		public override void Rasterize(Room src, Room dst, Point srcPos, Point dstPos) {
			Default(srcPos, dstPos, new DungeonTile {
				TileType = UndeadLairTemplate.BrownLines
			});
		}
	}
}