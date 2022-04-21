using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cosmeticApp.Data;
using cosmeticApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace cosmeticApp.Controllers
{
    public class ordersController : Controller
    {
        private readonly cosmeticAppContext _context;

        public ordersController(cosmeticAppContext context)
        {
            _context = context;
        }

        // GET: orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.orders.ToListAsync());
        }

        // GET: orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.ordersId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: orders/Create
        public async Task<IActionResult> Create(int? id)
        {
            var product = await _context.product.FindAsync(id);

            return View(product);
        }


        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ProductId, int noofproduct )
        {
            orders order = new orders();
            order.ProductId = ProductId;
            order.noofproduct = noofproduct;

            order.Userid = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
            order.orderdate = DateTime.Today;
            _context.Add(order);

            SqlConnection conn = new SqlConnection("Server = cosmeticapp.database.windows.net; Database = cosmeticdb; User Id = adminrole; Password = admin@12345");
            string sql;
            sql = "UPDATE product  SET productquantity  = productquantity   - '" + order.noofproduct + "'  where (productId ='" + order.ProductId + "' )";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();

            _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(myorders));
           
        }
        public async Task<IActionResult> myorders()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
            var orItems = await _context.orders.FromSqlRaw("select *  from orders where  Userid = '" + id + "'  ").ToListAsync();
            return View(orItems);

        }

        public async Task<IActionResult> customeranaylsis()
        {
            var orItems = await _context.analysis.FromSqlRaw("select users.id as Id, name as customername, sum (noofproducts * Price)  as total from product, orders,users where users.id = orders.userid  and PoductId= product.productId group by name,users.id ").ToListAsync();
            return View(orItems);

        }
        public async Task<IActionResult> OrderDetails(int? id)
        {


            var orItems = await _context.orders.FromSqlRaw("select *  from orders where  Userid = '" + id + "'  ").ToListAsync();
            return View(orItems);

        }


        // GET: orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ordersId,ProductId,Userid,noofproduct,orderdate")] orders orders)
        {
            if (id != orders.ordersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ordersExists(orders.ordersId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.orders
                .FirstOrDefaultAsync(m => m.ordersId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.orders.FindAsync(id);
            _context.orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ordersExists(int id)
        {
            return _context.orders.Any(e => e.ordersId == id);
        }
    }
}
