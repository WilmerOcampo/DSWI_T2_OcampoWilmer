using DSWI_T2_OcampoWilmer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DSWI_T2_OcampoWilmer.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IConfiguration _configuration;

        public ProductoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Delete(Producto p)
        {
            string msg;
            try
            {
                DatabaseHelper.ExecuteStoredProcedure(_configuration, "SP_EliminarProducto", p.Id);

                msg = $"El producto {p.Id + " | " + p.Nombre} fue eliminado";
            }
            catch (Exception e)
            {
                msg = "Error: " + e.Message;
            }
            return msg;
        }

        public Producto Buscar(int? id = null)
        {
            var producto = Productos().FirstOrDefault(c => c.Id == id);
            if (producto == null) return new Producto { Nombre = "Registro no encontrado" };
            return producto;
        }

        public IEnumerable<Producto> Productos(string? n = null)
        {
            List<Producto> productos = new List<Producto>();

            SqlDataReader dr = DatabaseHelper.ReturnDataReader(_configuration, "SP_ProductosN", n ?? string.Empty);
            while (dr.Read())
            {
                productos.Add(new Producto()
                {
                    Id = dr.GetInt32(0),
                    Nombre = dr.GetString(1),
                    IdProveedor = dr.GetInt32(2),
                    IdCategoria = dr.GetInt32(3),
                    UnidadMedida = dr.GetString(4),
                    PrecioUnidad = dr.GetDecimal(5),
                    Stock = dr.GetInt32(6)
                });
            }
            dr.Close();

            return productos;
        }

        public string Insert(Producto p)
        {
            return ExecuteStoredProcedure("SP_InsertarProducto", p, "registrado");
        }

        public string Update(Producto p)
        {
            return ExecuteStoredProcedure("SP_ActualizarProducto", p, "actualizado");
        }

        private string ExecuteStoredProcedure(string storedProcedure, Producto p, string action)
        {
            string msg;
            try
            {
                DatabaseHelper.ExecuteStoredProcedure(_configuration, storedProcedure, p.Id, p.Nombre, p.IdProveedor, p.IdCategoria, p.UnidadMedida, p.PrecioUnidad, p.Stock);
                msg = $"El producto {p.Id + " | " + p.Nombre} fue {action}";
            }
            catch (Exception e)
            {
                msg = "Error: " + e.Message;
            }
            return msg;
        }

        public int GenerarId()
        {
            int id = 0;

            try
            {
                id = (int)DatabaseHelper.ReturnScalarValue(_configuration, "SP_GenerarIdProducto");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el ID del producto: " + ex.Message);
            }

            return id;
        }
    }
}
