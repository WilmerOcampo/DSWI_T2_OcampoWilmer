using DSWI_T2_OcampoWilmer.Models;

namespace DSWI_T2_OcampoWilmer.Services
{
    public interface IProductoService
    {
        IEnumerable<Producto> Productos(string? n = null);
        Producto Buscar(int? id = null);
        string Insert(Producto p);
        string Update(Producto p);
        string Delete(Producto p);
        int GenerarId();
    }
}
