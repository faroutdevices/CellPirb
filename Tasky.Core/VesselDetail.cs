using System;

namespace Epirb.Core {

	public class VesselDetail {
		public VesselDetail ()
		{
		}

        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public string Concat { get; set; }
	}
}