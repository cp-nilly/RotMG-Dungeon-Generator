using DungeonGenerator.Dungeon;

namespace DungeonGenerator.Templates.UndeadLair {
	internal class Background : MapRender {
		public override void Rasterize() {
			var tile = new DungeonTile {
				TileType = UndeadLairTemplate.ShallowWater
			};

			Rasterizer.Clear(tile);
		}
	}
}