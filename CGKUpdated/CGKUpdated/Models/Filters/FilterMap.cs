namespace CGKUpdated.Models.Filters
{
    public static class FilterMap
    {
        public static Dictionary<string, Filter> filters = new Dictionary<string, Filter>
        {
            {"Even RGB Values", new RGBEven() },
            {"Shift RGB Values Left", new RGBLShift() },
            {"Shift RGB Values Right", new RGBRShift() },
            {"Nearest Neighbour Pixel Sort", new NearestNeighbour() }
        };
    }
}
