using System;
using System.Collections.Generic;

namespace Epirb.Core {

	public static class VesselDetailManager {

		static VesselDetailManager ()
		{
		}
		
		public static VesselDetail GetVesselDetail(int id)
		{
			return VesselDetailRepositoryADO.GetVesselDetail(id);
		}
		
		public static IList<VesselDetail> GetVesselDetails ()
		{
			return new List<VesselDetail>(VesselDetailRepositoryADO.GetVesselDetails());
		}
		
		public static int SaveVesselDetail (VesselDetail item)
		{
			return VesselDetailRepositoryADO.SaveVesselDetail(item);
		}

	}
}