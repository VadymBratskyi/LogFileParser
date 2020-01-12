using System.Collections.Generic;

namespace NeuronLibrary
{
	/**
	 <summary>
		Includ description about neuron networks
	 </summary>
	 */
	public class Topology
	{
		public int InputCount { get; }
		public int OutputCount { get; }
		public double LearningRate { get; }
		public List<int> HiddenLayers { get; }
		/**
		 <summary>
		 Constructor of topology
		 </summary>
		 <param name="inputCount">Count enters into neuron networks. First layer input items</param>
		 <param name="outputCount">Count outputs</param>
		 <param name="layers">Count neurons in hidden layer</param>
		 */
		public Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
		{
			InputCount = inputCount;
			OutputCount = outputCount;
			LearningRate = learningRate;
			HiddenLayers = new List<int>(layers);
		}
	}
}
