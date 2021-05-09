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



	}
}
