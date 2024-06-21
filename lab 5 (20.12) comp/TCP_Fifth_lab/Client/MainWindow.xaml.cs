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
using System.Windows.Media.Media3D;
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
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8888);
                NetworkStream stream = client.GetStream();

                byte[] data = Encoding.Unicode.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[256];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.Unicode.GetString(buffer, 0, bytes);

                client.Close();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
