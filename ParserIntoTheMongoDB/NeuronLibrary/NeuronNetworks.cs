using NeuronLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuronLibrary
{
	public class NeuronNetworks
	{
		public Topology Topology { get; }
		public List<Layer> Layers { get; }
		/**
		 <summary>Constructor of NeuronNetworks</summary>
		 <param name="topology">DEscription of neuron networks</param>
		 */
		public NeuronNetworks(Topology topology)
		{
			Topology = topology;
			Layers = new List<Layer>();

			CreateInputLayers();
			CreateHiddenLayers();
			CreateOutputLayers();
		}
		public Neuron FeedForward(params double[] inputSignals)
		{
			if (inputSignals == null || inputSignals.Length != Topology.InputCount) {
				throw new ArgumentException();
			}
			SendSignalsToInputNeurons(inputSignals);
			FeedForwardAllLayersAfterInput();
			if (Topology.OutputCount == 1)
			{
				return Layers.Last().Neurons.First();
			}
			return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
		}
		public double Learning(List<Tuple<double, double[]>> dataset, int epoch) {
			var error = 0.0;
			for (int i = 0; i < epoch; i++)
			{
				foreach (var data in dataset)
				{
					error += BackPropagation(data.Item1, data.Item2);
				}
			}
			return error / epoch;
		}
		private double BackPropagation(double expected, params double[] inputs) {
			var actual = FeedForward(inputs).Output;
			var difference = actual - expected;
			foreach (var neuron in Layers.Last().Neurons)
			{
				neuron.Learning(difference, Topology.LearningRate);
			}
			for (int i = Layers.Count - 2; i >= 0; i--)
			{
				var layer = Layers[i];
				var prewLayer = Layers[i + 1];
				for (int j = 0; j < layer.NeuronsCount; j++)
				{
					var neuron = layer.Neurons[i];
					for (int k = 0; k < prewLayer.NeuronsCount; k++)
					{
						var prewNeuron = prewLayer.Neurons[k];
						var error = prewNeuron.Weights[i] * prewNeuron.Delta;
						neuron.Learning(error, Topology.LearningRate);
					}
				}
			}
			return difference * difference;
		}
		private void FeedForwardAllLayersAfterInput()
		{
			for (int i = 1; i < Layers.Count; i++)
			{
				var layer = Layers[i];
				var previousLayersSignals = Layers[i - 1].GetSignals();
				foreach (var neuron in layer.Neurons)
				{
					neuron.FeedForward(previousLayersSignals);
				}
			}
		}
		private void SendSignalsToInputNeurons(params double[] inputSignals)
		{
			for (int i = 0; i < inputSignals.Length; i++)
			{
				var signal = new List<double>() { inputSignals[i] };
				var neuron = Layers[0].Neurons[i];
				neuron.FeedForward(signal);
			}
		}
		private void CreateInputLayers()
		{
			var inputNeurons = new List<Neuron>();
			for (int i = 0; i < Topology.InputCount; i++)
			{
				var neuron = new Neuron(1, NeuronType.Input);
				inputNeurons.Add(neuron);
			}
			var inputLayer = new Layer(inputNeurons, NeuronType.Input);
			Layers.Add(inputLayer);
		}
		private void CreateHiddenLayers()
		{
			for (int i = 0; i < Topology.HiddenLayers.Count; i++)
			{
				var hiddenNeurons = new List<Neuron>();
				var lastLayer = Layers.Last();
				for (int j = 0; j < Topology.HiddenLayers[i]; j++)
				{
					var neuron = new Neuron(lastLayer.NeuronsCount);
					hiddenNeurons.Add(neuron);
				}
				var hiddenLayer = new Layer(hiddenNeurons);
				Layers.Add(hiddenLayer);
			}
		}
		private void CreateOutputLayers()
		{
			var outputNeurons = new List<Neuron>();
			var lastLayer = Layers.Last();
			for (int i = 0; i < Topology.OutputCount; i++)
			{
				var neuron = new Neuron(lastLayer.NeuronsCount, NeuronType.Output);
				outputNeurons.Add(neuron);
			}
			var outputLayer = new Layer(outputNeurons, NeuronType.Output);
			Layers.Add(outputLayer);
		}
		
	}
}
