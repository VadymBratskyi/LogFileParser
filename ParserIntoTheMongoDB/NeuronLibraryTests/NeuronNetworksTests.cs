﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NeuronLibrary.Tests
{
	[TestClass()]
	public class NeuronNetworksTests
	{
		[TestMethod()]
		public void FeedForwardTest()
		{
			var topology = new Topology(4,1,2);
			var neuronNetwork = new NeuronNetworks(topology);
			neuronNetwork.Layers[1].Neurons[0].SetWeights(0.5, -0.1, 0.3, -0.1);
			neuronNetwork.Layers[1].Neurons[1].SetWeights(0.1, -0.3, 0.7, -0.3);
			neuronNetwork.Layers[2].Neurons[0].SetWeights(1.2, 0.8);

			var result = neuronNetwork.FeedForward(new List<double> { 1, 0, 0, 0 });
		}
	}
}