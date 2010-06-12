using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog;
using Encog.Util.Banchmark;

namespace Benchmark
{
    class Program : IStatusReportable
    {
        public void Report(int total, int current, String message) {
		Console.WriteLine( current + " of " + total + ":"+message);
		
	}

        public double run()
        {
            EncogBenchmark mark = new EncogBenchmark(this);
            return mark.Process();
        }

        static void Main(string[] args)
        {
            Program b = new Program();
		    Console.WriteLine("Benchmark result: " + b.run() );
        }
    }
}
