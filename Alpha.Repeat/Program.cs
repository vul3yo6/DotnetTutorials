using Alpha.Repeat.Models;

namespace Alpha.Repeat
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
				Console.WriteLine("---APP START---");

                App.Instance.Run(args);
            }
			catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
            finally
            {
                Console.WriteLine("---APP END---");
            }
        }
    }
}