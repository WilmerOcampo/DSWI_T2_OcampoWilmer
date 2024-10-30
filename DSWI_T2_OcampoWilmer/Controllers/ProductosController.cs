using DSWI_T2_OcampoWilmer.Models;
using DSWI_T2_OcampoWilmer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSWI_T2_OcampoWilmer.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly ICategoriaService _categoriaService;

        public ProductosController(IProductoService productoService, IProveedorService proveedorService, ICategoriaService categoriaService)
        {
            _productoService = productoService;
            _proveedorService = proveedorService;
            _categoriaService = categoriaService;
        }

        public async Task<ActionResult> Productos(string? n = null, int p = 0)
        {
            IEnumerable<Producto> productos = _productoService.Productos(n);

            foreach (var producto in productos) // Iterar y asignar nombres del proveedor y categoria
            {
                producto.NombreProveedor = _proveedorService.Buscar(producto.IdProveedor).Nombre;
                producto.NombreCategoria = _categoriaService.Buscar(producto.IdCategoria).Nombre;
            }
            (int totalPaginas, int desplazamiento, int elementosPorPagina) = PaginationHelper.CalculatePagination(productos, p);
            ViewBag.n = n;
            ViewBag.p = p;
            ViewBag.pags = totalPaginas;

            return View(await Task.Run(() => productos.Skip(desplazamiento).Take(elementosPorPagina)));
        }


        public async Task<ActionResult> Create()
        {
            ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
            ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");
            return View(await Task.Run(() => new Producto() { Id = _productoService.GenerarId() }));
        }

        [HttpPost]
        public async Task<ActionResult> Create(Producto p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
                ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");
                return View(await Task.Run(() => p));
            }

            ViewBag.msg = _productoService.Insert(p);
            ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
            ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");

            return View(await Task.Run(() => p));
        }


        public async Task<ActionResult> Edit(int? id = null)
        {
            if (id == null)
                return RedirectToAction("Productos");

            Producto producto = _productoService.Buscar(id);

            ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
            ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");
            return View(await Task.Run(() => producto));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Producto p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
                ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");
                return View(await Task.Run(() => p));
            }

            ViewBag.msg = _productoService.Update(p);
            ViewBag.proveedores = new SelectList(_proveedorService.Proveedores(), "Id", "Nombre");
            ViewBag.categorias = new SelectList(_categoriaService.Categorias(), "Id", "Nombre");

            return View(await Task.Run(() => p));
        }

        public async Task<IActionResult> Details(int? id = null)
        {
            if (id == null)
                return RedirectToAction("Productos");

            Producto producto = _productoService.Buscar(id);
            producto.NombreCategoria = _categoriaService.Categorias().FirstOrDefault(c => c.Id == producto.IdCategoria)?.Nombre;
            producto.NombreProveedor = _proveedorService.Proveedores().FirstOrDefault(p => p.Id == producto.IdProveedor)?.Nombre;

            return View(await Task.Run(() => producto));
        }


        public async Task<IActionResult> Delete(int? id = null)
        {
            if (id == null)
                return RedirectToAction("Productos");

            Producto producto = _productoService.Buscar(id);
            producto.NombreCategoria = _categoriaService.Categorias().FirstOrDefault(c => c.Id == producto.IdCategoria)?.Nombre;
            producto.NombreProveedor = _proveedorService.Proveedores().FirstOrDefault(p => p.Id == producto.IdProveedor)?.Nombre;

            return View(await Task.Run(() => producto));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Producto p)
        {
            if (p.Id == null)
                return RedirectToAction("Productos");

            _productoService.Delete(p);

            return RedirectToAction("Productos");
        }
    }
}
