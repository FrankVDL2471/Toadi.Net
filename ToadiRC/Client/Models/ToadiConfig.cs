using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToadiRC.Client.Models {
	public class ToadiConfig {

		public string IpAddress { get; set; }

		public int Port { get; set; } = 8080;

		public int DriveDistance { get; set; } = 100;

		public int DriveSpeed { get; set; } = 28;

		public int SpinSpeed { get; set; } = 14;

		public int SpinRotation { get; set; } = 50;
	}
}
