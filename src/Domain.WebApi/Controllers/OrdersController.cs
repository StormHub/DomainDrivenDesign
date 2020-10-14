using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Orders.Api.Commands.CreateOrder;
using Orders.Api.Queries.GetOrders;

namespace Domain.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public sealed class OrdersController : ControllerBase
    {
        readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetOrdersQueryResponse>> GetOrders([FromRoute] GetOrdersQuery query, CancellationToken token) => 
            await mediator.Query<GetOrdersQuery, GetOrdersQueryResponse>(query, token);

        [HttpGet("{OrderId:guid}")]
        public async Task<ActionResult<GetOrderQueryResponse>> GetOrder([FromRoute] GetOrderQuery query, CancellationToken token) =>
            await mediator.Query<GetOrderQuery, GetOrderQueryResponse>(query, token);

        [HttpPost]
        public async Task<ActionResult<GetOrderQueryResponse>> CreateOrder(CreateOrderCommand command, CancellationToken token)
        {
            await mediator.Command(command, token);
            var response = await mediator.Query<GetOrderQuery, GetOrderQueryResponse>(
                new GetOrderQuery
                {
                    OrderId = command.Id
                }, 
                token);

            return response;
        }
    }
}
