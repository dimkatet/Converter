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

namespace Converter
{
    public sealed partial class CurrencyPage : Page
    {
        List<CurrencyData> currencyList;
        string currentCurrency = "USD";
        public CurrencyPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Tuple<string, List<CurrencyData>>)
            {
                (currentCurrency, currencyList) = (Tuple<string, List<CurrencyData>>)e.Parameter;
                foreach (var i in currencyList)
                {
                    if(i.CharCode == currentCurrency)
                        currencyListView.Items.Add(new ListViewItem { Name = i.Name, Status = "✓" });
                    else currencyListView.Items.Add(new ListViewItem { Name = i.Name, Status = "" });
                }
            }
            else if (e.Parameter is List<CurrencyData>)
            {
                currencyList = e.Parameter as List<CurrencyData>;
                foreach (var i in currencyList)
                    currencyListView.Items.Add(new ListViewItem { Name = i.Name, Status = "" });
            }
            base.OnNavigatedTo(e);
        }

        private void submit(object sender, RoutedEventArgs e)
        {
            if(currencyListView.SelectedItem != null)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                //System.Diagnostics.Debug.WriteLine(currencyList[currencyListView.SelectedIndex].ToString());
                rootFrame.Navigate(typeof(ConvertPage), currencyList[currencyListView.SelectedIndex]);
            }
        }
    }
}
