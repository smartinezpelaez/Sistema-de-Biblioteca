using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Servicios;

namespace SistemaBiblioteca
{
    class Program
    {
        // Constante - Value Type
        private const string CONNECTION_STRING =
            "Server=localhost\\SQLEXPRESS;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE BIBLIOTECA ===\n");

            // Configurar DbContext
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaContext>();
            optionsBuilder.UseSqlServer(CONNECTION_STRING);

            using (var context = new BibliotecaContext(optionsBuilder.Options))
            {
                var servicio = new ServicioBiblioteca(context);

                bool salir = false;
                while (!salir)
                {
                    MostrarMenu();
                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            MostrarTodosLosLibros(context);
                            break;
                        case "2":
                            BuscarLibrosPorAutor(servicio);
                            break;
                        case "3":
                            RealizarPrestamo(servicio);
                            break;
                        case "4":
                            DevolverLibro(servicio);
                            break;
                        case "5":
                            MostrarEstadisticas(servicio);
                            break;
                        case "6":
                            MostrarLibrosMasPrestados(servicio);
                            break;
                        case "7":
                            MostrarPrestamosActivos(context);
                            break;
                        case "0":
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("Opción no válida");
                            break;
                    }

                    if (!salir)
                    {
                        Console.WriteLine("\nPresione Enter para continuar...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }

            Console.WriteLine("\n¡Hasta pronto!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
            Console.WriteLine("1. Ver todos los libros");
            Console.WriteLine("2. Buscar libros por autor");
            Console.WriteLine("3. Realizar préstamo");
            Console.WriteLine("4. Devolver libro");
            Console.WriteLine("5. Ver estadísticas");
            Console.WriteLine("6. Libros más prestados");
            Console.WriteLine("7. Préstamos activos");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");
        }

        static void MostrarTodosLosLibros(BibliotecaContext context)
        {
            // LINQ con Include para cargar relaciones
            var libros = context.Libros
                .Include(l => l.Categoria)
                .OrderBy(l => l.Titulo)
                .ToList();

            Console.WriteLine("\n=== CATÁLOGO DE LIBROS ===");
            foreach (var libro in libros)
            {
                string estado = libro.Disponible ? "Disponible" : "Prestado";
                Console.WriteLine($"\nID: {libro.Id}");
                Console.WriteLine($"Título: {libro.Titulo}");
                Console.WriteLine($"Autor: {libro.Autor}");
                Console.WriteLine($"Categoría: {libro.Categoria?.Nombre ?? "Sin categoría"}");
                Console.WriteLine($"Estado: {estado} (Stock: {libro.Stock})");
            }
        }

        static void BuscarLibrosPorAutor(ServicioBiblioteca servicio)
        {
            Console.Write("\nIngrese nombre del autor: ");
            string autor = Console.ReadLine();

            var libros = servicio.BuscarLibrosPorAutor(autor);

            if (!libros.Any())
            {
                Console.WriteLine("No se encontraron libros de ese autor.");
                return;
            }

            Console.WriteLine($"\n=== LIBROS DE '{autor}' ===");
            foreach (var libro in libros)
            {
                Console.WriteLine($"\n{libro.ObtenerInformacion()}");
                Console.WriteLine($"Categoría: {libro.Categoria.Nombre}");
            }
        }

        static void RealizarPrestamo(ServicioBiblioteca servicio)
        {
            Console.Write("\nID del libro: ");
            int libroId = int.Parse(Console.ReadLine());

            Console.Write("ID del miembro: ");
            int miembroId = int.Parse(Console.ReadLine());

            Console.Write("Días de préstamo: ");
            int dias = int.Parse(Console.ReadLine());

            bool exito = servicio.RealizarPrestamo(libroId, miembroId, dias);

            if (exito)
                Console.WriteLine("\n✓ Préstamo realizado exitosamente");
            else
                Console.WriteLine("\n✗ No se pudo realizar el préstamo");
        }

        static void DevolverLibro(ServicioBiblioteca servicio)
        {
            Console.Write("\nID del préstamo: ");
            int prestamoId = int.Parse(Console.ReadLine());

            bool exito = servicio.DevolverLibro(prestamoId);

            if (exito)
                Console.WriteLine("\n✓ Libro devuelto exitosamente");
            else
                Console.WriteLine("\n✗ No se pudo devolver el libro");
        }

        static void MostrarEstadisticas(ServicioBiblioteca servicio)
        {
            var stats = servicio.ObtenerEstadisticas();

            Console.WriteLine("\n=== ESTADÍSTICAS DE LA BIBLIOTECA ===");
            Console.WriteLine($"Total de libros: {stats.TotalLibros}");
            Console.WriteLine($"Libros disponibles: {stats.LibrosDisponibles}");
            Console.WriteLine($"Libros prestados: {stats.LibrosPrestados}");
            Console.WriteLine($"Total de miembros: {stats.TotalMiembros}");
            Console.WriteLine($"Miembros activos: {stats.MiembrosActivos}");
        }

        static void MostrarLibrosMasPrestados(ServicioBiblioteca servicio)
        {
            var topLibros = servicio.ObtenerLibrosMasPrestados(5);

            Console.WriteLine("\n=== TOP 5 LIBROS MÁS PRESTADOS ===");
            foreach (var item in topLibros)
            {
                Console.WriteLine($"{item.Titulo}: {item.CantidadPrestamos} préstamos");
            }
        }

        static void MostrarPrestamosActivos(BibliotecaContext context)
        {
            // LINQ con múltiples Include
            var prestamos = context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Miembro)
                .Where(p => !p.Devuelto)
                .ToList();

            Console.WriteLine("\n=== PRÉSTAMOS ACTIVOS ===");
            foreach (var prestamo in prestamos)
            {
                Console.WriteLine($"\nID Préstamo: {prestamo.Id}");
                Console.WriteLine($"Libro: {prestamo.Libro.Titulo}");
                Console.WriteLine($"Miembro: {prestamo.Miembro.Nombre}");
                Console.WriteLine($"Fecha préstamo: {prestamo.FechaPrestamo:dd/MM/yyyy}");
                Console.WriteLine($"Devolución esperada: {prestamo.FechaDevolucionEsperada:dd/MM/yyyy}");
                Console.WriteLine($"Días transcurridos: {prestamo.DiasTranscurridos}");
            }
        }
    }
}