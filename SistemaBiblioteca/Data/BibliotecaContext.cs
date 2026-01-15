using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
           : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Miembro)
                .WithMany(m => m.Prestamos)
                .HasForeignKey(p => p.MiembroId);
        }

    }
}
