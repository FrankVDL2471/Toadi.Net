using System;
using System.Threading.Tasks;
using Toadi.Net;

namespace ToadiCLI {
	class Program {
		static async Task Main(string[] args) {
			Console.WriteLine("Toadi CLI v1.0");

			Console.Write("Enter Toadi IP address : ");
			string ipAddress = Console.ReadLine();

			Toadi.Net.Toadi toadi = new Toadi.Net.Toadi(ipAddress);
			
			while(true) {

				var key = Console.ReadKey();
				if (key.Key == ConsoleKey.Q) break;


				decimal rotation = 0;
				if (key.Key == ConsoleKey.LeftArrow) {
					rotation = 0.50m;
				} else if (key.Key == ConsoleKey.RightArrow) {
					rotation = -0.50m;
				}


				if (key.Key == ConsoleKey.UpArrow) {
					Console.WriteLine("Up");
					await toadi.Forward(0.2m, 0.25m, rotation);
				} else if (key.Key == ConsoleKey.DownArrow) {
					Console.WriteLine("Down");
					await toadi.Backward(0.2m, 0.25m,rotation);
				} else if (rotation != 0) {
					Console.WriteLine($"Spin {rotation}");
					await toadi.Spin(rotation * 30);
				}
			}
		}
	}
}
