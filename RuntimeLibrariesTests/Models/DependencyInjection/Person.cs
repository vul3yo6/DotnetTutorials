namespace RuntimeLibrariesTests.Models.DependencyInjection
{
    internal class Person : IName
    {
        public static readonly string MyName = "Bill";

        public string Name => MyName;
    }
}
