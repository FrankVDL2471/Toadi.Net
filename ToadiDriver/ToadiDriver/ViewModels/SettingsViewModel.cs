using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToadiDriver.ViewModels {
	public class SettingsViewModel : BaseViewModel {
		public SettingsViewModel() {
			Title = "Toadi Driver - Settings";

			this.Config = Models.AppConfig.Instance;
			SaveCommand = new Command(() => {
				this.Config.Save();				
			});
		}

		public ICommand SaveCommand { get; }

		public Models.AppConfig Config { get; set; }
	}
}