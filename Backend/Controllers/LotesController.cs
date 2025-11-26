using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClienteCRUD.Modules.Lotes.Application;
using ClienteCRUD.Modules.Lotes.Domain;

namespace ClienteCRUD.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de Lotes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteRepository _repository;

        public LotesController(ILoteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todos los lotes
        /// </summary>
        /// <returns>Lista de lotes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lote>>> GetAll()
        {
            try
            {
                var lotes = await _repository.GetAllAsync();
                return Ok(lotes);
            }
            catch
            {
                return StatusCode(500, "Error al obtener los lotes");
            }
        }

        /// <summary>
        /// Obtiene un lote por ID
        /// </summary>
        /// <param name="id">ID del lote</param>
        /// <returns>Lote encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Lote>> GetById(int id)
        {
            try
            {
                var lote = await _repository.GetByIdAsync(id);
                if (lote == null)
                    return NotFound(new { message = "Lote no encontrado" });

                return Ok(lote);
            }
            catch
            {
                return StatusCode(500, "Error al obtener el lote");
            }
        }

        /// <summary>
        /// Crea un nuevo lote
        /// </summary>
        /// <param name="lote">Datos del lote a crear</param>
        /// <returns>Lote creado</returns>
        [HttpPost]
        public async Task<ActionResult<Lote>> Create([FromBody] Lote lote)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(lote.Codigo))
                    return BadRequest("El código del lote es obligatorio");

                if (lote.Cantidad < 1)
                    return BadRequest("La cantidad debe ser mayor que 0");

                if (lote.ProductoId < 1)
                    return BadRequest("El ID del producto es obligatorio");

                var nuevoLote = await _repository.AddAsync(lote);
                return CreatedAtAction(nameof(GetById), new { id = nuevoLote.Id }, nuevoLote);
            }
            catch
            {
                return StatusCode(500, "Error al crear el lote");
            }
        }

        /// <summary>
        /// Actualiza un lote existente
        /// </summary>
        /// <param name="id">ID del lote a actualizar</param>
        /// <param name="lote">Nuevos datos del lote</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Lote lote)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var loteExistente = await _repository.GetByIdAsync(id);
                if (loteExistente == null)
                    return NotFound(new { message = "Lote no encontrado" });

                lote.Id = id;
                var resultado = await _repository.UpdateAsync(lote);

                if (resultado)
                    return Ok(new { message = "Lote actualizado correctamente" });

                return StatusCode(500, "Error al actualizar el lote");
            }
            catch
            {
                return StatusCode(500, "Error al actualizar el lote");
            }
        }

        /// <summary>
        /// Elimina un lote
        /// </summary>
        /// <param name="id">ID del lote a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var lote = await _repository.GetByIdAsync(id);
                if (lote == null)
                    return NotFound(new { message = "Lote no encontrado" });

                var resultado = await _repository.DeleteAsync(id);

                if (resultado)
                    return Ok(new { message = "Lote eliminado correctamente" });

                return StatusCode(500, "Error al eliminar el lote");
            }
            catch
            {
                return StatusCode(500, "Error al eliminar el lote");
            }
        }
    }
}
