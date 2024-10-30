/* OCAMPO QUISPE, WILMER | T2 DSW1*/

USE Negocios2023
GO

/* LITADO DE PRODUCTOS (PRO NOMBRE OPCIONAL) */
CREATE OR ALTER PROC SP_ProductosN
@n VARCHAR(40) = ''
AS
	SELECT P.IdProducto, P.NombreProducto, P.IdProveedor, P.IdCategoria, P.umedida, P.PrecioUnidad, CAST(P.UnidadesEnExistencia AS INT) Stock
    FROM tb_productos P
    WHERE NombreProducto LIKE '%' + @n + '%'
GO


/* GENERAR ID PRODUCTO PARA SU REGISTRO */
CREATE OR ALTER PROC SP_GenerarIdProducto
AS
BEGIN
   SELECT ISNULL(MAX(IdProducto),0) + 1
   FROM tb_productos
END
GO

/* ACTUALIZAR PRODUCTO */
CREATE OR ALTER PROC SP_ActualizarProducto
(
   @id INT,
   @nom VARCHAR(40),
   @idProv INT,
   @idCat INT,
   @uMed VARCHAR(100),
   @pUni DECIMAL(10,0),
   @uExis SMALLINT
)
AS
BEGIN
   UPDATE tb_productos
   SET NombreProducto = @nom, IdProveedor = @idProv, IdCategoria = @idCat, umedida = @uMed, PrecioUnidad = @pUni, UnidadesEnExistencia = @uExis
   WHERE IdProducto = @id
END
GO

/* INSERTAR PRODUCTO */
CREATE OR ALTER PROC SP_InsertarProducto
(
   @id INT,
   @nom VARCHAR(40),
   @idProv INT,
   @idCat INT,
   @uMed VARCHAR(100),
   @pUni DECIMAL(10,0),
   @uExis SMALLINT
)
AS
BEGIN
   INSERT INTO tb_productos(IdProducto,NombreProducto,IdProveedor,IdCategoria,umedida,PrecioUnidad,UnidadesEnExistencia)
   VALUES(@id,@nom,@idProv,@idCat,@uMed, @pUni, @uExis)
END
GO

/* ELIMINAR PRODUCTO */
CREATE OR ALTER PROC SP_EliminarProducto
(
	@id INT
)
AS
BEGIN
   DELETE FROM tb_productos WHERE IdProducto = @id
END
GO

/* LISTAR PROVEEDORES (POR NOMBRE OPCIONAL) */
CREATE OR ALTER PROC SP_Proveedores
(
	@n VARCHAR(40) = ''
)
AS
	SELECT *
	FROM tb_proveedores P
	WHERE NombreCia LIKE '%' + @n + '%' OR NombreContacto LIKE '%' + @n + '%'
GO
EXEC SP_Proveedores
GO

/* LISTAR CATEGORIAS (PRO NOMBRE OPCIONAL) */
CREATE OR ALTER PROC SP_Categorias
(
	@n VARCHAR(15) = ''
)
AS
	SELECT *
	FROM tb_categorias C
	WHERE NombreCategoria LIKE '%' + @n + '%' OR Descripcion LIKE '%' + @n + '%'
GO
EXEC SP_Categorias
GO