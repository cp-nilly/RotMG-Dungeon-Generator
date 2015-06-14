using DungeonGenerator.Dungeon;
using RotMG.Common;

namespace DungeonGenerator.Templates.DeathArena
{
    class DeathArenaTemplate : DungeonTemplate
    {
        internal static readonly TileType LightSand = new TileType(0x00bd, "Light Sand");
        internal static readonly TileType DarkSand = new TileType(0x00be, "Dark Sand");
        internal static readonly ObjectType PalmTree = new ObjectType(0x018e, "Palm Tree");
        internal static readonly TileType ShallowWater = new TileType(0x0073, "Shallow Water");

        public override int MaxDepth { get { return 1; } }
        
        private NormDist _targetDepth;
        public override NormDist TargetDepth { get { return _targetDepth; } }

        private NormDist _specialRmCount;
        public override NormDist SpecialRmCount { get { return _specialRmCount; } }

        private NormDist _specialRmDepthDist;
        public override NormDist SpecialRmDepthDist { get { return _specialRmDepthDist; } }
        
        public override int CorridorWidth { get { return 20; } }
        
        public override Range RoomSeparation { get { return new Range(1, 2); } }

        public override void Initialize()
        {
            _targetDepth = new NormDist(1, 1, 1, 1, Rand.Next());
            _specialRmCount = new NormDist(1, 3, 3, 3, Rand.Next());
            _specialRmDepthDist = new NormDist(1, 1, 1, 1, Rand.Next());
        }

        public override Room CreateStart(int depth)
        {
            return new StartRoom(20);
        }

        public override Room CreateTarget(int depth, Room prev)
        {
            return new BossRoom(Rand.Next(5, 12));
        }

        public override Room CreateSpecial(int depth, Room prev)
        {
            return new BossRoom(Rand.Next(5, 12));
        }

        public override Room CreateNormal(int depth, Room prev)
        {
            return new NormalRoom(Rand.Next(3, 8), Rand.Next(3, 8));
        }

        public override MapCorridor CreateCorridor()
        {
            return new Corridor();
        }

        public override MapRender CreateBackground()
        {
            return new Background();
        }
    }
}
