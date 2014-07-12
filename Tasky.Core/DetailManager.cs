using System;
using System.Collections.Generic;

namespace Epirb.Core {

	public static class DetailManager {

		static DetailManager ()
		{
		}
		
		public static Detail GetDetail(int id)
		{
			return DetailRepositoryADO.GetDetail(id);
		}
		
		public static IList<Detail> GetDetails ()
		{
			return new List<Detail>(DetailRepositoryADO.GetDetails());
		}
		
		public static int SaveDetail (Detail item)
		{
			return DetailRepositoryADO.SaveDetail(item);
		}

	}
}