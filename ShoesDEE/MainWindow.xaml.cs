using ShoesDEE.DataBase;
using ShoesDEE.Helpers;
using ShoesDEE.Statics;
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

namespace ShoesDEE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private pupenkoshoesbdi43Entities1 _db = new pupenkoshoesbdi43Entities1();
        private MessageHelper _mh = new MessageHelper();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginEnter.Text;
            string password = PasswordEnter.Password;

            var user = _db.User.Where(u => u.Login == login && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                _mh.ShowError("Введен неправильный логин или пароль!");
                return;
            }
            else
            {
                CurrentSession.CurrentUser = user;
                new ProductWindow(user).Show();
                Close();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new ProductWindow().Show();
            Close();
        }
    }
}
