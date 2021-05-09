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
			this.SpinSpeed = Preferences.Get("SpinSpeed", 14) / 100.00;
			this.SpinRotation = Preferences.Get("SpinRotation", 4000) / 100.00;
			this.DriveSpeed = Preferences.Get("DriveSpeed", 28) / 100.00;
			this.DriveDistance = Preferences.Get("DriveDistance", 100) / 100.00;
			} catch {

			}
		}

		public void Save() {
			Preferences.Set("IpAddress", this.IpAddress ?? "127.0.0.1");
			Preferences.Set("SpinSpeed", (int)(this.SpinSpeed  * 100));
			Preferences.Set("SpinRotation", (int)(this.SpinRotation * 100));
			Preferences.Set("DriveSpeed", (int)(this.DriveSpeed * 100));
			Preferences.Set("DriveDistance", (int)(this.DriveDistance * 100));

			this.Load();
		}

	}
}
