using System;
using System.Collections.Generic;
using System.Text;

namespace NeuronLibrary.Models
{
	public class Neuron
	{
		public List<double> Weights { get; }
		public List<double> Inputs { get; }
		public NeuronType Type { get; }
		/**
		<summary>
			neuron results
		</summary>
		**/
		public double Output { get; private set; }
		public double Delta { get; private set; }
		/**
		 *<param name="inputCount">count input neurons</param>
		 *<param name="type">type of neuron</param>
		 *<summary>
		 * neuron constructor with default weight
		 * </summary>
		 */
		public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
		{
			Type = type;
			Weights = new List<double>();
			Inputs = new List<double>();
			InitWeightsRandomValues(inputCount);
		}
		private void InitWeightsRandomValues(int inputCount)
		{
			var rand = new Random();
			for (int i = 0; i < inputCount; i++)
			{
				if (Type == NeuronType.Input)
				{
					Weights.Add(1);
				}
				else {
					Weights.Add(rand.NextDouble());
				}
				Inputs.Add(0);
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
			for (int i = 0; i < inputs.Count; i++)
			{
				Inputs[i] = inputs[i];
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
		/**
		 <summary>
			Proizvodnaya function
		 </summary>
		 */
		private double SigmoidDx(double x) {
			return Sigmoid(x) / (1 - Sigmoid(x));
		}
		/**
		 <summary>
			Calculation new weights
		 </summary>
		 */
		public void Learning(double error, double learningRate) {
			if (Type == NeuronType.Input) {
				return;
			}
			Delta = error * SigmoidDx(Output);
			for (int i = 0; i < Weights.Count; i++)
			{
				var newWeight = Weights[i] - Inputs[i] * Delta * learningRate;
				Weights[i] = newWeight;
			}
		}
		public override string ToString()
		{
			return Output.ToString();
		}

	}
}
