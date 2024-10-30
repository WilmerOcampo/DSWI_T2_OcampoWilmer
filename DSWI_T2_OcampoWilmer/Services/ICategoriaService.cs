using DSWI_T2_OcampoWilmer.Models;

namespace DSWI_T2_OcampoWilmer.Services
{
    public interface ICategoriaService
    {
        IEnumerable<Categoria> Categorias(string? n = null);
        Categoria Buscar(int? id = null);
    }
}
