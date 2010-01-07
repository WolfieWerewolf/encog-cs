using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples
{
    public class ConsoleInterface: IExampleInterface
    {
        public ConsoleInterface(String[] args)
        {
            this.args = args;
        }

        public String[] Args
        {
            get
            {
                return this.args;
            }
        }

        private String[] args;

        public void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        public void Write(string str)
        {
            Console.Write(str);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void Exit()
        {
        }
    }
}
