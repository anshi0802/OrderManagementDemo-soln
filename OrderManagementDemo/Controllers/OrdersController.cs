using Microsoft.AspNetCore.Mvc;
using OrderManagementDemo.Model;
using OrderManagementDemo.Repository;
using OrderManagementDemo.ViewModel;

namespace OrderManagementDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // Call Repository
        private readonly IOrderRepository _repository;

        // Dependency Injection
        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }

        #region 1 - Get all orders - search all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllOrders()
        {
            var orders = await _repository.GetTblOrder();
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        #endregion

        #region 2 - Get all orders using ViewModel
        [HttpGet("vm")]
        public async Task<ActionResult<IEnumerable<CustOrderViewModel>>> GetAllOrdersByViewModel()
        {
            var orders = await _repository.GetOrderViewModel();
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        #endregion

        #region 3 - Get order by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderTable>> GetOrderById(int id)
        {
            var order = await _repository.GetOrderById(id);
            if (order == null)
            {
                return NotFound("No order found");
            }
            return Ok(order);
        }
        #endregion

        #region 4 - Insert an order - return order record
        [HttpPost]
        public async Task<ActionResult<OrderTable>> InsertOrderTableReturnRecord(OrderTable orderTable)
        {
            if (ModelState.IsValid)
            {
                var newOrder = await _repository.PostOrderTableReturnRecord(orderTable);
                if (newOrder != null)
                {
                    return Ok(newOrder);
                }
                else
                {
                    return NotFound("Failed to insert order");
                }
            }
            return BadRequest("Invalid data");
        }
        #endregion

        #region 5 - Insert an order - return order ID
        [HttpPost("v1")]
        public async Task<ActionResult<int>> InsertOrderTableReturnId(OrderTable orderTable)
        {
            if (ModelState.IsValid)
            {
                var newOrderId = await _repository.PostOrderTableReturnId(orderTable);
                if (newOrderId != null)
                {
                    return Ok(newOrderId);
                }
                else
                {
                    return NotFound("Failed to insert order");
                }
            }
            return BadRequest("Invalid data");
        }
        #endregion

        #region 6 - Update order
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderTable>> UpdateOrderTable(int id, OrderTable orderTable)
        {
            if (ModelState.IsValid)
            {
                var updatedOrder = await _repository.PutOrder(id, orderTable);
                if (updatedOrder != null)
                {
                    return Ok(updatedOrder);
                }
                else
                {
                    return NotFound("Order not found");
                }
            }
            return BadRequest("Invalid data");
        }
        #endregion

        #region 7 - Delete order
       [HttpDelete("{id}")]
public IActionResult DeleteOrder(int id)
{
    try
    {
        var result = _repository.DeleteOrder(id);

        dynamic resultValue = result.Value;

        if (resultValue.success)
        {
            return Ok(new { success = true, message = "Order deleted successfully" });
        }
        return NotFound(new { success = false, message = resultValue.message });
    }
    catch (Exception ex)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
            success = false,
            message = "Error occurred while deleting order"
        });
    }
}

        #endregion
    }
}
