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
using System.Net;
using System.Net.Sockets;

namespace StudyApp
{
    public partial class StudentPage : Page
    {
        MainWindow mainWindow;
        bool isQueueStarted = false;
        public StudentPage(MainWindow _mainWindow)
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

                int position = 0;
                int iCorrect = 0;
                for(int i = 0; i < Core.DB.Queue.Count(); i++)
                {
                    if (Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault() != null)
                    {
                        Queue queue = Core.DB.Queue.Where(s => s.Id == iCorrect).FirstOrDefault();
                        Queue userQueue = Core.DB.Queue.Where(s => s.UserId == mainWindow.user.Id).FirstOrDefault();
                        if(userQueue != null)
                        {
                            if (queue.DateTime < userQueue.DateTime)
                            {
                                position++;
                            }
                        }
                    }
                    else
                        i--;
                    iCorrect++;
                }

                if(position == 0)
                {
                    UserQueueLabel.Content = "-";
                }
                else
                {
                    position++;
                    UserQueueLabel.Content = position;
                    position--;
                }
                    

                if (Core.DB.Queue.Where(s => s.UserId == mainWindow.user.Id).FirstOrDefault() != null)
                {
                    isQueueStarted = true;
                    StartQueueBTN.Content = "Выйти из очереди";
                }
                else
                {

                    isQueueStarted = false;
                    StartQueueBTN.Content = "Занять очередь";
                }
                
                await Task.Delay(1000);
            }
        }

        public static DateTime GetNetworkTime()
        {
            const string ntpServer = "time.windows.com";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }

        private void StartQueueBTN_Click(object sender, RoutedEventArgs e)
        {
            if(isQueueStarted == true)
            {
                Queue q = Core.DB.Queue.Where(s => s.UserId == mainWindow.user.Id).FirstOrDefault();
                if (q.Used == false)
                {
                    Core.DB.Queue.Remove(q);
                    Core.DB.SaveChanges();
                    StartQueueBTN.Content = "Занять очередь";
                    isQueueStarted = false;
                }
                else
                    MessageBox.Show("Вас уже опрашивают, покинуть очередь невозможно", "Невозможно выполнить действие!", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                Queue q = new Queue() { UserId = mainWindow.user.Id, DateTime = GetNetworkTime() };
                Core.DB.Queue.Add(q);
                Core.DB.SaveChanges();
                StartQueueBTN.Content = "Выйти из очереди";
                isQueueStarted = true;
            }
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
