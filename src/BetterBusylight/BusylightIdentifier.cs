namespace BetterBusylight
{
    public static class BusylightIdentifier
    {
        public static bool IsBusylight(int vendorId, int productId)
        {
            if (vendorId == 10171)
            {
                return productId == 15306 || productId == 15307 || productId == 15309;
            }
            return vendorId == 1240 && productId == 63560;
        }
    }
}
