namespace RuntimeLibrariesTests.Models.DependencyInjection
{
    internal class Car : IName
    {
        public static readonly string MyName = "Benz";

        public string Name => MyName;
    }
}
