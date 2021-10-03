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
using StudyApp.Pages;
using StudyApp.Resources;

namespace StudyApp
{
    public partial class MainWindow : Window
    {
        public Users user;

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new LoginPage(this));
        }

        private void AccountBTN_Click(object sender, RoutedEventArgs e)
        {
            if(user == null)
            {
                MainFrame.Navigate(new LoginPage(this));
            }
            else
            {
                UpdateData();
            }
        }

        public void UpdateData()
        {
            if (user == null)
            {

                UserNameLabel.Content = "Войдите в аккаунт!";
                MainFrame.Navigate(new LoginPage(this));
            }
            else
            {

                if(user != null)
                {
                    UserNameLabel.Content = user.LastName + " " + user.FirstName;

                    switch (user.Access)
                    {
                        case 0:
                            MainFrame.Navigate(new StudentPage(this));
                            break;
                        case 1:
                            MainFrame.Navigate(new TeacherPage(this));
                            break;
                        case 2:
                            MainFrame.Navigate(new AdminPage(this));
                            break;
                    }
                }
                else
                {
                    UserNameLabel.Content = "Войдите в аккаунт!";
                    MainFrame.Navigate(new LoginPage(this));
                }
            }
        }
    }
}
