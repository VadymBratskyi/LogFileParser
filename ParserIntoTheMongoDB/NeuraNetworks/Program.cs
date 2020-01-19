using NeuronLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuraNetworks
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello Neuro Network!");

			var converter = new PictureConvector();
			var inputs = converter.Convert(@"C:\GithubProject\LogFileParser\ParserIntoTheMongoDB\NeuraNetworks\images\Parasitized.png");
			converter.Save("D:\\image.png", inputs);

			//RunNeuronNetworks1();
			//RunNeuronNetworks2();
			//RunNeuronNetworks3();

			Console.ReadKey();
		}
		public static void RunNeuronNetworks3() {

			var outputs = new List<double>();
			var inputs = new List<double[]>();
			using (var str = new StreamReader(@"C:\GithubProject\LogFileParser\ParserIntoTheMongoDB\NeuraNetworks\heart.csv"))
			{
				var header = str.ReadLine();
				while (!str.EndOfStream) {
					var row = str.ReadLine();
					var values = row.Split(',').Select(v => Convert.ToDouble(v.Replace('.',','))).ToList();
					var output = values.Last();
					var input = values.Take(values.Count - 1).ToArray();

					outputs.Add(output);
					inputs.Add(input);
				}
			}

			var inputSignals = new double[inputs.Count, inputs[0].Length];
			for (int i = 0; i < inputSignals.GetLength(0); i++)
			{
				for (int j = 0; j < inputSignals.GetLength(1); j++)
				{
					inputSignals[i, j] = inputs[i][j];
				}
			}

			var topology = new Topology(outputs.Count, 1, 0.1, outputs.Count / 2);
			var neuronNetwork = new NeuronNetworks(topology);
			var difference = neuronNetwork.Learning(outputs.ToArray(), inputSignals, 10);

			var result = new List<double>();
			for (int i = 0; i < outputs.Count; i++)
			{
				result.Add(neuronNetwork.FeedForward(inputs[i]).Output);
			}
			for (int i = 0; i < result.Count; i++)
			{
				var expecte = Math.Round(outputs[i], 3);
				var actual = Math.Round(result[i], 3);
				Console.WriteLine(expecte + " -- " + actual);
			}
		}
		public static void RunNeuronNetworks2() {
			//Result-- Human is sick - 1
			//			Human is helthi - 0

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
			var difference = neuronNetwork.Learning(outputs, inputs, 100000);

			var result = new List<double>();
			for (int i = 0; i < outputs.Length; i++)
			{
				var row = NeuronNetworks.GetRow(inputs, i);
				result.Add(neuronNetwork.FeedForward(row).Output);
			}
			for (int i = 0; i < result.Count; i++)
			{
				var expecte = Math.Round(outputs[i], 3);
				var actual = Math.Round(result[i], 3);
				Console.WriteLine(expecte + " -- " + actual);
			}
		}

		public static void RunNeuronNetworks1() {
			Console.WriteLine("Human is sick-1");
			Console.WriteLine("Human is helthi-0");
			//Result-- Human is sick - 1
			//			Human is helthi - 0


			//Temprache T
			//Good year   A
			//Smoking S
			//Food helthi F

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
				Console.WriteLine(expecte + " -- " + actual);
			}
		}
	}
}
