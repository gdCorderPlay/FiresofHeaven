using TrueSync;
namespace Battle.Logic
{
    public static class MathHelper
    {
        public static FP SoldierDistanceV2(SoldierData a1, SoldierData a2)
        {
            return DistanceV2(a1.x, a1.y, a2.x, a2.y);
        }
        public static FP DistanceV2(FP x1, FP y1, FP x2, FP y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
        public static FP Distance(FP x1, FP y1, FP x2, FP y2)
        {
            return TSMath.Sqrt( (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}

