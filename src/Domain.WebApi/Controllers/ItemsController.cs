using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure;
using Inventory.Api.Queries.GetItems;
using Microsoft.AspNetCore.Mvc;

namespace Domain.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        readonly IMediator mediator;

        public ItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetItemsQueryResponse>> GetOrders([FromRoute] GetItemsQuery query, CancellationToken token) =>
            await mediator.Query<GetItemsQuery, GetItemsQueryResponse>(query, token);

        [HttpGet("{ItemId:guid}")]
        public async Task<ActionResult<GetItemQueryResponse>> GetOrder([FromRoute] GetItemQuery query, CancellationToken token) =>
            await mediator.Query<GetItemQuery, GetItemQueryResponse>(query, token);
    }
}
