namespace Minimal.API
{
    public record Person(string FullName);
    public sealed class MapPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public static bool TryParse(string? value, out MapPoint? result)
        {
            try
            {
                var split = value?.Split(",").Select(double.Parse).ToArray();
                result = new MapPoint()
                {
                    X = split![0],
                    Y = split[1]
                };
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }   
    }
}
