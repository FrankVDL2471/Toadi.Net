using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using ToadiDriver.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToadiDriver.Views {
	public partial class DrivePage : ContentPage {

		private bool _runLoop;

		public DrivePage() {
			InitializeComponent();

	}

		protected override void OnAppearing() {
			base.OnAppearing();

			DriveViewModel vm = this.BindingContext as DriveViewModel;
			if (vm != null) {
				vm.Connect();
			}

			_runLoop = true;
			double fpsWanted = 30; //30 frames per seconde
			var ms = 1000.0 / fpsWanted;
			var ts = TimeSpan.FromMilliseconds(ms);

			// Create a timer that triggers roughly every 1/30 seconds
			Device.StartTimer(ts, TimerLoop);
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();

			_runLoop = false;
			DriveViewModel vm = this.BindingContext as DriveViewModel;
			if (vm != null) {
				vm.Disconnect();
			}

		}



		private bool TimerLoop() {
			if (!_runLoop) return false;

			canvasView.InvalidateSurface();
			return true;

		}

		private bool _painting = false;

		//Create the event handler
		void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args) {
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;

			using (var webClient = new WebClient()) {
				var stream = webClient.DownloadData($"http://{Models.AppConfig.Instance.IpAddress}:8080/image/front/img.jpg?timestamp={DateTime.Now.Ticks}");

				using (var canvas = surface.Canvas) {
					// use KBitmap.Decode to decode the byte[] in jpeg format 
					using (var bitmap = SKBitmap.Decode(stream)) {

						using (var paint = new SKPaint()) {
							// clear the canvas / fill with black
							//canvas.DrawColor(SKColors.Black);
							canvas.DrawBitmap(bitmap, canvas.LocalClipBounds);
							//canvas.DrawBitmap(bitmap, 0, 0, paint); // SKRect.Create(640, 480)
							//canvas.DrawBitmap(bitmap, SKRect.Create(640, 480), SKRect.Create((float)canvasView.Width, (float)canvasView.Height));
						}

					}
				}
			}
 		}


	}
}