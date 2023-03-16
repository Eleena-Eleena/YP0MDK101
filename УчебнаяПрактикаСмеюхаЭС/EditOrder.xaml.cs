using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
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

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        Product product;
        User user;
        public EditOrder(Product pproduct, User puser)
        {
            user = puser;
            product = pproduct;
            InitializeComponent();
            Update();
        }
        /// <summary>
        /// Вывод информации в TextBox для редактирования
        /// </summary>
        public void Update()
        {
            TboxName.Text = product.ProductName;
            TboxDisc.Text = product.ProductDescription;
            TboxCost.Text = product.ProductCost.ToString();
            TboxDiscoint.Text = product.ProductMaxDiscount.ToString();
            TboxStock.Text = product.ProductQuantityInStock.ToString();
            CBoxCategory.ItemsSource = AppData.db.Category.ToList();
            CBoxMan.ItemsSource = AppData.db.ProductManufacturer.ToList();
            CBoxProv.ItemsSource = AppData.db.Provider.ToList();
            CBoxType.ItemsSource = AppData.db.ProductUnit.ToList();

            int productCategory = product.ProductCategory - 1;
            CBoxCategory.SelectedIndex = productCategory;
            int productM = product.ProductManufacturer - 1;
            CBoxMan.SelectedIndex = productM;
            int productP = product.ProductProvider - 1;
            CBoxProv.SelectedIndex = productP;
            int productT = product.ProductUnit - 1;
            CBoxType.SelectedIndex = productT;
            if (product.ProductPhoto != null)
            {
                BitmapImage bi = new BitmapImage(new Uri(product.ProductPhoto, UriKind.RelativeOrAbsolute));
                ImageAgent.Source = bi;
            }
        }
        /// <summary>
        /// Сохранение в БД измененной информации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите редактировать товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    product.ProductName = TboxName.Text.Trim();
                    product.ProductDescription = TboxDisc.Text.Trim();
                    product.ProductCost = decimal.Parse(TboxCost.Text.Trim());
                    product.ProductMaxDiscount = int.Parse(TboxDiscoint.Text.Trim());
                    product.ProductQuantityInStock = int.Parse(TboxStock.Text.Trim());
                    product.ProductCategory = CBoxCategory.SelectedIndex + 1;
                    product.ProductManufacturer = CBoxMan.SelectedIndex + 1;
                    product.ProductProvider = CBoxProv.SelectedIndex + 1;
                    product.ProductUnit = CBoxType.SelectedIndex + 1;

                    AppData.db.Product.AddOrUpdate(product);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Готово", "Уведомление");
                    OrderList mw = new OrderList(user);
                    mw.Show();
                    this.Close();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            MessageBox.Show(err.ErrorMessage + "  ");
                        }
                    }
                }
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OrderList or = new OrderList(user);
            or.Show();
            this.Close();
        }
        /// <summary>
        /// Удаление товара из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.Product.Remove(product);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Готово", "Уведомление");
                    OrderList mw = new OrderList(user);
                    mw.Show();
                    this.Close();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            MessageBox.Show(err.ErrorMessage + "  ");
                        }
                    }
                }
            }
        }
    }
  
}
