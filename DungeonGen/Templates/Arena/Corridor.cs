using DungeonGenerator.Dungeon;
using RotMG.Common;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Arena
{
    internal class Corridor : MapCorridor
    {
        public override void Rasterize(Room src, Room dst, Point srcPos, Point dstPos)
        {
            var buf = Rasterizer.Bitmap;
            var lightSand = new DungeonTile
            {
                TileType = ArenaTemplate.LightSand
            };
            var shallowWater = new DungeonTile
            {
                TileType = ArenaTemplate.ShallowWater
            };

            Rect rect;
            if (srcPos.X == dstPos.X)
            {
                if (srcPos.Y > dstPos.Y)
                    Utils.Swap(ref srcPos, ref dstPos);
                rect = new Rect(srcPos.X, srcPos.Y, srcPos.X + Graph.Template.CorridorWidth, dstPos.Y);
                
            }
            else if (srcPos.Y == dstPos.Y)
            {
                if (srcPos.X > dstPos.X)
                    Utils.Swap(ref srcPos, ref dstPos);
                rect = new Rect(srcPos.X, srcPos.Y, dstPos.X, srcPos.Y + Graph.Template.CorridorWidth);
            }
            else
                return;

            for (var i = rect.X; i < rect.MaxX; i++)
                for (var j = rect.Y; j < rect.MaxY; j++)
                    buf[i, j] = (Rand.NextDouble() > .75) ? shallowWater : lightSand;
        }
    }
}
