using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples
{
    public class ExampleArgument
    {
        public String Name { get; set; }
        public String Description {get; set; }

        public ExampleArgument(String name, String description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
