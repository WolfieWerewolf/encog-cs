
 namespace Encog.ML.Genetic.Innovation {
	
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Provides the interface for an innovation. An innovation is a enhancement that
	/// was tried to the genome.
	/// </summary>
	///
	public interface IInnovation {
	
		/// <summary>
		/// Set the innovation id.
		/// </summary>
		///
		/// <value>The innovation id.</value>
		long InnovationID {
		
		/// <returns>The innovation id.</returns>
		  get;
		/// <summary>
		/// Set the innovation id.
		/// </summary>
		///
		/// <param name="innovationID">The innovation id.</param>
		  set;
		}
		
	
	}
}
