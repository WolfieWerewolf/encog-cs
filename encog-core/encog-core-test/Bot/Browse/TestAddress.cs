// Encog(tm) Artificial Intelligence Framework v2.5
// .Net Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-java/
// 
// Copyright 2008-2010 by Heaton Research Inc.
// 
// Released under the LGPL.
//
// This is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation; either version 2.1 of
// the License, or (at your option) any later version.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this software; if not, write to the Free
// Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301 USA, or see the FSF site: http://www.fsf.org.
// 
// Encog and Heaton Research are Trademarks of Heaton Research, Inc.
// For information on Heaton Research trademarks, visit:
// 
// http://www.heatonresearch.com/copyright.html

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Encog.Bot.Browse
{
    [TestClass]
    public class TestAddress
    {
        [TestMethod]
        public void TestAddressFunctions()
        {
            const string a = "http://www.httprecipes.com/";
            var address = new Address(new Uri(a));
            address.ToString();
            Assert.AreEqual(address.Original, a);
            Assert.AreEqual(address.Url.Host, "www.httprecipes.com");
            var address2 = new Address(new Uri(a), a);
            Assert.AreEqual(address2.Original, a);
            var address3 = new Address(null, a);
            Assert.AreEqual(address3.Original, a);
        }
    }
}