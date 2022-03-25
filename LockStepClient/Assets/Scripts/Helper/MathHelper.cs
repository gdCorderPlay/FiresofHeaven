namespace Battle.Logic
{
    public static class MathHelper
    {
        public static float SoldierDistanceV2(SoldierData a1, SoldierData a2)
        {
            return DistanceV2(a1.x, a1.y, a2.x, a2.y);
        }
        public static float DistanceV2(float x1, float y1, float x2, float y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return UnityEngine.Mathf.Sqrt( (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}

