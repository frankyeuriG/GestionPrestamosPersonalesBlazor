using GestionPrestamosPersonales.DAL;
using GestionPrestamosPersonales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionPrestamosPersonales.BLL
{
    public class PersonasBLL
    {
        private Contexto _contexto;
        public PersonasBLL(Contexto contexto)
        {
            this._contexto = contexto;
        }

        public bool Guardar(Personas persona)
        {
            if (!Existe(persona.PersonaId))
            {
                return Insertar(persona);
            }
            else
            {
                return Modificar(persona);
            }
        }

        public bool Existe(int personaId)
        {

            return _contexto.Personas.Any(p => p.PersonaId == personaId);
        }

        public bool Insertar(Personas persona)
        {
            _contexto.Personas.Add(persona);
            int cantidad = _contexto.SaveChanges();
            return cantidad > 0;
        }

        public bool Modificar(Personas persona)
        {
            _contexto.Entry(persona).State = EntityState.Modified;
            return _contexto.SaveChanges() > 0;
        }

        public bool Eliminar(Personas persona)
        {
            _contexto.Entry(persona).State = EntityState.Deleted;
            return _contexto.SaveChanges() > 0;
        }

        public Personas? Buscar(int personaId)
        {
            return _contexto.Personas
                    .Where(o => o.PersonaId == personaId)
            .AsNoTracking()
            .SingleOrDefault();
        }

        public List<Personas> GetList(Expression<Func<Personas, bool>> Criterio)
        {
            return _contexto.Personas
                .AsNoTracking()
                .Where(Criterio)
                .ToList();
        }
    }
}