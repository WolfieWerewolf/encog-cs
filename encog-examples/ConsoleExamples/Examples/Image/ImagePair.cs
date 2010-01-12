using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples.Image
{
    public class ImagePair
    {
        public String File { get; set; }
        public int Identity { get; set; }

        public ImagePair(String file, int identity)
        {
            this.File = file;
            this.Identity = identity;
        }

    }
}
