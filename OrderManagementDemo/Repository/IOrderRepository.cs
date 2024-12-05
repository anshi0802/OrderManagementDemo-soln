
using Microsoft.AspNetCore.Mvc;
using OrderManagementDemo.Model;
using OrderManagementDemo.ViewModel;

namespace OrderManagementDemo.Repository
{
    public interface IOrderRepository
    {
        #region - Get all orders from DB - Search All
        public Task<ActionResult<IEnumerable<OrderTable>>> GetTblOrder();
        #endregion

        #region - Get all orders using ViewModel
        public Task<ActionResult<IEnumerable<CustOrderViewModel>>> GetOrderViewModel();
        #endregion

        #region 3 - Get an order based on Id
        public Task<ActionResult<OrderTable>> GetOrderById(int id);
        #endregion

        #region 4 - Insert an order and return the order record
        public Task<ActionResult<OrderTable>> PostOrderTableReturnRecord(OrderTable orderTable);
        #endregion

        #region 5 - Insert an order and return the order ID
        public Task<ActionResult<int>> PostOrderTableReturnId(OrderTable orderTable);
        #endregion

        #region 6 - Update an order with ID and order
        public Task<ActionResult<OrderTable>> PutOrder(int id, OrderTable orderTable); // ID for editing
        #endregion

        #region 7 - Delete an order
        public JsonResult DeleteOrder(int id);
        #endregion
    }
}
