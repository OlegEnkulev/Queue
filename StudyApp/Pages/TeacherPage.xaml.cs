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

namespace StudyApp.Pages
{
    public partial class TeacherPage : Page
    {
        MainWindow mainWindow;
        bool isAskStarted = false;
        Users userAsking = null;
        public TeacherPage(MainWindow _mainWindow)
        {
            InitializeComponent();

            mainWindow = _mainWindow;

            Update();
        }

        async void Update()
        {
            while (true)
            {
                CountQueueLabel.Content = Core.DB.Queue.Count();

                int iCorrect = 0;
                Queue minimalQueue = null;
                for (int i = 0; i < Core.DB.Queue.Count(); i++)
                {
                    if (Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault() != null)
                    {
                        Queue queue = Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault();
                        if(minimalQueue == null)
                        {
                            minimalQueue = queue;
                        }
                        else
                        {
                            if(minimalQueue.DateTime > queue.DateTime)
                            {
                                minimalQueue = queue;
                            }
                        }
                    }
                    else
                        i--;
                    iCorrect++;
                }

                if (minimalQueue != null)
                {
                    Users user = Core.DB.Users.Where(s => s.Id == minimalQueue.UserId).FirstOrDefault();
                    userAsking = user;
                    UserQueueLabel.Content = user.LastName + " " + user.FirstName;
                }
                else
                    UserQueueLabel.Content = "Некого спрашивать...";

                await Task.Delay(1000);
            }
        }

        private void StartStopAskBTN_Click(object sender, RoutedEventArgs e)
        {
            if(isAskStarted == true)
            {
                isAskStarted = false;
                StartStopAskBTN.Content = "Начать опрос";
                Queue queueOnDelete = Core.DB.Queue.Where(s => s.UserId == userAsking.Id).FirstOrDefault();
                Core.DB.Queue.Remove(queueOnDelete);
                Core.DB.SaveChanges();
                userAsking = null;
            }
            else
            {
                int iCorrect = 0;
                Queue minimalQueue = null;
                for (int i = 0; i < Core.DB.Queue.Count(); i++)
                {
                    if (Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault() != null)
                    {
                        Queue queue = Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault();
                        if (minimalQueue == null)
                        {
                            minimalQueue = queue;
                        }
                        else
                        {
                            if (minimalQueue.DateTime > queue.DateTime)
                            {
                                minimalQueue = queue;
                            }
                        }
                    }
                    else
                        i--;
                    iCorrect++;
                }

                if (minimalQueue != null)
                {
                    Users user = Core.DB.Users.Where(s => s.Id == minimalQueue.UserId).FirstOrDefault();
                    isAskStarted = true;
                    StartStopAskBTN.Content = "Завершить опрос";
                    Queue queueHandled = Core.DB.Queue.Where(s => s.UserId == user.Id).FirstOrDefault();
                    queueHandled.Used = true;
                    Core.DB.SaveChanges();
                    userAsking = user;
                }
                else
                    MessageBox.Show("Некого спрашивать...", "Ошибка опроса", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
