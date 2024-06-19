using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using UDP_First_lab.Entity;

namespace UDP_First_lab
{
    class Program
    {
        private const int port = 8888;

        static void Main(string[] args)
        {
            UdpClient client = new UdpClient(port);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);

            try
            {
                while (true)
                {
                    Console.WriteLine("очікую");
                    byte[] data = client.Receive(ref ip);
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine(message);
                    string response = Request(message);
                    byte[] dataResponse = Encoding.Unicode.GetBytes(response);
                    client.Send(dataResponse, dataResponse.Length, ip);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                client.Close();
            }
        }

        private static string Request(string request)
        {
            string response = "";
            using (NintendoContext db = new NintendoContext())
            {
                string[] necro = request.Split(',');
                if (necro[0] == "id")
                {
                    int id = int.Parse(necro[1]);
                    Car car = db.Cars.Find(id);
                    response = car != null ? Necrosociety(car) : "немає нічого";
                }
                else if (necro[0] == "price")
                {
                    double min = double.Parse(necro[1]);
                    double max = double.Parse(necro[2]);
                    List<Car> car = db.Cars.Where(c => c.Price >= min && c.Price <= max && c.Available).ToList();
                    response= car.Any() ? string.Join("\n", car.ConvertAll(Necrosociety)) : "немає нічого";
                }
                else if (necro[0] == "model")
                {
                    string make = necro[1];
                    string model = necro[2];
                    var cars = db.Cars.Where(c => c.Make == make && c.Model == model && c.Available).ToList();
                    response = cars.Any() ? string.Join("\n", cars.Select(Necrosociety)) : "немає нічого";
                }
                else if (necro[0] == "remove")
                {
                    int id = int.Parse(necro[1]);
                    var car = db.Cars.FirstOrDefault(c => c.Id == id);
                    if (car != null)
                    {
                        car.Available = false;
                        db.SaveChanges();
                        response = "знято з продажу";
                    }
                    else
                    {
                        response = "немає нічого";
                    }
                }
            }
            return response;
        }

        private static string Necrosociety(Car car)
        {
            return $"Id: {car.Id}, Make: {car.Make}, Model: {car.Model}, Price: {car.Price}, Available: {car.Available}";
        }
    }
}