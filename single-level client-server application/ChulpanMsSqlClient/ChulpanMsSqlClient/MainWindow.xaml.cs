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
using MsSqlClientTools;

namespace ChulpanMsSqlClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LoginPage LP;
        public MainPage MP;
        public MainWindow()
        {
            InitializeComponent();
            LP = new LoginPage(this);
            MP = new MainPage();
            Window.Content = LP;
        }
        public void LoggedIn()
        {
            Window.Content = MP;
        }
    }
}
