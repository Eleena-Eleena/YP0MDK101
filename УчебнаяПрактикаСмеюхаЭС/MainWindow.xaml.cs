using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
using System.Windows.Threading;
using УчебнаяПрактикаСмеюхаЭС.Class;
using УчебнаяПрактикаСмеюхаЭС.DataBase;

namespace УчебнаяПрактикаСмеюхаЭС
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        String pwd = "";
        private int i = 10;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer diTi = new DispatcherTimer();
        User CuttentGuest;
        User CuttentAdmin;
        User CuttentManager;
        User CuttentUser;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxLogin.Text.Length > 0)
            {
                if (TBoxPassword.Password.Length > 0)
                {
                    var userLOG = AppData.db.User.ToList();

                    CuttentUser = userLOG.FirstOrDefault(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim() && u.UserRole == 1);
                    CuttentAdmin = userLOG.FirstOrDefault(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim() && u.UserRole == 2);
                    CuttentManager = userLOG.FirstOrDefault(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim() && u.UserRole == 3);

                    if (CuttentUser != null && CuttentAdmin == null && CuttentManager == null)
                    {
                        OrderList orderListWin = new OrderList(CuttentUser);
                        string name = userLOG.Single(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim()).UserName;
                        this.Close();
                        orderListWin.Show();
                        MessageBox.Show("Добро пожаловать, " + name.Trim(), "Уведомление");
                    }
                    else if (CuttentUser == null && CuttentAdmin != null && CuttentManager == null)
                    {
                        Menu menuA = new Menu(CuttentAdmin);
                        string name = userLOG.Single(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim()).UserName;
                        this.Close();
                        menuA.Show();
                        MessageBox.Show("Добро пожаловать, " + name, "Уведомление");
                    }
                    else if (CuttentUser == null && CuttentAdmin == null && CuttentManager != null)
                    {
                        Menu menuM = new Menu(CuttentManager);
                        string name = userLOG.Single(u => u.UserLogin.Trim() == TBoxLogin.Text.Trim() && u.UserPassword.Trim() == TBoxPassword.Password.Trim()).UserName;
                        this.Close();
                        menuM.Show();
                        MessageBox.Show("Добро пожаловать, " + name, "Уведомление");
                    }
                    else
                    {
                        MessageBox.Show("Такого пользователя нет", "Уведомление");
                        CAPTCHA();
                    }
                    TBoxLogin.Clear();
                    TBoxPassword.Clear();
                }
                else
                {
                    TBoxLogin.Clear();
                    TBoxPassword.Clear();
                    MessageBox.Show("Введите пароль", "Уведомление");
                }
            }
            else
            {
                TBoxLogin.Clear();
                TBoxPassword.Clear();
                MessageBox.Show("Введите логин", "Уведомление");
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OrderList orderListWin = new OrderList(CuttentGuest);
            this.Close();
            orderListWin.Show();
        }

        /// <summary>
        /// Создание CAPTCHA
        /// </summary>
        private void CAPTCHA()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            pwd = "";
            string temp = "";
            Random randomSym = new Random();
            Random rA = new Random();
            for (int i = 0; i < rA.Next(5, 10); i++)
            {
                temp = ar[randomSym.Next(0, ar.Length)];
                pwd += temp;
            }
            Random randonColor = new Random();
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)randonColor.Next(1, 255), (byte)randonColor.Next(1, 255), (byte)randonColor.Next(1, 233)));
            TblockCAPTCHA.Text = pwd;
            TblockCAPTCHA.Foreground = brush;
            GridCAPCHA.Visibility = Visibility.Visible;
            SPCAPCHA.Visibility = Visibility.Visible;
            TblockCAPTCHA.TextDecorations = TextDecorations.Strikethrough;
            TBlockGuest.IsEnabled = false;
            BtnLog.IsEnabled = false;
        }

        private void timer1(object sender, EventArgs e)
        {
            if (i != 0)
            {
                i--;
            }
            else
            {
                MessageBox.Show("Время вышло.\n Повторите попытку.");
                BtnCAPCHA.IsEnabled = true;
                CAPTCHA();
                diTi.Stop();
            }
        }

        private void DisTimer_Tick1(object sender, EventArgs e)
        {
            dispatcherTimer.Tick -= new EventHandler(DisTimer_Tick1);
        }

        /// <summary>
        /// Проверка ввода CAPTCHA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCAPCHA_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxCAPTCHA.Text == pwd)
            {
                TBlockGuest.IsEnabled = true;
                BtnLog.IsEnabled = true;
                GridCAPCHA.Visibility = Visibility.Collapsed;
                SPCAPCHA.Visibility = Visibility.Collapsed;
                TBoxCAPTCHA.Clear();
                MessageBox.Show("Правильно", "Проверка");
            }
            else
            {
                MessageBox.Show("Неверно", "Проверка");
                BtnCAPCHA.IsEnabled = false;
                TBoxCAPTCHA.Clear();
                dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
                diTi.Interval = TimeSpan.FromSeconds(1);
                diTi.Tick += timer1;
                dispatcherTimer.Tick += new EventHandler(DisTimer_Tick1);
                diTi.Start();
                dispatcherTimer.Start();
            }
        }
    }
}


