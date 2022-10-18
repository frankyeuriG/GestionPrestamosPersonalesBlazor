using GestionPrestamosPersonales.DAL;
using GestionPrestamosPersonales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionPrestamosPersonales.BLL
{
    public class PrestamosBLL
    {
        private Contexto _contexto;

        public PrestamosBLL(Contexto contexto)
        {
            this._contexto = contexto;
        }

        public bool Guardar(Prestamos prestamo)
        {
            if (!Existe(prestamo.PrestamoId))
            {
                return Insertar(prestamo);
            }
            else
            {
                return Modificar(prestamo);
            }
        }

        public bool Existe(int prestamoId)
        {
            return _contexto.Prestamos.Any(p => p.PrestamoId == prestamoId);
        }

        public bool Insertar(Prestamos prestamo)
        {
            _contexto.Prestamos.Add(prestamo);
            
            var persona = _contexto.Personas.Find(prestamo.PersonaId);
            persona.Balance += prestamo.Monto;
            
            int cantidad = _contexto.SaveChanges();
            
            return cantidad > 0;
        }

        public bool Modificar(Prestamos prestamoActual)
        {
            //descontar el monto anterior
            var prestamoAnterior = _contexto.Prestamos
                .Where(p => p.PrestamoId == prestamoActual.PrestamoId)
                .AsNoTracking()
                .SingleOrDefault();

            var personaAnterior = _contexto.Personas.Find(prestamoAnterior.PersonaId);
            personaAnterior.Balance -= prestamoAnterior.Monto;

            _contexto.Entry(prestamoActual).State = EntityState.Modified;
            
            //descontar el monto nuevo
            var persona = _contexto.Personas.Find(prestamoActual.PersonaId);
            persona.Balance += prestamoActual.Monto;

            return _contexto.SaveChanges() > 0;
        }
        public bool Eliminar(Prestamos prestamo)
        {
            var persona = _contexto.Personas.Find(prestamo.PersonaId);
            persona.Balance -= prestamo.Monto;
            
            _contexto.Entry(prestamo).State = EntityState.Deleted;
            return _contexto.SaveChanges() > 0;
        }

        public Prestamos? Buscar(int prestamoId)
        {
            return _contexto.Prestamos
                    .Where(p => p.PrestamoId == prestamoId)
                    .AsNoTracking()
                    .SingleOrDefault();

        }

        public List<Prestamos> GetList(Expression<Func<Prestamos, bool>> Criterio)
        {
            return _contexto.Prestamos
                .AsNoTracking()
                .Where(Criterio)
                .ToList();
        }
    }

}