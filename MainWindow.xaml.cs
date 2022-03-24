using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double price = 1;
        public int number = 1;
        public double price_per_item = 1;
        public bool colorPaper = false;
        public double grammage = 1;
        public int deliveryPrice = 0;
        public string seletcted_color;
        public bool colorPrint = false;
        public bool twoSidedPrint = false;
        public bool laserUV = false;

        public MainWindow()
        {
            InitializeComponent();
            slider.Value = 5;
        }

        private void quantity(object sender, TextChangedEventArgs e)
        {
            bool success = int.TryParse(q.Text, out number);
            if (!success)
            {
                MessageBox.Show("Podaj liczbę!");
                q.Text = 1.ToString();
                return;
            }
            calculate();
            uTextbox();
        }

        private void slide(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)slider.Value;
            price_per_item = 20;

            for (int i = 5; i >= 0; i--)
            {
                if (i == 5 && value == 5)
                {
                    price_per_item = 20;
                    format.Content = "A5 - cena " + price_per_item + "gr/szt.";
                    break;
                }
                else if (value == i)
                {
                    format.Content = "A" + value + " - cena " + Math.Round(price_per_item, MidpointRounding.AwayFromZero) + "gr/szt.";
                    break;
                }
                price_per_item = price_per_item * 2.5;
            }
            calculate();
            uTextbox();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void colors(object sender, SelectionChangedEventArgs e)
        {
            seletcted_color = color.SelectedValue.ToString();
            uTextbox();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zamówienie zostało przyjęte!");

            q.Text = 1.ToString();

            slider.Value = 5;

            papier.IsChecked = false;

            order.Text = String.Empty;

            foreach (UIElement control in gramatura.Children)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    RadioButton txtBox = (RadioButton)control;
                    if (txtBox.IsChecked == true)
                    {
                        txtBox.IsChecked = false;
                    }
                }
            }

            foreach (UIElement control in druk.Children)
            {
                if (control.GetType() == typeof(CheckBox))
                {
                    CheckBox txtBox = (CheckBox)control;
                    if (txtBox.IsChecked == true)
                    {
                        txtBox.IsChecked = false;
                    }
                }
            }

            foreach (UIElement control in termin.Children)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    RadioButton txtBox = (RadioButton)control;
                    if (txtBox.IsChecked == true)
                    {
                        txtBox.IsChecked = false;
                    }
                }
            }

        }

        private void papier_Checked(object sender, RoutedEventArgs e)
        {
            if (papier.IsChecked == true)
            {
                color.IsEnabled = true;
                colorPaper = true;
            }
            else
            {
                color.IsEnabled = false;
                colorPaper = false;
            }

            calculate();
            uTextbox();
        }

        private void gramatura_Checked(object sender, RoutedEventArgs e)
        {
            if (gramatura1.IsChecked == true) grammage = 1;
            else if (gramatura2.IsChecked == true) grammage = 2;
            else if (gramatura3.IsChecked == true) grammage = 2.5;
            else if (gramatura4.IsChecked == true) grammage = 3;

            calculate();
            uTextbox();
        }

        private void print(object sender, RoutedEventArgs e)
        {

            if (in_color.IsChecked == true) colorPrint = true;
            else colorPrint = false;
            if (two_sided.IsChecked == true) twoSidedPrint = true;
            else twoSidedPrint = false;
            if (UV.IsChecked == true) laserUV = true;
            else laserUV = false;

            calculate();
            uTextbox();
        }

        private void delivery(object sender, RoutedEventArgs e)
        {
            if (ekspres.IsChecked == true) deliveryPrice = 15;
            if (standard.IsChecked == true) deliveryPrice = 0;

            calculate();
            uTextbox();
        }

        public void uTextbox()
        {
            int discount = number / 100;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Przedmiot zamówienia: {0} szt., format A{1}", number, slider.Value);

            if (colorPaper == true) sb.Append(", kolor: " + seletcted_color);

            foreach (UIElement control in gramatura.Children)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    RadioButton txtBox = (RadioButton)control;
                    if (txtBox.IsChecked == true)
                    {
                        sb.Append(", " + txtBox.Tag.ToString() + " ");
                    }
                }
            }

            if (colorPrint == true) sb.Append(", druk kolor");
            if (twoSidedPrint == true) sb.Append(", druk dwustronny");
            if (laserUV == true) sb.Append(", lakier Uv");
            if (ekspres.IsChecked == true) sb.Append(", przesyłka ekspres");

            calculate();

            sb.Append("\nCena przed rabatem: " + Math.Round(price, 2, MidpointRounding.AwayFromZero) + "zł");

            if (discount > 10) discount = 10;

            price = price * (100 - discount) / 100;

            sb.AppendFormat("\nNaliczony rabat: {0}%\nCena po rabacie: {1}zł", discount, Math.Round(price, 2, MidpointRounding.AwayFromZero));

            order.Text = sb.ToString();
        }

        public void calculate()
        {
            double in_c = 0.3;
            double two_s = 0.5;
            double laser = 0.2;
            double optionPrice;

            price = number * price_per_item * grammage / 100;

            if (colorPaper == true)
            {
                optionPrice = price * 0.5;
                price = (price + optionPrice);
            }
            if (colorPrint == true)
            {
                optionPrice = price * in_c;
                price = (price + optionPrice);
            }
            if (twoSidedPrint == true)
            {
                optionPrice = price * two_s;
                price = (price + optionPrice);
            }
            if (laserUV == true)
            {
                optionPrice = price * laser;
                price = (price + optionPrice);
            }

            price = price + deliveryPrice;

        }
    }
}
