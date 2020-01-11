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
		public NeuronNetworks(Topology topology)
		{
			Topology = topology;
			Layers = new List<Layer>();

			CreateInputLayers();
			CreateHiddenLayers();
			CreateOutputLayers();
		}
		public Neuron FeedForward(List<double> inputSignals)
		{
			SendSignalsToinputNeurons(inputSignals);
			FeedForwardAllLayersAfterInput();
			if (Topology.OutputCount == 1)
			{
				return Layers.Last().Neurons[0];
			}

			return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
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

		private void SendSignalsToinputNeurons(List<double> inputSignals)
		{
			for (int i = 0; i < inputSignals.Count; i++)
			{
				var signal = new List<double>() { inputSignals[i] };
				var neuron = Layers.First().Neurons[i];
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
			for (int j = 0; j < Topology.HiddenLayers.Count; j++)
			{
				var hiddenNeurons = new List<Neuron>();
				var lastLayer = Layers.Last();
				for (int i = 0; i < Topology.HiddenLayers[j]; i++)
				{
					var neuron = new Neuron(lastLayer.Count);
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
				var neuron = new Neuron(lastLayer.Count, NeuronType.Output);
				outputNeurons.Add(neuron);
			}
			var outputLayer = new Layer(outputNeurons, NeuronType.Input);
			Layers.Add(outputLayer);
		}
		
	}
}
