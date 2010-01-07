using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples
{
    public interface IExampleInterface
    {
        String[] Args
        {
            get;
        }
        void WriteLine(String str);
        void Write(String str);
        void WriteLine();
        void Exit();
    }
}
