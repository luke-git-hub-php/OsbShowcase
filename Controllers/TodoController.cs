using Microsoft.AspNetCore.Mvc;
using OsbShowcase.Services;
using System.Net.Mime;
using System.Threading.Tasks;
using OsbShowcase.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace OsbShowcase.Controllers
{
    /// <summary>
    /// Controlador de Todos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TodosController : ControllerBase
    {
        private readonly TodoService _service;

        public TodosController(TodoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista Todos. Busca somente um se o id for passado
        /// </summary>  
        /// <param name="todoId">Id do Todo</param>
        /// <returns>Lita de Todos ou somente um Todo se o id for passado</returns>
        /// <response code="200">Retorna a lista de Todos ou o Todo com o id especificado</response>
        /// <response code="404">Se não há Todo com o id especificado</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index([FromRoute] long? todoId)
        {
            var response = new ApiResponse();
            var todos = _service.GetTodos(todoId);

            if (todos.Count == 0)
            {
                response.Message = "Nenhum item encontrado";
                return NotFound(response);
            }

            response.Data = todos;
            return Ok(response);
        }

        /// <summary>
        /// Cria um novo Todo
        /// </summary>
        /// <remarks>
        /// Quem define em que momento o Todo é criado é a Api, logo não é necessário passar o campo <code>CreatedAt</code>.
        /// </remarks>
        /// <param name="data">Dados do todo que será criado</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TodoDto data)
        {
            var response = new ApiResponse();

            try
            {
                var todo = await _service.CreateTodo(data);
                if (todo == null)
                {
                    throw new Exception("Não foi possível adicionar esse item.");
                };

                response.Data = todo;
                return CreatedAtRoute("Todos", response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Atualiza um Todo
        /// </summary>
        /// <param name="data">Dados de atualização do Todo</param>
        /// <returns>Retorna o resultado da atualização do Todo</returns>
        /// <response code="200">Todo atualizado</response>
        /// <response code="400">Erro na atualização do Todo</response>
        /// <response code="404">Todo não encontrado</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] TodoDto data)
        {
            var response = new ApiResponse();

            try
            {
                var todo = await _service.UpdateTodo(data);
                response.Data = todo;

                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;

                switch (e)
                {
                    case KeyNotFoundException:
                        return NotFound(response);
                    default:
                        return BadRequest(response);
                }
            }
        }

        /// <summary>
        /// Remove um Todo
        /// </summary>
        /// <param name="todoId">Id do Todo</param>
        /// <returns>Retorna o resultado da remoção do Todo</returns>
        /// <response code="200">Todo removido</response>
        /// <response code="404">Todo não encontrado</response>
        [HttpDelete("{todoId:long?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(long? todoId)
        {
            var response = new ApiResponse();

            try
            {
                await _service.DeleteTodo(todoId);

                response.Message = "Item removido com sucesso";
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return BadRequest(response);
            }
        }
    }
}