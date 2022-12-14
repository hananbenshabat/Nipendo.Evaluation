using System;

namespace Nipendo.Evaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Insurance Rating System Starting...");

            var engine = new RatingEngine();
            engine.Rate();

            if (engine.Rating > 0)
            {
                Console.WriteLine($"Rating: {engine.Rating}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No rating produced.");
            }

        }
    }
}
