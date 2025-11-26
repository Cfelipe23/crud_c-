using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClienteCRUD.Modules.Productos.Application;
using ClienteCRUD.Modules.Productos.Domain;

namespace ClienteCRUD.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de Productos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repository;

        public ProductosController(IProductoRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        /// <returns>Lista de productos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
        {
            try
            {
                var productos = await _repository.GetAllAsync();
                return Ok(productos);
            }
            catch
            {
                return StatusCode(500, "Error al obtener los productos");
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            try
            {
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                    return NotFound(new { message = "Producto no encontrado" });

                return Ok(producto);
            }
            catch
            {
                return StatusCode(500, "Error al obtener el producto");
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="producto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        [HttpPost]
        public async Task<ActionResult<Producto>> Create([FromBody] Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(producto.Nombre))
                    return BadRequest("El nombre del producto es obligatorio");

                if (producto.Precio < 0)
                    return BadRequest("El precio debe ser positivo");

                if (producto.Stock < 0)
                    return BadRequest("El stock no puede ser negativo");

                var nuevoProducto = await _repository.AddAsync(producto);
                return CreatedAtAction(nameof(GetById), new { id = nuevoProducto.Id }, nuevoProducto);
            }
            catch
            {
                return StatusCode(500, "Error al crear el producto");
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        /// <param name="id">ID del producto a actualizar</param>
        /// <param name="producto">Nuevos datos del producto</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var productoExistente = await _repository.GetByIdAsync(id);
                if (productoExistente == null)
                    return NotFound(new { message = "Producto no encontrado" });

                producto.Id = id;
                var resultado = await _repository.UpdateAsync(producto);

                if (resultado)
                    return Ok(new { message = "Producto actualizado correctamente" });

                return StatusCode(500, "Error al actualizar el producto");
            }
            catch
            {
                return StatusCode(500, "Error al actualizar el producto");
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id">ID del producto a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                    return NotFound(new { message = "Producto no encontrado" });

                var resultado = await _repository.DeleteAsync(id);

                if (resultado)
                    return Ok(new { message = "Producto eliminado correctamente" });

                return StatusCode(500, "Error al eliminar el producto");
            }
            catch
            {
                return StatusCode(500, "Error al eliminar el producto");
            }
        }
    }
}
