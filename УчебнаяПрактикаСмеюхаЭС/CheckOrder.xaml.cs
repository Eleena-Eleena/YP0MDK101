using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using УчебнаяПрактикаСмеюхаЭС.DataBase;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для CheckOrder.xaml
    /// </summary>
    public partial class CheckOrder : Window
    {
        public List<Product> listOrder = new List<Product>();
        User user;
        string selectedPP;
        decimal price;
        decimal priced;
        public CheckOrder(List<Product> plistOrder, User puser, string pselectedPP, decimal pprice, decimal ppriced)
        {
            listOrder = plistOrder;
            user = puser;
            selectedPP = pselectedPP;
            price = pprice;
            priced = ppriced;
            InitializeComponent();
            LViewOrder.ItemsSource = listOrder.GroupBy(u => u.ProductArticleNumber);
            TBoxAddress.Text = selectedPP;
            RANDOM();
            TBoxData.Text = DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();
            DataPP();
            TBlockPr.Text = price.ToString();
            TBlockDis.Text = priced.ToString();
          
            if (user != null)
            {
                TBoxFIO.Text = user.UserSurname.Trim() + " " + user.UserName.Trim() + " " + user.UserPatronymic.Trim();
            }
            else
            {
                TBoxFIO.Text = "Гость";
            }
        }
        /// <summary>
        /// Вычисление даты выдачи товара
        /// </summary>
        public void DataPP()
        {
            DateTime maxday = DateTime.Now;
            foreach (var item in listOrder)
            {
                var stock = listOrder.FirstOrDefault(u => u.ProductArticleNumber == item.ProductArticleNumber).ProductQuantityInStock;

                if (stock > 3)
                {
                    if (maxday < DateTime.Now.AddDays(3))
                    {
                        TBoxDataDel.Text = DateTime.Now.AddDays(3).ToString();
                    }
                }
                else
                {
                    if (maxday < DateTime.Now.AddDays(6))
                    {
                        TBoxDataDel.Text = DateTime.Now.AddDays(6).ToString();
                    }
                }
            }
        }

        public void RANDOM()
        {
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                TBlockNumber.Text += random.Next(0, 9);
            }
        }
        /// <summary>
        /// Сохранение талона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnSavePDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnSavePDF.Visibility = Visibility.Collapsed;
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if(printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Invoise");
                }
            }
            finally
            {
                OrderList or = new OrderList(user);
                BtnSavePDF.Visibility = Visibility.Visible;
                this.IsEnabled = true;
                MessageBox.Show("Документ сохранен");
                this.Close();
                or.Show();

            }
        }
    }
}
