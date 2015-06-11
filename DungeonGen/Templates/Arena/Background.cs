using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonGenerator.Dungeon;
using DungeonGenerator.Templates.PirateCave;

namespace DungeonGenerator.Templates.Arena	
{ 
    internal class Background : MapRender {
		public override void Rasterize() {
			var tile = new DungeonTile {
				TileType = ArenaTemplate.ShallowWater
			};

			Rasterizer.Clear(tile);
		}
	}
}
