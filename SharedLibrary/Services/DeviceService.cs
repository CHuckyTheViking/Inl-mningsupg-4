using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public class DeviceService
    {
        private static readonly string _connweather = "http://api.weatherstack.com/current?access_key=e699857f79960f12fc50911e6203d374&query=koping";
        private static HttpClient _client = new HttpClient();

        private static readonly string _connect = "HostName=WIN20-iothub.azure-devices.net;DeviceId=IotDeviceUpg4;SharedAccessKey=atKNxnC4x43YsnC9vaEEga9R8P1U7iDVhweUILuMAkM=";
        public static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_connect, TransportType.Mqtt);

        public static ObservableCollection<AzureMessageModel> receiveMessage = new ObservableCollection<AzureMessageModel>();

        public static void sendmessage()
        {
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();
        }
        public static async Task receivemessage()
        {
            await DeviceService.ReceiveMessageAsync(deviceClient);
        }

        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            try
            {
                var response = await _client.GetAsync(_connweather);
                if (response.IsSuccessStatusCode)
                {
                    WeatherDataModel weather = JsonConvert.DeserializeObject<WeatherDataModel>(await response.Content.ReadAsStringAsync());

                    var data = new Current
                    {
                        temperature = weather.current.temperature,
                        humidity = weather.current.humidity
                    };

                    var json = JsonConvert.SerializeObject(data);

                    var payload = new Message(Encoding.UTF8.GetBytes(json));
                    await deviceClient.SendEventAsync(payload);

                }

            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {

            while (true)
            {
                try
                {
                    var payload = await deviceClient.ReceiveAsync();
                    if (payload == null)
                        continue;

                    var azm = new AzureMessageModel { MessageAzure = Encoding.UTF8.GetString(payload.GetBytes()) };

                    receiveMessage.Add(azm);

                    await deviceClient.CompleteAsync(payload);

                }
                catch (Exception)
                {
                    throw;
                }

            }

        }
    }
}
