using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SharedLibrary.Models;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpIotDevice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public ObservableCollection<AzureMessageModel> Bodys;
        public MainPage()
        {
            this.InitializeComponent();
            Bodys = DeviceService.receiveMessage;
            DeviceService.receivemessage().GetAwaiter();
            //await gethttprequest(HttpRequest req);
        
            
        }
        private void btnsendMessage_Click(object sender, RoutedEventArgs e)
        {
            DeviceService.sendmessage().GetAwaiter();
        }
        //public static void gethttprequest(HttpRequest req)
        //{

        //    List<string> hej = new List<string>();
        //    HttpClient client = new HttpClient();

        //    client.GetAsync("http://localhost:7071/api/IotDeviceupg4");

        //    dynamic requestBody = new StreamReader(req.Body);
        //    var data = JsonConvert.DeserializeObject<BodyMessageModel>(requestBody);
        //    hej.Add(data.Message);

        //}
        //public static async Task Run(HttpRequest req)
            
        //{
        //    //string message;
        //    List<string> hej = new List<string>();
        //    HttpClient client = new HttpClient();
        //    //http://localhost:7071/api/IotDeviceupg4?targetdeviceid=IotDeviceupg4&message=Hejsan
        //    await client.GetAsync("http://localhost:7071/api/IotDeviceupg4?targetdeviceid=IotDeviceupg4&message="+ message);
        //    //http://localhost:7071/api/IotDeviceupg4
        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var data = JsonConvert.DeserializeObject<BodyMessageModel>(requestBody);
        //    hej.Add(data.Message);
        //}

    }
}
