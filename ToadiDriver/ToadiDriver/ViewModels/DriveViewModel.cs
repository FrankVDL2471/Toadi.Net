using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToadiDriver.ViewModels {
	public class DriveViewModel : BaseViewModel {


		private Toadi.Net.Toadi _toadi;

		public DriveViewModel() {
			Title = "Drive";

			this.Config = Models.AppConfig.Instance;

			this.CmdForward = new Command(() => _toadi.Forward(this.Config.DriveDistance, this.Config.DriveSpeed));
			this.CmdBackward = new Command(() => _toadi.Backward(this.Config.DriveDistance, this.Config.DriveSpeed));
			this.CmdLeft = new Command(() => _toadi.Spin(this.Config.SpinRotation, this.Config.SpinSpeed));
			this.CmdRight = new Command(() => _toadi.Spin(this.Config.SpinRotation * -1.00, this.Config.SpinSpeed));

			this.CmdDock = new Command(async () => {
				await _toadi.StopManualDriving();
				await Task.Delay(500);
				await _toadi.StartDocking();
			});


		}


		public Models.AppConfig Config { get; set; }


		public ImageSource Camera { get; set; }
		public string CameraUrl { get; set; }

		public ICommand CmdForward { get; }
		public ICommand CmdBackward { get; }
		public ICommand CmdLeft { get; }
		public ICommand CmdRight { get; }
		public ICommand CmdDock { get; set; }



		private bool _showImage = false;

		public bool Connect() {
			if (string.IsNullOrEmpty(this.Config?.IpAddress)) return false;
			_toadi = new Toadi.Net.Toadi(this.Config.IpAddress);


			_ = _toadi.StartManualDriving();
			_showImage = true;
			//			_ = this.UpdateImage();

			double fpsWanted = 30; //30 frames per seconde
			var ms = 1000.0 / fpsWanted;
			var ts = TimeSpan.FromMilliseconds(ms);

			// Create a timer that triggers roughly every 1/30 seconds
			//Device.StartTimer(ts, TimerLoop);


			return true;
		}

		public void Disconnect() {
			_ = _toadi.StopManualDriving();
			_showImage = false;
		}


		public  Task<byte[]> GetImage() {
			return _toadi.GetImage();
		}


		private bool TimerLoop() {
			if (!_showImage) return false;



			_ = Xamarin.Forms.Device.InvokeOnMainThreadAsync(async () => {


				try {
					byte[] data = await _toadi.GetImage();
					MemoryStream ms = new MemoryStream(data);

					this.Camera = ImageSource.FromStream(() => ms);
					base.OnPropertyChanged(nameof(Camera));
				} catch (Exception ex) {

				}


				//this.CameraUrl = $"http://{this.Config.IpAddress}:8080/image/front/img.jpg?timestamp={DateTime.Now.Ticks}";
				//this.Camera = ImageSource.FromUri(new Uri(this.CameraUrl));
				//base.OnPropertyChanged(nameof(Camera));

				//this.Image = ImageSource.FromFile("dr_noprofile.png");
			});


			return true;

		}



		private async Task<bool> UpdateImage() {
			while(_showImage) {

				this.CameraUrl = $"http://{this.Config.IpAddress}:8080/image/front/img.jpg?timestamp={DateTime.Now.Ticks}";
				this.Camera = ImageSource.FromUri(new Uri(this.CameraUrl));

				base.OnPropertyChanged(nameof(Camera));
				//base.OnPropertyChanged(nameof(CameraUrl));

				//try {
				//	byte[] data = await _toadi.GetImage();
				//	MemoryStream ms = new MemoryStream(data);
				//	this.Camera = ImageSource.FromStream(() => ms);
				//	base.OnPropertyChanged(nameof(Camera));
				//} catch (Exception ex) {

				//}

				await Task.Delay(250);
			}
			return true;
		}

	}
}