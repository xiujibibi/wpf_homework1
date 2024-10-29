﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace _1015
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>
        {
            { "紅茶大杯", 60 },
            { "紅茶小杯", 40 },
            { "綠茶大杯", 50 },
            { "綠茶小杯", 30 },
            { "可樂大杯", 50 },
            { "可樂小杯", 30 },
            { "咖啡大杯", 80 },
            { "咖啡小杯", 50 }
        };
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string takeout = "";
        public MainWindow()
        {
            InitializeComponent();
            // 顯示飲料品項
            DisplayDrinkMenu(drinks);
        }
        private void DisplayDrinkMenu(Dictionary<string, int> drinks)
        {
            stackpanel_DrinkMenu.Height = 42 * drinks.Count;
            foreach (var drink in drinks)
            {
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(3),
                    Background = Brushes.LightBlue,
                    Height = 35,
                };
                var cb = new CheckBox
                {
                    Content = drink.Key,
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Blue,
                    Width = 150,
                    Margin = new Thickness(5),
                    VerticalContentAlignment = VerticalAlignment.Center,
                };
                var lb_price = new Label
                {
                    Content = $"{drink.Value}元",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Green,
                    Width = 60,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };
                var sl = new Slider
                {
                    Width = 150,
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Center,
                    IsSnapToTickEnabled = true,
                };
                var lb_amount = new Label
                {
                    Content = "0",
                    FontFamily = new FontFamily("微軟正黑體"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Red,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Width = 50,
                };
                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb_amount.SetBinding(ContentProperty, myBinding);
                sp.Children.Add(cb);
                sp.Children.Add(lb_price);
                sp.Children.Add(sl);
                sp.Children.Add(lb_amount);
                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true)
            {
                takeout = rb.Content.ToString();
            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            // 確認訂購內容
            orders.Clear();
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var drinkName = cb.Content.ToString();
                var sl = sp.Children[2] as Slider;
                var amount = (int)sl.Value;
                if (cb.IsChecked == true && amount > 0) orders.Add(drinkName, amount);
            }
            // 顯示訂購內容
            string msg = "";
            string discount_msg = "";
            int total = 0;
            msg += $"此次訂購為{takeout}，訂購內容如下：\n";
            int num = 1;
            foreach (var order in orders)
            {
                int subtotal = drinks[order.Key] * order.Value;
                msg += $"{num}. {order.Key} x {order.Value}杯，小計{subtotal}元\n";
                total += subtotal;
                num++;
            }
            msg += $"總金額為{total}元";
            int sellPrice = total;
            if (total >= 500)
            {
                sellPrice = (int)(total * 0.8);
                discount_msg = $"恭喜您獲得滿500元打8折優惠";
            }
            else if (total >= 300)
            {
                sellPrice = (int)(total * 0.9);
                discount_msg = $"恭喜您獲得滿300元打9折優惠";
            }
            else
            {
                discount_msg = $"未達到任何折扣條件";
            }
            msg += $"\n{discount_msg}，原價為{total}元，售價為 {sellPrice}元。";
            ResultTextBlock.Text = msg;
        }
    }
}