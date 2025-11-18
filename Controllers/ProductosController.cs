using Microsoft.AspNetCore.Mvc;
using crud_c_.Modules.Productos.Application;
using crud_c_.Modules.Productos.Domain;
using Swashbuckle.AspNetCore.Filters;
using crud_c_.Swagger.Examples;

namespace crud_c_.Controllers;

/// <summary>
/// Controlador para gestionar Productos.
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
  /// Obtiene todos los productos.
  /// </summary>
  /// <returns>Lista de productos</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
  {
    var productos = await _repository.GetAllAsync();
    return Ok(productos);
  }

  /// <summary>
  /// Obtiene un producto por su Id.
  /// </summary>
  /// <param name="id">Identificador del producto</param>
  /// <returns>El producto solicitado o 404 si no existe</returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<Producto>> GetProducto(int id)
  {
    var producto = await _repository.GetByIdAsync(id);
    if (producto == null)
    {
      return NotFound();
    }
    return Ok(producto);
  }

  /// <summary>
  /// Crea un nuevo producto.
  /// </summary>
  /// <param name="producto">Objeto producto a crear</param>
  /// <returns>El producto creado</returns>
  [HttpPost]
  [SwaggerRequestExample(typeof(Producto), typeof(CreateProductoRequestExample))]
  [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ProductoResponseExample))]
  public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
  {
    await _repository.AddAsync(producto);
    return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
  }

  /// <summary>
  /// Actualiza un producto existente.
  /// </summary>
  /// <param name="id">Id del producto a actualizar</param>
  /// <param name="producto">Objeto producto con los nuevos datos</param>
  /// <returns>204 NoContent si se actualiza correctamente</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateProducto(int id, Producto producto)
  {
    if (id != producto.Id)
    {
      return BadRequest();
    }

    var existingProducto = await _repository.GetByIdAsync(id);
    if (existingProducto == null)
    {
      return NotFound();
    }

    await _repository.UpdateAsync(producto);
    return NoContent();
  }

  /// <summary>
  /// Elimina un producto por Id.
  /// </summary>
  /// <param name="id">Id del producto a eliminar</param>
  /// <returns>204 NoContent si se elimina correctamente</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProducto(int id)
  {
    var producto = await _repository.GetByIdAsync(id);
    if (producto == null)
    {
      return NotFound();
    }

    await _repository.DeleteAsync(id);
    return NoContent();
  }
}