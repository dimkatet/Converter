using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
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
    public sealed partial class ConvertPage : Page
    {
        private int changedCurrency = 0;
        private CurrencyData cur1;
        private CurrencyData cur2;
        private List<CurrencyData> currencyList;
        public ConvertPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is CurrencyData)
            {
                var temp = (CurrencyData)e.Parameter;
                if (changedCurrency == 1)
                {
                    if(cur2 != null && cur1 != null && cur2.CharCode == temp.CharCode)
                    {
                        cur2 = cur1;
                        currName2.Text = cur2.CharCode;
                    }
                    cur1 = temp;
                    currName1.Text = cur1.CharCode;
                }
                else
                {
                    if (cur1 != null && cur2 != null && cur1.CharCode == temp.CharCode)
                    {
                        cur1 = cur2;
                        currName1.Text = cur1.CharCode;
                    }
                    cur2 = temp;
                    currName2.Text = cur2.CharCode;
                }
                if (cur1 != null && cur2 != null && !String.IsNullOrEmpty(value1.Text))
                {
                    var val = String.Format("{0:0.0000}", convert(float.Parse(value1.Text), "1"));
                    value2.Text = val;
                }
            }
            else if(e.Parameter is List<CurrencyData>)
            {
                currencyList = e.Parameter as List<CurrencyData>;
            }
            base.OnNavigatedTo(e);
        }

        private void chengeCurrency(object sender, RoutedEventArgs e)
        {
            changedCurrency = Int16.Parse(((Button)sender).Name.ToString().Last().ToString());
            CurrencyData currentCurrency;
            if (changedCurrency == 1)
                currentCurrency = cur1;
            else currentCurrency = cur2;
            Frame rootFrame = Window.Current.Content as Frame;
            if (currentCurrency != null)
                rootFrame.Navigate(typeof(CurrencyPage), new Tuple<string, List<CurrencyData>>(currentCurrency.CharCode, currencyList));
            else rootFrame.Navigate(typeof(CurrencyPage), currencyList);
        }

        private double convert(double x, string changingVaule)
        {
            var y1 = cur1.Value / (double)cur1.Nominal;
            var y2 = cur2.Value / (double)cur2.Nominal; ;
            return changingVaule == "1" ? x * y1 / y2 : x * y2 / y1;
        }


        private void onChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if(sender.FocusState == FocusState.Unfocused)
                return;
            float f;
            if (float.TryParse(args.NewText, out f) && cur1 != null && cur2 != null && (args.NewText.Length <= 10 || args.NewText.Length < sender.Text.Length))
            {
                var val = String.Format("{0:0.0000}", convert(float.Parse(args.NewText), sender.Tag.ToString()));
                if (sender.Tag.ToString() == "1")
                    value2.Text = val;
                else value1.Text = val;
            }
            else if (String.IsNullOrEmpty(args.NewText)) 
            {
                value1.Text = "";
                value2.Text = "";
            }
            else args.Cancel = true;
        }

        private void swapCurrency(object sender, RoutedEventArgs e)
        {
            CurrencyData tempCur = cur1;
            cur1 = cur2;
            cur2 = tempCur;
            string tempVal = value1.Text;
            value1.Text = value2.Text;
            value2.Text = tempVal;
            string tempCurCode = currName1.Text;
            currName1.Text = currName2.Text;
            currName2.Text = tempCurCode;
        }
    }
}
