using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacao>()
                .HasMany(trans => trans.Categorias)
                .WithMany(trans => trans.Transacoes)
                .UsingEntity<CategoriaTransacao>(
                    j => j
                        .HasOne(catTrans => catTrans.Categoria)
                        .WithMany(trans => trans.CategoriasTransacoes)
                        .HasForeignKey(catTrans => catTrans.CategoriaId),
                    j => j
                        .HasOne(catTrans => catTrans.Transacao)
                        .WithMany(trans => trans.CategoriasTransacoes)
                        .HasForeignKey(catTrans => catTrans.TransacaoId),
                    j =>
                    {
                        j.Property(catTrans => catTrans.CriadoEm).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        j.HasKey(trans => new { trans.TransacaoId, trans.CategoriaId });
                    });
        }

    }
}