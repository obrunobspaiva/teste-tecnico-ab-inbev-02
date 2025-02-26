using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Criar uma nova venda
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetSaleById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Buscar todas as vendas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<GetSaleResult>>> GetAllSales()
        {
            var query = new GetAllSalesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Buscar uma venda por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSaleResult>> GetSaleById(Guid id)
        {
            var query = new GetSaleByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Atualizar uma venda
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleCommand command)
        {
            if (id != command.Id)
                return BadRequest("O ID da URL não corresponde ao ID do corpo da requisição.");

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Excluir uma venda
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            var command = new DeleteSaleCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<List<GetSaleResult>>> GetSalesWithFilters(
        [FromQuery] string? customer,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortOrder = "desc")
        {
            var query = new GetSalesWithFiltersQuery
            {
                Customer = customer,
                StartDate = startDate,
                EndDate = endDate,
                Page = page,
                PageSize = pageSize,
                SortOrder = sortOrder
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
