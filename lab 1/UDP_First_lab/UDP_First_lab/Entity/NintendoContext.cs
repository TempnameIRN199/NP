using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP_First_lab.Entity
{
    public class NintendoContext : DbContext
    {
        public NintendoContext() : base("Data Source=DESKTOP-U9EP3LS;Initial Catalog=Car;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<Car> Cars { get; set; }
    }
}
