using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;

namespace Encog.Examples
{
    public interface IExample
    {
        void Execute( IExampleInterface app);
    }
}
