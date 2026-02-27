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
using Common;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<News> News = new List<News>();
        public MainWindow()
        {
            InitializeComponent();
            LoadNews();
        }
        public void GetContent()
        {
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (socket.Connected)
            {
                Command command = new Command() { Message = "start" };
                string JsonCommand = JsonConvert.SerializeObject(command);
                byte[] buffer = Encoding.UTF8.GetBytes(JsonCommand);
                socket.Send(buffer);
                buffer = new byte[10485760];
                int ByteLenght = socket.Receive(buffer);
                string Response = Encoding.UTF8.GetString(buffer,0, ByteLenght);
                News = JsonConvert.DeserializeObject<List<News>>(Response);
                LoadNews();
            }
            socket.Close();
        }
        public void LoadNews()
        {
            ParentNews.Items.Clear();
            foreach (var New in News)
            {
                ParentNews.Items.Add(New);
            }
        }
        private void Get(object sender, RoutedEventArgs e) => GetContent();

    }
}
