using DSWI_T2_OcampoWilmer.Models;
using Microsoft.Data.SqlClient;

namespace DSWI_T2_OcampoWilmer.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IConfiguration _configuration;

        public ProveedorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Proveedor Buscar(int? id = null)
        {
            var proveedor = Proveedores().FirstOrDefault(c => c.Id == id);
            if (proveedor == null) return new Proveedor { Nombre = "Registro no encontrado" };
            return proveedor;
        }

        public IEnumerable<Proveedor> Proveedores(string? n = null)
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            SqlDataReader dr = DatabaseHelper.ReturnDataReader(_configuration, "SP_Proveedores", n ?? string.Empty);
            while (dr.Read())
            {
                proveedores.Add(new Proveedor()
                {
                    Id = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    NombreContacto = dr.GetString(2),
                    CargoContacto = dr.GetString(3),
                    Direccion = dr.GetString(4),
                    IdPais = dr.GetString(5),
                    Telefono = dr.GetString(6),
                    Fax = dr.GetString(7)
                });
            }
            dr.Close();

            return proveedores;
        }
    }
}
