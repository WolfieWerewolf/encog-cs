using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Banchmark;
using ConsoleExamples.Examples;

namespace Encog.Examples.Benchmark
{
    public class EncogBenchmarkExample : IExample, IStatusReportable
    {
        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(EncogBenchmarkExample),
                    "benchmark",
                    "Perform an Encog benchmark.",
                    "Return a number to show how fast Encog executes on this machine.  The lower the number, the better.");
                return info;
            }
        }

        public void Report(int total, int current, String message)
        {
            Console.WriteLine(current + " of " + total + ":" + message);

        }

        public void Execute(IExampleInterface app)
        {
            EncogBenchmark mark = new EncogBenchmark(this);
            Console.WriteLine("Benchmark result: " + mark.Process());
        }
    }
}
