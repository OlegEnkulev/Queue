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
using StudyApp.Resources;
using StudyApp.Pages;

namespace StudyApp.Pages
{
    public partial class LoginPage : Page
    {
        MainWindow mainWindow;
        public LoginPage(MainWindow _mainWindow)
        {
            InitializeComponent();

            mainWindow = _mainWindow;
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            if(LoginBox.Text.Length >= 4 || PasswordBox.Password.Length >= 4)
            {
                if(Core.DB.Users.Where(s => s.Login == LoginBox.Text) != null)
                {
                    if (Core.DB.Users.Where(s => s.Login == LoginBox.Text).FirstOrDefault() != null)
                    {
                        Users user = Core.DB.Users.Where(s => s.Login == LoginBox.Text).FirstOrDefault();

                        if (user.Password == PasswordBox.Password)
                        {
                            mainWindow.user = user;
                            mainWindow.UpdateData();
                        }
                        else
                            MessageBox.Show("Введены неправильные данные", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Введены неправильные данные", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Введены неправильные данные", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Введены неправильные данные", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
