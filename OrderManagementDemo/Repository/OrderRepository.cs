using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementDemo.Model;
using OrderManagementDemo.ViewModel;

namespace OrderManagementDemo.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderMgntDbContext _context;

        public OrderRepository(OrderMgntDbContext context)
        {
            _context = context;
        }

        #region 1 - Get all Orders
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetTblOrder()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.OrderTables.Include(order => order.Customer).Include(order => order.OrderItem).ToListAsync();
                }
                // Returns an empty list if context is null
                return new List<OrderTable>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 2 - Order ViewModel
        public async Task<ActionResult<IEnumerable<CustOrderViewModel>>> GetOrderViewModel()
        {
            try
            {
                if (_context != null)
                {
                    return await (from o in _context.OrderTables
                                  join c in _context.Customers on o.CustomerId equals c.CustomerId
                                  join oi in _context.OrderItems on o.OrderItemId equals oi.OrderItemId
                                  join i in _context.Items on oi.ItemId equals i.ItemId
                                  select new CustOrderViewModel
                                  {
                                      CustomerId = c.CustomerId,
                                      CustomerName = c.CustomerName,
                                      ItemName = i.ItemName,
                                      Price = i.Price,
                                      Quantity = oi.Quantity,
                                      OrderDate = o.OrderDate
                                  }).ToListAsync();
                }
                // Returns an empty list if context is null
                return new List<CustOrderViewModel>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 3 - Get an order by its Id
        public async Task<ActionResult<OrderTable>> GetOrderById(int id)
        {
            try
            {
                if (_context != null)
                {
                    var custOrder = await _context.OrderTables.Include(order => order.Customer).Include(order => order.OrderItem).FirstOrDefaultAsync(e => e.OrderId == id);
                    return custOrder;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 4 - Insert an order and return the order record
        public async Task<ActionResult<OrderTable>> PostOrderTableReturnRecord(OrderTable orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new ArgumentNullException(nameof(orderTable), "Order data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("db context not initialised");
                }

                await _context.OrderTables.AddAsync(orderTable);
                await _context.SaveChangesAsync();

                var orderWithDetails = await _context.OrderTables.Include(o => o.Customer).Include(o => o.OrderItem).FirstOrDefaultAsync(o => o.OrderId == orderTable.OrderId);

                return orderWithDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 5 - Insert an order and return the order ID
        public async Task<ActionResult<int>> PostOrderTableReturnId(OrderTable orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new ArgumentNullException(nameof(orderTable), "Order data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("db context not initialised");
                }

                await _context.OrderTables.AddAsync(orderTable);
                var changesRecord = await _context.SaveChangesAsync();

                if (changesRecord > 0)
                {
                    return orderTable.OrderId;
                }
                else
                {
                    throw new Exception("Failed to save order record to the database");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 6 - Update an order with ID and order
        public async Task<ActionResult<OrderTable>> PutOrder(int id, OrderTable orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new ArgumentNullException(nameof(orderTable), "Order data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("db context not initialised");
                }

                var existingOrder = await _context.OrderTables.FindAsync(id);
                if (existingOrder == null)
                {
                    return null;
                }

                existingOrder.OrderDate = orderTable.OrderDate;
                existingOrder.CustomerId = orderTable.CustomerId;
                existingOrder.OrderItemId = orderTable.OrderItemId;

                await _context.SaveChangesAsync();

                var orderWithDetails = await _context.OrderTables.Include(o => o.Customer).Include(o => o.OrderItem).FirstOrDefaultAsync(o => o.OrderId == id);

                return orderWithDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 7 - Delete an order

        public JsonResult DeleteOrder(int id)
{
    try
    {
        if (_context == null)
        {
            throw new InvalidOperationException("db context not initialised");
        }

        var order = _context.OrderTables.Find(id);
        if (order == null)
        {
            return new JsonResult(new { success = false, message = "Order could not be deleted or not found" });
        }

        _context.OrderTables.Remove(order);
        _context.SaveChanges();

        return new JsonResult(new { success = true, message = "Order deleted successfully" });
    }
    catch (Exception ex)
    {
        return new JsonResult(new { success = false, message = "Error occurred while deleting order" });
    }
}


        


        #endregion
    }
}
