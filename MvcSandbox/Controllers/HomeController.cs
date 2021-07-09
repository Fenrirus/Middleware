// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace MvcSandbox.Controllers
{
    [TypeFilter(typeof(OutageAuthorizationFilter))]
    public class HomeController : Controller
    {
        [ModelBinder]
        public string Id { get; set; }

        [Route("/contact-us", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}