using GestionPrestamosPersonales.DAL;
using GestionPrestamosPersonales.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionPrestamosPersonales.BLL
{
    public class PagosBLL
    {
        private Contexto _contexto;

        public PagosBLL(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> Existe(int pagosId)
        {
            return await _contexto.Pagos.AnyAsync(o => o.PagoId == pagosId);
        }

        public async Task<bool> Guardar(Pagos pagos)
        {
            var existe = await Existe(pagos.PagoId);

            if (!existe)
                return await this.Insertar(pagos);
            else
                return await this.Modificar(pagos);
        }

        private async Task<bool> Insertar(Pagos pago)
        {
            await _contexto.Pagos.AddAsync(pago);

            foreach (var item in pago.Detalle)
            {
                var prestamo = await _contexto.Prestamos.FindAsync(item.PrestamoId);
                prestamo!.Balance -= item.ValorPagado;
            }

            var persona = await _contexto.Personas.FindAsync(pago.PersonaId);
            persona!.Balance -= pago.Monto;

            var insertados = await _contexto.SaveChangesAsync();

            return insertados > 0;
        }
        private async Task<bool> Modificar(Pagos pagoActual)
        {
            var pagoAnterior = await _contexto.Pagos
                 .Where(p => p.PagoId == pagoActual.PagoId)
                 .AsNoTracking()
                 .SingleOrDefaultAsync();

            //revertir el balance pagado a la persona.
            var persona = await _contexto.Personas.FindAsync(pagoAnterior!.PersonaId);
            persona!.Balance += pagoAnterior.Monto;

            //revertir el balance pagado a los prestamos
            foreach (var item in pagoAnterior.Detalle)
            {
                var prestamos = await _contexto.Prestamos.FindAsync(item.PrestamoId);
                prestamos!.Balance += item.ValorPagado;
            }

            //borrar el detalle anterior
            await _contexto.Database.ExecuteSqlRawAsync($"Delete FROM PagosDetalle Where PagoId = {pagoActual.PagoId}");

            foreach (var item in pagoActual.Detalle)
            {
                _contexto.Entry(item).State = EntityState.Added;

                //afectar los prestamos segun el nuevo valor pagado.
                var prestamo = await _contexto.Prestamos.FindAsync(item.PrestamoId);
                prestamo!.Balance -= item.ValorPagado;
            }

            //reafectar el balance de la persona con el monto pagado.
            persona.Balance -= pagoActual.Monto;

            //marcar el prestamo como modificado
            _contexto.Entry(pagoActual).State = EntityState.Modified;
            
            //finalmente guardar
            var cantidad = await _contexto.SaveChangesAsync();

            //liberar la instancia de pago para evitar que EF de error.
            _contexto.Entry(pagoActual).State = EntityState.Detached;

            return cantidad > 0;
        }
        
        public async Task<bool> Eliminar(Pagos pagos)
        {

            var persona = await _contexto.Personas.FindAsync(pagos.PersonaId);
            persona!.Balance += pagos.Monto;

            foreach (var item in pagos.Detalle)
            {
                var prestamos = await _contexto.Prestamos.FindAsync(item.PrestamoId);
                prestamos!.Balance += item.ValorPagado;
            }

            _contexto.Entry(pagos).State = EntityState.Deleted;

            var cantidad = await _contexto.SaveChangesAsync();
            
            return cantidad > 0;
        }
        
        public async Task<Pagos?> Buscar(int pagoId)
        {
            var pago = await _contexto.Pagos
            .Where(o => o.PagoId == pagoId)
            .Include(o => o.Detalle)
            .AsNoTracking()
            .SingleOrDefaultAsync();
            
            return pago;
        }
 
        public async Task< List<Pagos>> GetList(Expression<Func<Pagos, bool>> Criterio)
        {
            return await _contexto.Pagos
                .Where(Criterio)
                .AsNoTracking()
                .ToListAsync();
        }

    }

}