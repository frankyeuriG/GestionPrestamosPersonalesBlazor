using GestionPrestamosPersonales.DAL;
using GestionPrestamosPersonales.Models;

namespace GestionPrestamosPersonales.BLL
{
    public class OcupacionesBLL
    {
        private Contexto _contexto;
        public OcupacionesBLL( Contexto contexto)
        {
            _contexto = contexto;

        }

        public bool Existe(int ocupacionId)
        {
         return   _contexto.Ocupaciones
                .Any(o => o.OcupacionId == ocupacionId);
        }

        public bool Guardar(Ocupaciones ocupacion)
        {
            //if (!Existe(ocupacion.OcupacionId))
                return this.Insertar(ocupacion);
            //else
                //return this.Modificar(compra);
        }


        private bool Insertar(Ocupaciones ocupacion)
        {
            _contexto.Ocupaciones.Add(ocupacion);
 

            return _contexto.SaveChanges() > 0;
        }
    }
}
