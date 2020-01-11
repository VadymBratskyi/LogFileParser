using System;
using System.Collections.Generic;
using System.Text;

namespace NeuronLibrary.Models
{
	public class Neuron
	{
		public List<double> Weight { get; set; }
		public NeuronType Type { get; set; }		
		public double Output { get; private set; }
		public Neuron(int inputCount, NeuronType type = NeuronType.Normal) {
			Type = type;
			Weight = new List<double>();

			for (int i = 0; i < inputCount; i++)
			{
				Weight.Add(1);
			}
		}

		/**
		 *check input signal ***???
		 */
		public double FeedForward(List<double> inputs) {
			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
			{
				sum = +inputs[i] * Weight[i];
			}
			Output = Sigmoid(sum);


			return sum;
		}

		private double Sigmoid(double x) {
			return 1.0 / (1.0 + Math.Pow(Math.E, -x));
		}

		public override string ToString()
		{
			return Output.ToString();
		}

	}
}
