using NeuronLibrary;
using System;
using System.Collections.Generic;

namespace NeuraNetworks
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello Neuro Network!");
			Console.WriteLine("Human is sick-1");
			Console.WriteLine("Human is helthi-0");
			//Result-- Human is sick - 1
			//			Human is helthi - 0


			//Temprache T
			//Good year   A
			//Smoking S
			//Food helthi F

			//var dataset = new List<Tuple<double, double[]>> { 
			////												T	A	S	F
			//	new Tuple<double, double[]>(0, new double[] { 0, 0, 0, 0}),
			//	new Tuple<double, double[]>(0, new double[] { 0, 0, 0, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 0, 0, 1, 0}),
			//	new Tuple<double, double[]>(0, new double[] { 0, 0, 1, 1}),
			//	new Tuple<double, double[]>(0, new double[] { 0, 1, 0, 0}),
			//	new Tuple<double, double[]>(0, new double[] { 0, 1, 0, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 0, 1, 1, 0}),
			//	new Tuple<double, double[]>(0, new double[] { 0, 1, 1, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 0, 0, 0}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 0, 0, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 0, 1, 0}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 0, 1, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 1, 0, 0}),
			//	new Tuple<double, double[]>(0, new double[] { 1, 1, 0, 1}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 1, 1, 0}),
			//	new Tuple<double, double[]>(1, new double[] { 1, 1, 1, 1}),
			//};

			var outputs = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };
			var inputs = new double[,] {
				{ 0, 0, 0, 0},
				{ 0, 0, 0, 1},
				{ 0, 0, 1, 0},
				{ 0, 0, 1, 1},
				{ 0, 1, 0, 0},
				{ 0, 1, 0, 1},
				{ 0, 1, 1, 0},
				{ 0, 1, 1, 1},
				{ 1, 0, 0, 0},
				{ 1, 0, 0, 1},
				{ 1, 0, 1, 0},
				{ 1, 0, 1, 1},
				{ 1, 1, 0, 0},
				{ 1, 1, 0, 1},
				{ 1, 1, 1, 0},
				{ 1, 1, 1, 1}
			};

			var topology = new Topology(4, 1, 0.1, 2);
			var neuronNetwork = new NeuronNetworks(topology);
			//var difference = neuronNetwork.Learning(dataset, 100000);
			var difference = neuronNetwork.Learning(outputs, inputs, 100000);

			var result = new List<double>();
			for (int i = 0; i < outputs.Length; i++)
			{
				var row = NeuronNetworks.GetRow(inputs, i);
				result.Add(neuronNetwork.FeedForward(row).Output);
			}
			//foreach (var data in dataset)
			//{
			//	result.Add(neuronNetwork.FeedForward(data).Output);
			//}

			for (int i = 0; i < result.Count; i++)
			{
				var expecte = Math.Round(outputs[i], 3);
				var actual = Math.Round(result[i], 3);
				Console.WriteLine(expecte + " -- " + actual);
			}
			Console.ReadKey();
		}
	}
}
