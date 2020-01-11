using NeuronLibrary.Models;
using System.Collections.Generic;

namespace NeuronLibrary
{
	public class Layer
	{
		public List<Neuron> Neurons { get; }
		public int Count => Neurons?.Count ?? 0;

		/**
		 *check list
		 *check neuron type. mast be one types
		 */
		public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal) {
			Neurons = neurons;
		}
		public List<double> GetSignals() {
			var result = new List<double>();
			foreach (var neuron in Neurons)
			{
				result.Add(neuron.Output);
			}
			return result;
		}
	}
}
