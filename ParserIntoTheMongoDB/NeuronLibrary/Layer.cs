using NeuronLibrary.Models;
using System;
using System.Collections.Generic;

namespace NeuronLibrary
{
	public class Layer
	{
		public List<Neuron> Neurons { get; }
		public int Count => Neurons?.Count ?? 0;

		/**
		 * <summary>
		 * Constructo layer with naurons
		 * </summary>
		 * <param name="neurons">Neurons list</param>
		 * <param name="type">Type neurons. Mast be one type all neurons in layers</param>
		 */
		public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal) {
			if (neurons == null) {
				throw new ArgumentNullException();
			}
			Neurons = neurons;
		}
		/**
		 * <summary>
		 * Get output from each neuron
		 * </summary>
		 */
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
