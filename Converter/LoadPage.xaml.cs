using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace Converter
{
    sealed partial class LoadPage: Page
    {
        public LoadPage()
        {
            this.InitializeComponent();
            loadData();
        }
        private async void loadData()
        {
            var currencyList = new List<CurrencyData>();
            var client = new HttpClient();
            var resp = await client.GetAsync(new Uri("https://www.cbr-xml-daily.ru/daily_json.js"));
            var data = JsonObject.Parse(resp.Content.ToString());
            data = JsonObject.Parse(data["Valute"].ToString());
            foreach (var i in data.Values)
            {
                var temp = new CurrencyData();
                var record = JsonObject.Parse(i.ToString());
                temp.CharCode = record["CharCode"].GetString();
                temp.Name = record["Name"].GetString();
                temp.Nominal = (int)record["Nominal"].GetNumber();
                temp.Value = record["Value"].GetNumber();
                currencyList.Add(temp);
            }
            await Task.Delay(500);
            ring.IsActive = false;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ConvertPage), currencyList);
        }
    }
}
