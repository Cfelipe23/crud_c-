using Microsoft.AspNetCore.Mvc;
using crud_c_.Modules.Clientes.Application;
using crud_c_.Modules.Clientes.Domain;
using Swashbuckle.AspNetCore.Filters;
using crud_c_.Swagger.Examples;

namespace crud_c_.Controllers;

/// <summary>
/// Controlador para gestionar operaciones CRUD sobre Clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
  private readonly IClienteRepository _repository;

  public ClientesController(IClienteRepository repository)
  {
    _repository = repository;
  }

  /// <summary>
  /// Obtiene la lista de todos los clientes.
  /// </summary>
  /// <returns>Lista de clientes</returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
  {
    var clientes = await _repository.GetAllAsync();
    return Ok(clientes);
  }

  /// <summary>
  /// Obtiene un cliente por su Id.
  /// </summary>
  /// <param name="id">Identificador del cliente</param>
  /// <returns>El cliente solicitado o 404 si no existe</returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<Cliente>> GetCliente(int id)
  {
    var cliente = await _repository.GetByIdAsync(id);
    if (cliente == null)
    {
      return NotFound();
    }
    return Ok(cliente);
  }

  /// <summary>
  /// Crea un nuevo cliente.
  /// </summary>
  /// <param name="cliente">Objeto cliente a crear</param>
  /// <returns>El cliente creado con su Id</returns>
  [HttpPost]
  [SwaggerRequestExample(typeof(Cliente), typeof(CreateClienteRequestExample))]
  [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ClienteResponseExample))]
  public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
  {
    await _repository.AddAsync(cliente);
    return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
  }

  /// <summary>
  /// Actualiza un cliente existente.
  /// </summary>
  /// <param name="id">Id del cliente a actualizar</param>
  /// <param name="cliente">Objeto cliente con los nuevos datos</param>
  /// <returns>204 NoContent si se actualiza correctamente</returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
  {
    if (id != cliente.Id)
    {
      return BadRequest();
    }

    var existingCliente = await _repository.GetByIdAsync(id);
    if (existingCliente == null)
    {
      return NotFound();
    }

    await _repository.UpdateAsync(cliente);
    return NoContent();
  }

  /// <summary>
  /// Elimina un cliente por Id.
  /// </summary>
  /// <param name="id">Id del cliente a eliminar</param>
  /// <returns>204 NoContent si se elimina correctamente</returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCliente(int id)
  {
    var cliente = await _repository.GetByIdAsync(id);
    if (cliente == null)
    {
      return NotFound();
    }

    await _repository.DeleteAsync(id);
    return NoContent();
  }
}