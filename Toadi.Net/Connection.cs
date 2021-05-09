using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Toadi.Net {
	internal class Connection {

		private readonly HttpClient _client;

		public Connection(string ipAddress, int port) {
			_client = new HttpClient();
			_client.BaseAddress = new Uri($"http://{ipAddress}:{port}");
		}

		public async Task<bool> Send(string url) {
			var resp = await _client.GetAsync(url);
			return resp.IsSuccessStatusCode;
		}

		public async Task<T> Get<T>(string url) {
			var resp = await  _client.GetAsync(url);
			if (resp.IsSuccessStatusCode) {
				var data = await resp.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(data);
			}
			return default(T);
		}

		public async Task<byte[]> GetData(string url) {
			var resp = await _client.GetAsync(url);
			if (resp.IsSuccessStatusCode) {
				return await resp.Content.ReadAsByteArrayAsync();
			}
			return null;
		}


		public async Task<T> Post<T>(string url, object body) {
			string json = JsonConvert.SerializeObject(body);

			HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var resp = await _client.PostAsync(url, content);
			if (resp.IsSuccessStatusCode) {
				var data = await resp.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(data);
			}
			return default(T);
		}

		public async Task<T> Put<T>(string url, object body) {
			string json = JsonConvert.SerializeObject(body);

			HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var resp = await _client.PostAsync(url, content);
			if (resp.IsSuccessStatusCode) {
				var data = await resp.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(data);
			}
			return default(T);
		}



	}
}
