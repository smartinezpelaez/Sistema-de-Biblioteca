using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.DTOs;
using SistemaBiblioteca.Interfaces;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Servicios
{
    public class ServicioBiblioteca : IServicioBiblioteca
    {
        private readonly BibliotecaContext _context;

        public ServicioBiblioteca(BibliotecaContext context)
        {
            _context = context;
        }

        public bool RealizarPrestamo(int libroId, int miembroId, int diasPrestamo)
        {
            var libro = _context.Libros.Find(libroId);
            var miembro = _context.Miembros.Find(miembroId);

            if (libro == null || miembro == null || !libro.Disponible || !miembro.Activo)
                return false;

            var prestamo = new Prestamo
            {
                LibroId = libroId,
                MiembroId = miembroId,
                FechaPrestamo = DateTime.Now,
                FechaDevolucionEsperada = DateTime.Now.AddDays(diasPrestamo),
                Devuelto = false
            };

            libro.Disponible = false;
            libro.Stock--;

            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();

            return true;
        }

        public bool DevolverLibro(int prestamoId)
        {
            var prestamo = _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefault(p => p.Id == prestamoId);

            if (prestamo == null || prestamo.Devuelto)
                return false;

            prestamo.FechaDevolucionReal = DateTime.Now;
            prestamo.Devuelto = true;
            prestamo.Libro.Disponible = true;
            prestamo.Libro.Stock++;

            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Libro> BuscarLibrosPorAutor(string autor)
        {
            return _context.Libros
                .Where(l => l.Autor.Contains(autor))
                .Include(l => l.Categoria)
                .ToList();
        }

        public IEnumerable<Libro> BuscarLibrosPorCategoria(string categoria)
        {
            return _context.Libros
                .Include(l => l.Categoria)
                .Where(l => l.Categoria.Nombre.Contains(categoria))
                .ToList();
        }

        public EstadisticasBiblioteca ObtenerEstadisticas()
        {
            return new EstadisticasBiblioteca
            {
                TotalLibros = _context.Libros.Count(),
                LibrosDisponibles = _context.Libros.Count(l => l.Disponible),
                LibrosPrestados = _context.Libros.Count(l => !l.Disponible),
                TotalMiembros = _context.Miembros.Count(),
                MiembrosActivos = _context.Miembros.Count(m => m.Activo)
            };
        }

        public IEnumerable<dynamic> ObtenerLibrosMasPrestados(int cantidad)
        {
            return _context.Prestamos
                .GroupBy(p => p.LibroId)
                .Select(g => new
                {
                    LibroId = g.Key,
                    Titulo = g.First().Libro.Titulo,
                    CantidadPrestamos = g.Count()
                })
                .OrderByDescending(x => x.CantidadPrestamos)
                .Take(cantidad)
                .ToList();
        }
    }
}
