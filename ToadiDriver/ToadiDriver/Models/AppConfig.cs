using Xamarin.Essentials;

namespace ToadiDriver.Models {
	public class AppConfig {


		private static AppConfig _instance;
		public static AppConfig Instance {
			get {
				if (_instance == null) {
					_instance = new AppConfig();
					_instance.Load();
				}
				return _instance;
			}
		}


		public string IpAddress { get; set; }

		public double SpinSpeed { get; set; }

		public double SpinRotation { get; set; }

		public double DriveSpeed { get; set; }

		public double DriveDistance { get; set; }


		private void Load() {
			try {
			this.IpAddress = Preferences.Get("IpAddress", "127.0.0.1");
			this.SpinSpeed = double.Parse(Preferences.Get("SpinSpeed", "0.14"));
			this.SpinRotation = double.Parse(Preferences.Get("SpinRotation", "2"));
			this.DriveSpeed = double.Parse(Preferences.Get("DriveSpeed", "0.28"));
			this.DriveDistance = double.Parse(Preferences.Get("DriveDistance", "1.0"));
			} catch {

			}
		}

		public void Save() {
			Preferences.Set("IpAddress", this.IpAddress ?? "127.0.0.1");
			Preferences.Set("SpinSpeed", this.SpinSpeed.ToString());
			Preferences.Set("SpinRotation", this.SpinRotation.ToString());
			Preferences.Set("DriveSpeed", this.DriveSpeed.ToString());
			Preferences.Set("DriveDistance", this.DriveDistance.ToString());
		}

	}
}
