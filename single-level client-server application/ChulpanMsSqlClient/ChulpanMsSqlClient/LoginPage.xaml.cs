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

namespace ChulpanMsSqlClient
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        MainWindow w1;
        public LoginPage(MainWindow w)
        {
            InitializeComponent();
            w1 = w;
        } 
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(MsSqlClientTools.MsSqlClientTools.ConnectToDb(Login.Text, Password.Text) == "Connection established")
            {
                w1.LoggedIn();
            }
            else
            {
                Result.Text = "Логин или пароль не корректны, либо база данных не доступна";
            }
        }
    }
}
