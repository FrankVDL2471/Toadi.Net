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
			Title = "Toadi Driver";

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



		private double _distance = 0;
		public double Distance {
			get { return _distance; }
			set {
				_distance = value;
				//CaluculateJoyStick();
				//Console.WriteLine("Distance : " + _distance);
			}
		}


		private double _angle = 0;
		public double Angle {
			get { return _angle;  }
			set {
				_angle = value;
				//CaluculateJoyStick();
				//Console.WriteLine("Angle : " + _angle);
			}
		}


		public bool ShowGrass { get; set; }

		public bool ShowScene { get; set; }


		private bool _manualMode = false;
		public bool Connect() {
			if (string.IsNullOrEmpty(this.Config?.IpAddress)) return false;
			_toadi = new Toadi.Net.Toadi(this.Config.IpAddress);

			_manualMode = true;
			_ = _toadi.StartManualDriving();

			Device.StartTimer(TimeSpan.FromMilliseconds(750), this.TimerLoop);

			return true;
		}

		public void Disconnect() {
			_manualMode = false;
			_ = _toadi.StopManualDriving();
		}


		public  Task<byte[]> GetImage() {
			return _toadi.GetImage();
		}

		private bool TimerLoop() {
			if (!_manualMode) return false;

			this.CaluculateJoyStick();
			return true;

		}


		private double _prevDistance = 0;
		public void CaluculateJoyStick() {
			if (this.Distance == 0) {
				if (_prevDistance > 0) {
					_toadi.Stop();
					_prevDistance = 0;
				}
				return;
			}
			_prevDistance = this.Distance;

			double powerPercentage = _distance / 100;
			double speed = this.Config.DriveSpeed / powerPercentage;
			double rotation = this.Config.SpinRotation / powerPercentage;

			
			if ((_angle > 360 - 45) || (_angle < 45)) {
				Console.WriteLine($"Drive Forward : speed={speed}");
				_toadi.Forward(this.Config.DriveDistance, speed);
			} else if ((_angle > 90 - 45) && (_angle < 90 + 45)) {
				//spin right
				Console.WriteLine($"Turn right : speed={speed}");
				_toadi.Spin(this.Config.SpinRotation * -1, speed);
			} else if ((_angle > 180 - 45) && (_angle < 180 + 45)) {
				Console.WriteLine($"Drive Backward : speed={speed}");
				_toadi.Backward(this.Config.DriveDistance, speed);
			} else {
				//spin left
				Console.WriteLine($"Turn Left : speed={speed}");
				_toadi.Spin(this.Config.SpinRotation, speed);
			}



			//drive and turn in one command does not seem to work anymore
			//	var requestedTurnAngle = Math.Abs(toadiDirection);

			//	var straightTurnRadiusPart = 30; //degrees
			//	var minTurnRadius = 0.1;
			//	var maxTurnRadius = 2;
			//	var turnRadiusRange = 90 - straightTurnRadiusPart; //degrees
			//						if(requestedTurnAngle<straightTurnRadiusPart) {
			//								minTurnRadius = 2;
			//								maxTurnRadius = 5;
			//								turnRadiusRange = straightTurnRadiusPart;
			//						}

			//						else {
			//	requestedTurnAngle -= straightTurnRadiusPart;
			//}

			//var turnPercentage = requestedTurnAngle / turnRadiusRange;
			//var turnRadius = (minTurnRadius * turnPercentage) + maxTurnRadius * (1 - turnPercentage);
			//if (toadiDirection < 0) {
			//	//right turn
			//	if (!backwards) {
			//		turnRadius *= -1;
			//	}
			//} else {
			//	//left turn
			//	if (backwards) {
			//		turnRadius *= -1;
			//	}
			//}

			//if (backwards) {
			//		Console.WriteLine($"Backward {this.Config.DriveDistance}, speed={speed}, turn={turnRadius}");
			//		//_toadi.Backward(this.Config.DriveDistance, speed, turnRadius);
			//	} else {
			//		Console.WriteLine($"Forward {this.Config.DriveDistance}, speed={speed}, turn={turnRadius}");
			//		//_toadi.Forward(this.Config.DriveDistance, speed, turnRadius, turnRadius);
			//	}
		}


	}
}