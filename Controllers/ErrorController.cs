using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

using test_mvc_app.Models;

namespace test_mvc_app.Controllers {
    public class ErrorController
        : Controller {
            [Route ("Error/{statusCode}")]
            public IActionResult HttpStatusCodeHandler (int statusCode) {
                var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature> ();

                switch (statusCode) {
                    case 404:
                        {
                            ViewBag.ErrorMessage = "Sorry, the resource you requested not found";
                            ViewBag.Path = statusCodeResult.OriginalPath;
                            ViewBag.QS = statusCodeResult.OriginalQueryString;

                            break;
                        }
                }
                return View ("NotFound");
            }

            [Route ("Error")]
            [AllowAnonymous]
            public IActionResult Error () {
                var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature> ();
                ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
                ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
                ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
                return View ("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
}