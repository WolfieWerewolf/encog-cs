﻿// Encog Artificial Intelligence Framework v2.x
// DotNet Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2009, Heaton Research Inc., and individual contributors.
// See the copyright.txt in the distribution for a full listing of 
// individual contributors.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Repository;
using log4net;
using System.Reflection;
using log4net.Layout;
using log4net.Appender;

namespace Encog.Util.Logging
{
    /// <summary>
    /// A simple class used to quickly configure the log4j package that Encog uses.
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Get the assembly's repository.  From here we will perform much of the configuration.
        /// </summary>
        /// <returns>The assembly repository.</returns>
        public static ILoggerRepository GetRootRepository()
        {
            ILoggerRepository result = LogManager.GetRepository(Assembly.GetCallingAssembly());
            return result;
        }

        /// <summary>
        /// Start logging to the console.
        /// </summary>
        public static void StartConsoleLogging()
        {
            ILoggerRepository repository = GetRootRepository();

            // Create the layout
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = PatternLayout.DetailConversionPattern;
            layout.ActivateOptions();

            // Create the appender
            ConsoleAppender appender = new ConsoleAppender();
            appender.Layout = layout;
            appender.ActivateOptions();

            // Now use it on the root repository
            IBasicRepositoryConfigurator configurableRepository = repository as IBasicRepositoryConfigurator;
            if (configurableRepository != null)
            {
                configurableRepository.Configure(appender);
            }
        }

        /// <summary>
        /// Stop logging to the console.
        /// </summary>
        public static void StopConsoleLogging()
        {
            GetRootRepository().ResetConfiguration();
        }
    }
}
