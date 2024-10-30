using Microsoft.Data.SqlClient;
using System.Data;

public class DatabaseHelper
{
    private static string GetConnectionString(IConfiguration configuration)
    {
        return configuration["ConnectionStrings:sql"] ?? throw new InvalidOperationException("La cadena de conexión es nula o está vacía.");
    }

    private static SqlCommand CreateCommand(SqlConnection cn, string storedProcedureName, params object[] parameters)
    {
        SqlCommand cmd = new SqlCommand(storedProcedureName, cn)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (parameters.Length > 0)
        {
            FillStoredProcedureParameters(cmd, parameters);
        }

        return cmd;
    }

    // Para los StoreProcedure INSERT, UPDATE y/o DELETE, que no devuelvan filas
    public static void ExecuteStoredProcedure(IConfiguration configuration, string storedProcedureName, params object[] parameters)
    {
        string _sql = GetConnectionString(configuration);
        using (SqlConnection cn = new SqlConnection(_sql))
        {
            cn.Open();
            SqlCommand cmd = CreateCommand(cn, storedProcedureName, parameters);
            cmd.ExecuteNonQuery();
            cn.Close();
        }
    }

    public static void ExecuteStoredProcedureWithTransaction(IConfiguration configuration, string storedProcedureName, params object[] parameters)
    {
        string _sql = GetConnectionString(configuration);
        using (SqlConnection cn = new SqlConnection(_sql))
        {
            cn.Open();
            SqlTransaction trx = cn.BeginTransaction();

            try
            {
                SqlCommand cmd = CreateCommand(cn, storedProcedureName, parameters);
                cmd.Transaction = trx;
                cmd.ExecuteNonQuery();
                trx.Commit();
            }
            catch (Exception ex)
            {
                trx.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
    }

    // Para los StoreProcedure de Consultas SELECT, donde se devuelvan muchas filas y columnas
    public static SqlDataReader ReturnDataReader(IConfiguration configuration, string storedProcedureName, params object[] parameters)
    {
        string _sql = GetConnectionString(configuration);
        SqlConnection cn = new SqlConnection(_sql);
        cn.Open();
        SqlCommand cmd = CreateCommand(cn, storedProcedureName, parameters);
        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        return reader;
    }

    // Para los StoreProcedure de Consultas SELECT, donde sólo se devuelve 1 fila y 1 columna
    public static object ReturnScalarValue(IConfiguration configuration, string storedProcedureName, params object[] parameters)
    {
        string _sql = GetConnectionString(configuration);
        using (SqlConnection cn = new SqlConnection(_sql))
        {
            cn.Open();
            SqlCommand cmd = CreateCommand(cn, storedProcedureName, parameters);
            object response = cmd.ExecuteScalar();
            cn.Close();
            return response;
        }
    }

    // Llenar paráetros necesarios para los StoreProcedure
    private static void FillStoredProcedureParameters(SqlCommand cmd, params object[] arguments)
    {
        int index = 0;
        SqlCommandBuilder.DeriveParameters(cmd);

        foreach (SqlParameter parameter in cmd.Parameters)
        {
            if (parameter.ParameterName != "@RETURN_VALUE")
            {
                parameter.Value = arguments[index];
                index++;
            }
        }
    }
}
