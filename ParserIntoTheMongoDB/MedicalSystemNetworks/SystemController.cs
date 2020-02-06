using NeuronLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedicalSystemNetworks {
	public class SystemController {
		public NeuronNetworks NeuronNetworks { get; }
		public NeuronNetworks ImageNetwork { get; }

		public SystemController() {
			var dataTopology = new Topology(14, 1, 0.1, 7);
			NeuronNetworks = new NeuronNetworks(dataTopology);

			var iamgeTopology = new Topology(400, 1, 0.1, 200);
			ImageNetwork = new NeuronNetworks(iamgeTopology);
		}
	}
}
