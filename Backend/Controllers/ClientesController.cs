using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClienteCRUD.Modules.Clientes.Application;
using ClienteCRUD.Modules.Clientes.Domain;

namespace ClienteCRUD.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de Clientes
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
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            try
            {
                var clientes = await _repository.GetAllAsync();
                return Ok(clientes);
            }
            catch
            {
                return StatusCode(500, "Error al obtener los clientes");
            }
        }

        /// <summary>
        /// Obtiene un cliente por ID
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            try
            {
                var cliente = await _repository.GetByIdAsync(id);
                if (cliente == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                return Ok(cliente);
            }
            catch
            {
                return StatusCode(500, "Error al obtener el cliente");
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>Cliente creado</returns>
        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Apellido))
                    return BadRequest("El nombre y apellido son obligatorios");

                var nuevoCliente = await _repository.AddAsync(cliente);
                return CreatedAtAction(nameof(GetById), new { id = nuevoCliente.Id }, nuevoCliente);
            }
            catch
            {
                return StatusCode(500, "Error al crear el cliente");
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente a actualizar</param>
        /// <param name="cliente">Nuevos datos del cliente</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clienteExistente = await _repository.GetByIdAsync(id);
                if (clienteExistente == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                cliente.Id = id;
                var resultado = await _repository.UpdateAsync(cliente);

                if (resultado)
                    return Ok(new { message = "Cliente actualizado correctamente" });

                return StatusCode(500, "Error al actualizar el cliente");
            }
            catch
            {
                return StatusCode(500, "Error al actualizar el cliente");
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _repository.GetByIdAsync(id);
                if (cliente == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                var resultado = await _repository.DeleteAsync(id);

                if (resultado)
                    return Ok(new { message = "Cliente eliminado correctamente" });

                return StatusCode(500, "Error al eliminar el cliente");
            }
            catch
            {
                return StatusCode(500, "Error al eliminar el cliente");
            }
        }
    }
}
