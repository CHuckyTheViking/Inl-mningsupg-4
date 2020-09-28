using MAD = Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Sockets;
using DotNetty.Transport.Channels.Local;

namespace SharedLibrary.Services
{
    public class DeviceService
    {

        private static readonly string _connweather = "http://api.weatherstack.com/current?access_key=e699857f79960f12fc50911e6203d374&query=koping";
        private static HttpClient _client = new HttpClient();

        private static readonly string _connect = "HostName=WIN20-iothub.azure-devices.net;DeviceId=IotDeviceUpg4;SharedAccessKey=atKNxnC4x43YsnC9vaEEga9R8P1U7iDVhweUILuMAkM=";
        public static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_connect, MAD.Client.TransportType.Mqtt);

        public static ObservableCollection<AzureMessageModel> receiveMessage = new ObservableCollection<AzureMessageModel>();
 
        public static async Task sendmessage()
        {
            await SendMessageAsync(deviceClient);
        }
        public static async Task receivemessage()
        {
            await ReceiveMessageAsync(deviceClient);
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
                    var payload = new MAD.Client.Message(Encoding.UTF8.GetBytes(json));
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

                    receiveMessage.Add(new AzureMessageModel {MessageAzure = Encoding.UTF8.GetString(payload.GetBytes())});

                    await deviceClient.CompleteAsync(payload);
                }
                catch (Exception)
                { }
            }
        }

    }
}
