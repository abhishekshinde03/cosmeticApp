using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cosmeticApp.Data;
using cosmeticApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace cosmeticApp.Controllers
{
    public class usersController : Controller
    {
        private readonly cosmeticAppContext _context;

        public usersController(cosmeticAppContext context)
        {
            _context = context;
        }

        // GET: users
        public async Task<IActionResult> Index()
        {
            return View(await _context.users.ToListAsync());
        }

        // GET: users/Details/5
      

        // GET: users/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(string na, string pa)
        {
            SqlConnection conn1 = new SqlConnection("Server = cosmeticapp.database.windows.net; Database = cosmeticdb; User Id = adminrole; Password = admin@12345");
            string sql;
            sql = "SELECT * FROM users where name ='" + na + "' and  password ='" + pa + "' ";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {
                string role = (string)reader["role"];
                string id = Convert.ToString((int)reader["Id"]);
                HttpContext.Session.SetString("Name", na);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("UserId", id);
                reader.Close();
                conn1.Close();
                if (role == "customer")
                    return RedirectToAction("store", "products");

                else
                    return RedirectToAction("Index", "products");

            }
            else
            {
                ViewData["Message"] = "Entered password or username is inncorrect";
                return View();
            }
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,password,email")] users users)
        {
            users.role = "customer";
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(login));
            
        }

        // GET: users/Edit/5
        public async Task<IActionResult> Edit()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("useri d"));

            var users = await _context.users.FindAsync(id);
            
            return View(users);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,password,role,email")] users users)
        {
           
                    _context.Update(users);
                    await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(login));
           
        }

        

        private bool usersExists(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }
    }
}
