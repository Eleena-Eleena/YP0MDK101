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
using УчебнаяПрактикаСмеюхаЭС.DataBase;

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        User user;
        public Menu(User puser)
        {
            user = puser;
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void BtnList_Click(object sender, RoutedEventArgs e)
        {
            OrderList orderList = new OrderList(user);
            this.Close();
            orderList.Show();
        }
    }
}
