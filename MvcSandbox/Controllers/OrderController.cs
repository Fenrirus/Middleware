using Microsoft.AspNetCore.Mvc;
using MvcSandbox.Models;
using System.Collections.Generic;

namespace MvcSandbox.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Create(List<Order> order)
        {
            return Content("Binding succesful");
        }
    }
}