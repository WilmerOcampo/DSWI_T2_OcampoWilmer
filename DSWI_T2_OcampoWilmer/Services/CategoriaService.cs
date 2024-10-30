using DSWI_T2_OcampoWilmer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DSWI_T2_OcampoWilmer.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IConfiguration _configuration;

        public CategoriaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Categoria Buscar(int? id = null)
        {
            var categoria = Categorias().FirstOrDefault(c => c.Id == id);
            if (categoria == null) return new Categoria { Nombre = "Registro no encontrado" };
            return categoria; ;
        }

        public IEnumerable<Categoria> Categorias(string? n = null)
        {
            List<Categoria> categorias = new List<Categoria>();

            SqlDataReader dr = DatabaseHelper.ReturnDataReader(_configuration, "SP_Categorias", n ?? string.Empty);
            while (dr.Read())
            {
                categorias.Add(new Categoria()
                {
                    Id = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    Descripcion = dr.GetString(2)
                });
            }
            dr.Close();

            return categorias;
        }
    }
}
