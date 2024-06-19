using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            string request = textBoxRequest.Text;
            string response = Request(request);
            textBoxResponse.Text = response;
        }

        private string Request(string request)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, 8888);

            byte[] data = Encoding.Unicode.GetBytes(request);
            client.Send(data, data.Length, ip);

            byte[] dataResponse = client.Receive(ref ip);
            string response = Encoding.Unicode.GetString(dataResponse);

            return response;
        }
    }
}
