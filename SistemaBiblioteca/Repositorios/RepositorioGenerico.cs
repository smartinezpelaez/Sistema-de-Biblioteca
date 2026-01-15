using SistemaBiblioteca.Data;
using SistemaBiblioteca.Interfaces;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Repositorios
{
    public class RepositorioGenerico<T> : IRepositorio<T> where T : EntidadBase
    {
        private readonly BibliotecaContext _context;

        public RepositorioGenerico(BibliotecaContext context)
        {
            _context = context;
        }
        public IEnumerable<T> ObtenerTodos()
        {
            return _context.Set<T>().ToList();
        }

        public T ObtenerPorId(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Agregar(T entidad)
        {
            _context.Set<T>().Add(entidad);
        }

        public void Actualizar(T entidad)
        {
            _context.Set<T>().Update(entidad);
        }

        public void Eliminar(int id)
        {
            var entidad = ObtenerPorId(id);
            if (entidad != null)
            {
                _context.Set<T>().Remove(entidad);
            }
        }

        public int Guardar()
        {
            return _context.SaveChanges();
        }

    }
}
