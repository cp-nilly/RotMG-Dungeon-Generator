using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.Arena
{
    class StartRoom : Room
    {
        private readonly int _radius;

        public StartRoom(int radius)
        {
            _radius = radius;
        }

        public override RoomType Type { get { return RoomType.Start; } }

        public override int Width { get { return _radius * 2 + 1; } }

        public override int Height { get { return _radius * 2 + 1; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            var lightSand = new DungeonTile
            {
                TileType = ArenaTemplate.LightSand
            };
            var darkSand = new DungeonTile
            {
                TileType = ArenaTemplate.LightSand
            };

            var cX = Pos.X + _radius + 0.5;
            var cY = Pos.Y + _radius + 0.5;
            var bounds = Bounds;
            var r2 = _radius * _radius;
            var buf = rasterizer.Bitmap;

            var pR = rand.NextDouble()*(_radius - 2);
            var pX = (int)(cX + Math.Cos(pR) * pR);
            var pY = (int)(cY + Math.Sin(pR) * pR);

            for (int x = bounds.X; x < bounds.MaxX; x++)
                for (int y = bounds.Y; y < bounds.MaxY; y++)
                {
                    var a = (int)(2 * rand.NextDouble());
                    if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2 + 2 * a * _radius + a * a)
                    {
                        var r = rand.NextDouble();
                        if (r > .70)
                            buf[x, y] = darkSand;
                        else
                            buf[x, y] = lightSand;

                        if (rand.NextDouble() > 0.98)
                        {
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = ArenaTemplate.PalmTree
                            };
                        }
                    }
                    if (x == pX && y == pY)
                    {
                        buf[x, y].Region = "Spawn";
                    }
                }
        }
    }
}
