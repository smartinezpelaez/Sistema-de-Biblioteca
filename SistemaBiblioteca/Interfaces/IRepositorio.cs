using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Interfaces
{
    public interface IRepositorio<T> where T : EntidadBase
    {
        IEnumerable<T> ObtenerTodos();
        T ObtenerPorId(int id);
        void Agregar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(int id);
        int Guardar();
    }

    public interface IServicioBiblioteca
    {
        bool RealizarPrestamo(int libroId, int miembroId, int diasPrestamo);
        bool DevolverLibro(int prestamoId);
        IEnumerable<Libro> BuscarLibrosPorAutor(string autor);
        IEnumerable<Libro> BuscarLibrosPorCategoria(string categoria);
    }
}
