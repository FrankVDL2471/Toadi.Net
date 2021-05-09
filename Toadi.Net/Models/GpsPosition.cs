using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toadi.Net.Models {
	public class GpsPosition {
		public float accuracy { get; set; }
		public DateTime datetime { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
		public int numSatellites { get; set; }
		public float raw_lat { get; set; }
		public float raw_lng { get; set; }
		public float speed { get; set; }
		public int status { get; set; }
		public bool valid { get; set; }
	}

}
