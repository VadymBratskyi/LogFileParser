using System;
using System.Collections.Generic;
using System.Text;

namespace NeuronLibrary.Models
{
	public class Neuron
	{
		public List<double> Weights { get; }
		public NeuronType Type { get; }
		/**
		<summary>
			neuron results
		</summary>
		**/
		public double Output { get; private set; }

		/**
		 *<param name="inputCount">count input neurons</param>
		 *<param name="type">type of neuron</param>
		 *<summary>
		 * neuron constructor with default weight
		 * </summary>
		 */
		public Neuron(int inputCount, NeuronType type = NeuronType.Normal) {
			Type = type;
			Weights = new List<double>();
			for (int i = 0; i < inputCount; i++)
			{
				Weights.Add(1);
			}
		}
		/**
		 * <summary>
		 *Lienes neron networks
		 * </summary>
		 * <param name="inputs">List inputs signals</param>
		 */
		public double FeedForward(List<double> inputs) {
			if (inputs == null)	{
				throw new ArgumentNullException();
			}
			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
			{
				sum += inputs[i] * Weights[i];
			}
			if (Type != NeuronType.Input)
			{
				Output = Sigmoid(sum);
			}
			else
			{
				Output = sum;
			}
			return Output;
		}
		/**
		 <summary>
		 Function sigmoid
		 </summary>
		 <param name="x"></param>
		 */
		private double Sigmoid(double x) {
			return 1.0 / (1.0 + Math.Pow(Math.E, -x));
		}
		public void SetWeights(params double[] weights) {
			for (int i = 0; i < weights.Length; i++)
			{
			//delete after study networks
				Weights[i] = weights[i];
			}
		}
		public override string ToString()
		{
			return Output.ToString();
		}

	}
}
