using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Examples;
using System.Reflection;

namespace ConsoleExamples.Examples
{
    public class ExampleInfo
    {
        public String Command { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Type ExampleType { get; set; }

        public IList<ExampleArgument> Arguments 
        {
            get
            {
                return this.args;
            }
        }

        private IList<ExampleArgument> args = new List<ExampleArgument>();

        public ExampleInfo(Type type, String command, String title, String description)
        {
            this.ExampleType = type;
            this.Command = command;
            this.Title = title;
            this.Description = description;
        }

        public IExample CreateInstance()
        {
            IExample result = (IExample)Assembly.GetExecutingAssembly().CreateInstance(this.ExampleType.FullName);
            return result;
        }

    }
}
