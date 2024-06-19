using System;
using System.Data.Entity;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCP_Fifth_lab.Entity;

namespace toothfairy
{
    class Program
    {
        private static int port = 8888;

        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Any;
            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.WriteLine("engine start");

            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    byte[] data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string message = Encoding.Unicode.GetString(data, 0, bytes);
                    string response = Request(message);
                    byte[] bytes1 = Encoding.Unicode.GetBytes(response);
                    stream.Write(bytes1, 0, bytes1.Length);
                    if(message == "0")
                    {
                        Console.WriteLine("engine kaput");
                        server.Stop();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
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
                    response = car != null ? Necrosociety(car) : "нічого немає";
                }
                else if (necro[0] == "price")
                {
                    int min = int.Parse(necro[1]);
                    int max = int.Parse(necro[2]);
                    foreach (Car car in db.Cars)
                    {
                        if (car.Price >= min && car.Price <= max)
                        {
                            response += Necrosociety(car) + "\n";
                        }
                    }
                }
                else if (necro[0] == "search")
                {
                    string mark = necro[1];
                    string model = necro[2];
                    foreach (Car car in db.Cars)
                    {
                        if (car.Brand == mark && car.Model == model)
                        {
                            response += Necrosociety(car) + "\n";
                        }
                    }
                }
                else if (necro[0] == "takeOff")
                {
                    string mark = necro[1];
                    string model = necro[2];
                    foreach (Car car in db.Cars)
                    {
                        if (car.Brand == mark && car.Model == model)
                        {
                            car.IsForSale = false;
                            db.Entry(car).State = EntityState.Modified;
                            db.SaveChanges();
                            response = $"знято з продажу {car.Brand} {car.Model}";
                        }
                    }
                }
                else if (necro[0] == "add")
                {
                    string brand = necro[1];
                    string model = necro[2];
                    decimal price = decimal.Parse(necro[3]);
                    int available = int.Parse(necro[4]);
                    bool isForSale = bool.Parse(necro[5]);
                    Car car = new Car()
                    {
                        Brand = brand,
                        Model = model,
                        Price = price,
                        Available = available,
                        IsForSale = isForSale
                    };
                    db.Cars.Add(car);
                    db.SaveChanges();
                    response = $"додано {brand} {model}";
                }
                else if (necro[0] == "addAuto")
                {
                    string brand = necro[1];
                    string model = necro[2];
                    decimal price = decimal.Parse(necro[3]);
                    int available = int.Parse(necro[4]);
                    bool isForSale = bool.Parse(necro[5]);
                    Car car = new Car()
                    {
                        Brand = brand,
                        Model = model,
                        Price = price,
                        Available = available,
                        IsForSale = isForSale
                    };
                    db.Cars.Add(car);
                    db.SaveChanges();
                    response = $"додано {brand} {model}";
                }
                else if (necro[0] == "edit")
                {
                    int id = int.Parse(necro[1]);
                    Car car = db.Cars.Find(id);
                    if (car != null)
                    {
                        car.Price = decimal.Parse(necro[2]);
                        db.Entry(car).State = EntityState.Modified;
                        db.SaveChanges();
                        response = $"змінено ціну {car.Brand} {car.Model}";
                    }
                }
            }
            return response;
        }

        private static string Necrosociety(Car car)
        {
            return $"Id: {car.Id}, Make: {car.Brand}, Model: {car.Model}, Price: {car.Price}, Available: {car.Available}, IsForSale: {car.IsForSale}";
        }
    }
}