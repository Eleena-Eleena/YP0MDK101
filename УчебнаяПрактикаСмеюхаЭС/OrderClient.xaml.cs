using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using УчебнаяПрактикаСмеюхаЭС.Class;
using УчебнаяПрактикаСмеюхаЭС.DataBase;

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для OrderClient.xaml
    /// </summary>
    public partial class OrderClient : Window
    {
       
        public User user;
        public List<Product> current;
        List<Product> result;
        public List<Product> listOrder = new List<Product>();
        List<string> addresses = new List<string>();
        string selectedPP;
        decimal price = 0;
        decimal priced = 0;

        public OrderClient(List<Product> products, User puser)
        {
            user = puser;
            listOrder = products;
            InitializeComponent();
            Update();
            var adr = AppData.db.OrderPickupPoint.ToList();
            foreach (var item in adr)
            {
                string address = item.OrderPickupPointNumber + " " + item.OrderPickupPointTown + " " + item.OrderPickupPointSteet + " " + item.OrderPickupPointHouse;
                addresses.Add(address);
            }
            CBoxPV.ItemsSource = addresses;
            if (user != null)
            {
                TBlockFIO.Text = user.UserSurname.Trim() + " " + user.UserName.Trim() + " " + user.UserPatronymic.Trim();
            }
            else
            {
                TBlockFIO.Text = "Гость";
            }
        }
        /// <summary>
        /// Обновление товара и стоимости
        /// </summary>
        public void Update()
        {
            LViewOrder.ItemsSource = listOrder.GroupBy(u => u.ProductArticleNumber);
            decimal price1 = 0;
            decimal priced1 = 0;
            foreach (var item in listOrder)
            {
                price1 += item.PriceItem;
                priced1 += item.PriceItemD;
            }
            price = price1;
            priced = priced1;

            TBlockAllPrice.Text = price.ToString();
            TBlockAllPriceD.Text = priced.ToString();
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OrderList co = new OrderList(user);
            this.Close();
            co.Show();
        }

        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {
            CheckOrder co = new CheckOrder(listOrder, user, selectedPP,price,priced);
            this.Close();
            co.Show();
        }

        private void CBoxPV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxPV.SelectedItem != null)
            {
                selectedPP = (string)CBoxPV.SelectedItem;
            }
        }
        /// <summary>
        /// Добавление товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is IEnumerable)
            {
                IEnumerable<Product> testValues = (IEnumerable<Product>)cmd.DataContext;
                result = testValues.ToList();
                if(result.Count > 0)
                {
                    result[0].Count--;
                    listOrder.Remove(result[0]);
                    Update();
                }
            }
        }
        /// <summary>
        /// Удаление товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;

            if (cmd.DataContext is IEnumerable)
            {
                IEnumerable<Product> testValues = (IEnumerable<Product>)cmd.DataContext;
                result = testValues.ToList();
                if (result.Count > 0)
                {
                    if (result.Count < result[0].ProductQuantityInStock)
                    {
                        result[0].Count++;
                        listOrder.Add(result[0]);
                        Update();
                    }
                }

            }
        }
    }
}
