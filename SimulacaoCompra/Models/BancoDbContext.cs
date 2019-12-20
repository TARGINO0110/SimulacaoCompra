using Microsoft.EntityFrameworkCore;

namespace SimulacaoCompra.Models
{
    public class BancoDbContext : DbContext
    {
        public virtual DbSet<Compra> Compras { get; set; }

        public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }
    }
}
