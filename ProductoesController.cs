using Microsoft.AspNetCore.Mvc;
using TiendaTecnologia.Models;
using System.Linq;

namespace TiendaTecnologia.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult ReporteMasVendidos()
        {
            var productosVendidos = _context.Productos
                .OrderByDescending(p => p.Cantidad)  // Suponiendo que menor cantidad significa mayor venta
                .Take(10)  // Los 10 productos más vendidos
                .ToList();

            return View(productosVendidos);
        }

        public IActionResult ReporteMenorStock()
        {
            var productosMenorStock = _context.Productos
                .Where(p => p.Cantidad <= 5)  // Filtramos productos con stock bajo (puedes modificar este valor)
                .OrderBy(p => p.Cantidad)
                .ToList();

            return View(productosMenorStock);
        }
        public IActionResult Estadisticas()
        {
            var totalValorInventario = _context.Productos.Sum(p => p.Precio * p.Cantidad);
            var cantidadTotalProductos = _context.Productos.Sum(p => p.Cantidad);

            var estadisticas = new
            {
                TotalValorInventario = totalValorInventario,
                CantidadTotalProductos = cantidadTotalProductos
            };

            return View(estadisticas);
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Update(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
