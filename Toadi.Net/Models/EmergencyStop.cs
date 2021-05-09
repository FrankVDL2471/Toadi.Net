using System;
using System.Collections.Generic;
using System.Text;

namespace Toadi.Net.Models {
	public class EmergencyStop {

		private string _description;

		public string description {
			get { return _description; }
			set {
				_description = value;

				try {
					string[] parts = _description.Split('|');
					foreach(string part in parts) {
						string[] flds = part.Split(':');
						if (flds[0].Equals("Source")) {
							this.Source = flds[1];
						} else if (flds[0].Equals("Sensors")) {
							this.Sensors = flds[1];
						} else if (flds[0].Equals("Alarms")) {
							this.Alarms = flds[1];
						}

					}
				} catch {

				}

			}
		}

		public string Source { get; set; }

		public string Sensors { get; set; }

		public string Alarms { get; set; }
	}

}
