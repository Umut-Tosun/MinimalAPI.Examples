namespace Minimal.API
{
    public static class Example
    {
        public  static string SomeMethod()
        {
            return "this is a coming from somemetohd";
        }
    }
    public sealed class GuidGenerator
    {
        public Guid NewGuid => new Guid();
    }
}
