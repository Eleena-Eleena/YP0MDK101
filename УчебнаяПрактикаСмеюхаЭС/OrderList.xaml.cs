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
using System.Windows.Shapes;
using УчебнаяПрактикаСмеюхаЭС.Class;
using УчебнаяПрактикаСмеюхаЭС.DataBase;
using static System.Net.Mime.MediaTypeNames;

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        User user;
        List<Product> current;
        List<Product> currentEdit;
        public List<Product> listOrder = new List<Product>();
        public List<string> sort = new List<string>();
        int len = 0;
        bool flag;

        public OrderList(User puser)
        {
            InitializeComponent();
            user = puser;
            if (user != null)
            {
                TboxFIO.Text = user.UserSurname.Trim() + " " + user.UserName.Trim() + " " + user.UserPatronymic.Trim();
                if (user.UserRole == 3)
                {
                    SPEdit.Visibility = Visibility.Visible;
                    BtnBack.Visibility = Visibility.Hidden;
                    BtnBack1.Visibility = Visibility.Visible;
                }
                else if (user.UserRole == 2)
                {
                    BtnBack.Visibility = Visibility.Hidden;
                    BtnBack1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                TboxFIO.Text = "Гость";
            }
            current = AppData.db.Product.ToList();
            LViewOrder.ItemsSource = current;

            sort.Add("Все диапозоны");
            sort.Add("0-9,99%");
            sort.Add("10-14,99%");
            sort.Add(">15%");

            CBSort.ItemsSource = sort;
            CBSort.SelectedIndex = 0;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        /// <summary>
        /// Сортировка и поиск в реальном времени
        /// </summary>
        private void Update()
        {
            currentEdit = current;
            if (CBUpDown.IsChecked.Value)
            {
                CBUpDown.Content = "По возрастанию";
                if (CBSort.SelectedIndex == 0)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.OrderBy(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 1)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount < 10 && n.ProductMaxDiscount >= 0).OrderBy(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 2)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount < 15 && n.ProductMaxDiscount >= 10).OrderBy(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 3)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount >= 15).OrderBy(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
            }
            else
            {
                CBUpDown.Content = "По убыванию";
                if (CBSort.SelectedIndex == 0)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.OrderByDescending(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 1)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount < 10 && n.ProductMaxDiscount >= 0).OrderByDescending(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 2)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount < 15 && n.ProductMaxDiscount >= 10).OrderByDescending(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
                else if (CBSort.SelectedIndex == 3)
                {
                    currentEdit = UpdateTB(currentEdit);
                    currentEdit = currentEdit.Where(n => n.ProductMaxDiscount >= 15).OrderByDescending(n => n.ProductMaxDiscount).ToList();
                    UpdateListCount(currentEdit);
                }
            }
            LViewOrder.ItemsSource = currentEdit.ToList();
        }
        /// <summary>
        /// Счет для количества товаров на экране
        /// </summary>
        /// <param name="currentEdit"></param>
        private void UpdateListCount(List<Product> currentEdit)
        {
            string res = " из ";
            res += currentEdit.Count().ToString();
            TBlockListCount.Text = res;
        }
        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="currentEdit"></param>
        /// <returns></returns>
        private List<Product> UpdateTB(List<Product> currentEdit)
        {
            List<Product> r;
            if (TBSearch.Text != "" && TBSearch.Text != "Поиск")
            {
                r = currentEdit.Where(p => p.ProductName.ToLower().Trim().Contains(TBSearch.Text.Trim().ToLower())).ToList();
            }
            else
            {
                r = currentEdit;
            }
            return r;
        }

        private void CBUpDown_Checked(object sender, RoutedEventArgs e)
        {
            if (LViewOrder != null)
            {
                Update();
            }
        }

        private void CBUpDown_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LViewOrder != null)
            {
                Update();
            }
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LViewOrder != null)
            {
                Update();
            }
        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LViewOrder != null)
            {
                Update();
            }
        }
      
        private void LViewOrder_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContextMenuOrder.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Меню на правую кнопку мыши. Добавление товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var index = currentEdit[LViewOrder.SelectedIndex];
            if (index.Count <= index.ProductQuantityInStock)
            {
                index.Count++;
                listOrder.Add(index);

                SPListOrderCount.Visibility = Visibility.Visible;
                TBlockListOrderCount.Text = listOrder.Count.ToString();
            }
        }
        /// <summary>
        /// Меню на правую кнопку мыши. Удаление товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var index = currentEdit[LViewOrder.SelectedIndex];
            index.Count --;
            listOrder.Remove(index);
            TBlockListOrderCount.Text = listOrder.Count.ToString();
            if (listOrder.Count == 0)
            {
                SPListOrderCount.Visibility = Visibility.Collapsed;
            }
            else if (listOrder.Count > 0)
            {
                SPListOrderCount.Visibility = Visibility.Visible;
            }

        }
        /// <summary>
        /// Редактирование товара для администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!flag)
            {
                BtnEdit.IsEnabled = true;
                var ew = currentEdit[LViewOrder.SelectedIndex];
                EditOrder eo = new EditOrder(ew, user);
                this.Close();
                eo.Show();
            }
            else
            {
                BtnEdit.IsEnabled = false;
            }


        }

        private void BtnBack1_Click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu(user);
            this.Close();
            m.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderClient ol = new OrderClient(listOrder,user);
            this.Close();
            ol.Show();
        }
    }
}
