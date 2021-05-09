using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Toadi.Net.Models;

namespace Toadi.Net {
	public class Toadi {

		private readonly Connection _conn;
		
		public Toadi(string ipAddress, int port = 8080) {
			_conn = new Connection(ipAddress, port);
		}


		public Task<bool> Stop() {
			return _conn.Send("/navitaion/stop");
		}

		public Task<bool> Spin(double rotation, double speed = 0.15, WheelLock wheelLock = WheelLock.None) {
			return _conn.Send($"/navigation/spinaround?speed={speed.ToString(CultureInfo.InvariantCulture)}&rotation={rotation.ToString(CultureInfo.InvariantCulture)}&wheelLock={wheelLock.ToString().ToLower()}&powerLimit=1.0");
		}

		public Task<bool> Forward(double distance, double speed = 0.25 , double turn = 0, double rotation = 0) {
			return _conn.Send($"/navigation/forward?speed={speed.ToString(CultureInfo.InvariantCulture)}&distance={distance.ToString(CultureInfo.InvariantCulture)}&turnRaduis={turn.ToString(CultureInfo.InvariantCulture)}&rotation={rotation.ToString(CultureInfo.InvariantCulture)}&powerLimit=1.0");
		}

		public Task<bool> Backward(double distance, double speed = 0.25, double turn = 0, double rotation = 0) {
			return _conn.Send($"/navigation/backwards?speed={speed.ToString(CultureInfo.InvariantCulture)}&distance={distance.ToString(CultureInfo.InvariantCulture)}&turnRaduis={turn.ToString(CultureInfo.InvariantCulture)}&rotation={rotation.ToString(CultureInfo.InvariantCulture)}&powerLimit=1.0");
		}

		public Task<bool> StartManualDriving() {
			return _conn.Send("/navigation/startmanualdriving");
		}
		public Task<bool> StopManualDriving() {
			return _conn.Send("/navigation/stopmanualdriving");
		}

		public Task<bool> StartDocking() {
			return _conn.Send("/navigation/startdocking");
		}
		public Task<bool> StopDocking() {
			return _conn.Send("/navigation/stopdocking");
		}


		public Task<byte[]> GetImage() {
			return _conn.GetData($"/image/front/img.jpg?timestamp={DateTime.Now.Ticks}");
		}


		public Task<GpsPosition> GetPosition() {
			return _conn.Get<GpsPosition>("/statuslog/sensors/gps");			
		}
		public void TrackPostion(Action<GpsPosition> callback, CancellationToken cancellationToken, int interval = 5000) {
			if (callback == null) return;

			_ = Task.Run(async () => {
				while (!cancellationToken.IsCancellationRequested) {
					var pos = await GetPosition();
					if (pos != null) {
						callback(pos);
					}
					await Task.Delay(5000, cancellationToken);
				}
			},cancellationToken);

		}

		public Task<EmergencyStop> GetEmergencyStop() {
			return _conn.Get<EmergencyStop>("/system/emergencyStop");
		}
		public Task<bool> ReleaseEmergencyStop() {
			return _conn.Send("/navigation/releaseEmergencyStop");
		}

		public Task<ActrivityInfo> GetActrivityInfo() {
			return _conn.Get<ActrivityInfo>("/activities/info");
		}


		public async Task<ToadiStatus > GetStatus() {
			ToadiStatus status = new ToadiStatus();


			var act = await this.GetActrivityInfo();
			if (act?.userActivity == "manualdriving") {
				status.Modus = "Manual";
			} else if (string.IsNullOrEmpty(act?.userActivity)) {
				status.Modus = "Auto";
			} else {
				status.Modus = act?.userActivity;			
			}
			if (act.scheduledActivity == "docking") {
				status.Modus = "Docking";
			}
			

			var stop = await this.GetEmergencyStop();			
			if ((string.IsNullOrEmpty(stop.description)) || (stop.description.Equals("none"))) {
				status.EmergencyLock = false;
				status.Error = string.Empty;
			} else {
				status.EmergencyLock = true;
				status.Error = stop.description;
			}

			return status;
		}


	}
}
