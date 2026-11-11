using Microsoft.EntityFrameworkCore;    //Proceso, lógica (CRUD)
using TiendaApp.Data; //Exposición > Acceso BD
using TiendaApp.Models; //Acceso estructura, validación

namespace TiendaApp.Services
{
    //Contrato
    public interface IProductoService
    {
        //Métodos que deberán cumplirse en la clase que implemente el contrato
        Task<List<Producto>> ListarAsync();
        Task<Producto?> ObtenerAsync(int id); //Es posible retornar un Null en lugar de un  objeto Producto
        Task CrearAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
        Task<List<Marca>> ListarMarcasAsync();
    }

    //La clase IDbContextFactory se utiliza para crear instancias de DbContext "BAJO DEMANDA" y de forma "SEGURA"
    public class ProductoService(IDbContextFactory<TiendaContext> factory) : IProductoService
    {
        public async Task ActualizarAsync(Producto producto)
        {
            await using var db = await factory.CreateDbContextAsync();
            db.Productos.Update(producto);
            await db.SaveChangesAsync();
        }

        public async Task CrearAsync(Producto producto)
        {
            await using var db = await factory.CreateDbContextAsync();
            db.Productos.Add(producto);
            await db.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await using var db = await factory.CreateDbContextAsync();
            await db.Productos.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Producto>> ListarAsync()
        {
            await using var db = await factory.CreateDbContextAsync();
            return await db.Productos
                .Include(p => p.Marca)
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .ToListAsync(); //LinQ
        }

        public async Task<List<Marca>> ListarMarcasAsync()
        {
            await using var db = await factory.CreateDbContextAsync();
            return await db.Marcas
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<Producto?> ObtenerAsync(int id)
        {
            await using var db = await factory.CreateDbContextAsync();
            return await db.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
