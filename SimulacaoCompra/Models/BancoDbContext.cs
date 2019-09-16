using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimulacaoCompra.Models
{
    public class BancoDbContext : DbContext
    {
        public virtual DbSet<Compra> Compras { get; set; }

        public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }
    }
}
