using Microsoft.AspNetCore.Mvc;
using crud_c_.Modules.Lotes.Application;
using crud_c_.Modules.Lotes.Domain;
using crud_c_.Modules.Productos.Application;
using Swashbuckle.AspNetCore.Filters;
using crud_c_.Swagger.Examples;

namespace crud_c_.Controllers;

/// <summary>
/// Controlador para gestionar Lotes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LotesController : ControllerBase
{
  private readonly ILoteRepository _repository;
  private readonly IProductoRepository _productoRepository;

  public LotesController(ILoteRepository repository, IProductoRepository productoRepository)
  {
    _repository = repository;
    _productoRepository = productoRepository;
  }

  /// <summary>
  /// Obtiene todos los lotes.
  /// </summary>
  /// <returns>Lista de lotes</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Lote>>> GetLotes()
  {
    var lotes = await _repository.GetAllAsync();
    return Ok(lotes);
  }

  /// <summary>
  /// Obtiene un lote por su Id.
  /// </summary>
  /// <param name="id">Identificador del lote</param>
  /// <returns>El lote solicitado o 404 si no existe</returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<Lote>> GetLote(int id)
  {
    var lote = await _repository.GetByIdAsync(id);
    if (lote == null)
    {
      return NotFound();
    }
    return Ok(lote);
  }

  /// <summary>
  /// Crea un nuevo lote.
  /// </summary>
  /// <param name="lote">Objeto lote a crear</param>
  /// <returns>El lote creado</returns>
  [HttpPost]
  [SwaggerRequestExample(typeof(Lote), typeof(CreateLoteRequestExample))]
  [SwaggerResponseExample(StatusCodes.Status201Created, typeof(LoteResponseExample))]
  public async Task<ActionResult<Lote>> CreateLote(Lote lote)
  {
    // Validar FK: el producto debe existir
    var producto = await _productoRepository.GetByIdAsync(lote.ProductoId);
    if (producto == null)
    {
      return BadRequest($"ProductoId {lote.ProductoId} no existe.");
    }
    await _repository.AddAsync(lote);
    return CreatedAtAction(nameof(GetLote), new { id = lote.Id }, lote);
  }

  /// <summary>
  /// Actualiza un lote existente.
  /// </summary>
  /// <param name="id">Id del lote a actualizar</param>
  /// <param name="lote">Objeto lote con los nuevos datos</param>
  /// <returns>204 NoContent si se actualiza correctamente</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateLote(int id, Lote lote)
  {
    if (id != lote.Id)
    {
      return BadRequest();
    }

    var existingLote = await _repository.GetByIdAsync(id);
    if (existingLote == null)
    {
      return NotFound();
    }

    // Validar FK cuando se actualiza el ProductoId
    if (lote.ProductoId != existingLote.ProductoId)
    {
      var producto = await _productoRepository.GetByIdAsync(lote.ProductoId);
      if (producto == null)
      {
        return BadRequest($"ProductoId {lote.ProductoId} no existe.");
      }
    }

    await _repository.UpdateAsync(lote);
    return NoContent();
  }

  /// <summary>
  /// Elimina un lote por Id.
  /// </summary>
  /// <param name="id">Id del lote a eliminar</param>
  /// <returns>204 NoContent si se elimina correctamente</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteLote(int id)
  {
    var lote = await _repository.GetByIdAsync(id);
    if (lote == null)
    {
      return NotFound();
    }

    await _repository.DeleteAsync(id);
    return NoContent();
  }
}