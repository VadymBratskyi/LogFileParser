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
			//Result -- Human is sick-1
			//			Human is helthi-0

			//Temprache	T
			//Good year	A
			//Smoking	S
			//Food helthi	F

			var dataset = new List<Tuple<double, double[]>> { 
			//												T	A	S	F
				new Tuple<double, double[]>(0, new double[] { 0, 0, 0, 0}),
				new Tuple<double, double[]>(0, new double[] { 0, 0, 0, 1}),
				new Tuple<double, double[]>(1, new double[] { 0, 0, 1, 0}),
				new Tuple<double, double[]>(0, new double[] { 0, 0, 1, 1}),
				new Tuple<double, double[]>(0, new double[] { 0, 1, 0, 0}),
				new Tuple<double, double[]>(0, new double[] { 0, 1, 0, 1}),
				new Tuple<double, double[]>(1, new double[] { 0, 1, 1, 0}),
				new Tuple<double, double[]>(0, new double[] { 0, 1, 1, 1}),
				new Tuple<double, double[]>(1, new double[] { 1, 0, 0, 0}),
				new Tuple<double, double[]>(1, new double[] { 1, 0, 0, 1}),
				new Tuple<double, double[]>(1, new double[] { 1, 0, 1, 0}),
				new Tuple<double, double[]>(1, new double[] { 1, 0, 1, 1}),
				new Tuple<double, double[]>(1, new double[] { 1, 1, 0, 0}),
				new Tuple<double, double[]>(0, new double[] { 1, 1, 0, 1}),
				new Tuple<double, double[]>(1, new double[] { 1, 1, 1, 0}),
				new Tuple<double, double[]>(1, new double[] { 1, 1, 1, 1}),
			};

			var topology = new Topology(4, 1, 0.1, 2);
			var neuronNetwork = new NeuronNetworks(topology);
			var difference = neuronNetwork.Learning(dataset, 100000);

			var result = new List<double>();
			foreach (var data in dataset)
			{
				result.Add(neuronNetwork.FeedForward(data.Item2).Output);
			}
			for (int i = 0; i < result.Count; i++)
			{
				var expecte = Math.Round(dataset[i].Item1, 3);
				var actual = Math.Round(result[i], 3);
				Console.WriteLine(expecte + " -- " +actual);
			}
			Console.ReadKey();
		}
	}
}
