using System;
using System.Collections.Generic;
using System.Text;

namespace NeuronLibrary.Models
{
	public class Neuron
	{
		public List<double> Weights { get; }
		public NeuronType Type { get; }		
		public double Output { get; private set; }
		public Neuron(int inputCount, NeuronType type = NeuronType.Normal) {
			Type = type;
			Weights = new List<double>();

			for (int i = 0; i < inputCount; i++)
			{
				Weights.Add(1);
			}
		}
		/**
		 *check input signal ***???
		 */
		public double FeedForward(List<double> inputs) {
			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
			{
				sum = +inputs[i] * Weights[i];
			}
			if (Type != NeuronType.Input)
			{
				Output = Sigmoid(sum);
			}
			else {
				Output = sum;
			}
			return sum;
		}
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
