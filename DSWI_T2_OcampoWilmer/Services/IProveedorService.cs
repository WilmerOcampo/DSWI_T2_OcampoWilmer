using DSWI_T2_OcampoWilmer.Models;

namespace DSWI_T2_OcampoWilmer.Services
{
    public interface IProveedorService
    {
        IEnumerable<Proveedor> Proveedores(string? n = null);
        Proveedor Buscar(int? id = null);
    }
}
