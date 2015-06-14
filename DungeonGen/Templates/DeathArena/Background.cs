using DungeonGenerator.Dungeon;

namespace DungeonGenerator.Templates.DeathArena	
{ 
    internal class Background : MapRender {
		public override void Rasterize() {
			var tile = new DungeonTile {
				TileType = DeathArenaTemplate.ShallowWater
			};

			Rasterizer.Clear(tile);
		}
	}
}
