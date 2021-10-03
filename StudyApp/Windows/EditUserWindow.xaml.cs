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
using StudyApp.Pages;
using StudyApp.Resources;

namespace StudyApp.Windows
{
    public partial class EditUserWindow : Window
    {
        MainWindow mainWindow;
        int action;
        Users editableUser;

        public EditUserWindow(MainWindow _mainWindow, int _action, Users _editableUser)
        {
            InitializeComponent();

            mainWindow = _mainWindow;
            action = _action;
            editableUser = _editableUser;
        }

        private void NameBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void LastNameBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void LoginBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            switch (action)
            {
                case 0:
                    if (NameBox.Text.Length >= 2 || LastNameBox.Text.Length >= 3 || LoginBox.Text.Length >= 4 || PasswordBox.Password.Length >= 4 || AccessBox.SelectedItem != null)
                    {
                        editableUser.FirstName = NameBox.Text;
                        editableUser.LastName = LastNameBox.Text;
                        editableUser.Login = LoginBox.Text;
                        editableUser.Password = PasswordBox.Password;
                        editableUser.Access = AccessBox.SelectedIndex;

                        Core.DB.SaveChanges();

                        mainWindow.MainFrame.Navigate(new AdminPage(mainWindow));
                        mainWindow.IsEnabled = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Введены неудовлетворяющие данные", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 1:
                    if (NameBox.Text.Length >= 2 || LastNameBox.Text.Length >= 3 || LoginBox.Text.Length >= 4 || PasswordBox.Password.Length >= 4 || AccessBox.SelectedItem != null)
                    {
                        Users user = new Users() { FirstName = NameBox.Text, LastName = LastNameBox.Text, Login = LoginBox.Text, Password = PasswordBox.Password, Access = AccessBox.SelectedIndex };
                        Core.DB.Users.Add(user);
                        Core.DB.SaveChanges();
                        mainWindow.MainFrame.Navigate(new AdminPage(mainWindow));
                        mainWindow.IsEnabled = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Введены неудовлетворяющие данные", "Невозможно выполнить действие", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.Navigate(new AdminPage(mainWindow));
            mainWindow.IsEnabled = true;
            this.Close();
        }
    }
}
