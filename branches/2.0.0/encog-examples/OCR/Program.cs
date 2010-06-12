// Introduction to Neural Networks for C#, 2nd Edition
// Copyright 2008 by Heaton Research, Inc. 
// http://www.heatonresearch.com/online/introduction-neural-networks-cs-edition-2
//         
// ISBN13: 978-1-60439-009-4  	 
// ISBN:   1-60439-009-3
//   
// This class is released under the:
// GNU Lesser General Public License (LGPL)
// http://www.gnu.org/copyleft/lesser.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Chapter12OCR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OCRForm());
        }
    }
}
