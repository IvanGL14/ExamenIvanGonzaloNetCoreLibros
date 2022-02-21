using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenIvanGonzaloNetCoreLibros.Filters
{
    public class AuthorizeUsuariosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated == false)
            {
                string controller = context.RouteData.Values["controller"].ToString();
                string action = context.RouteData.Values["action"].ToString();
                string idlibro = "";

                ITempDataProvider provider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
                //DEBEMOS RECUPERAR EXACTAMENTE EL OBJETO TEMPDATA QUE ESTÁ UTILIZANDO EL CONTROLLER
                var tempdata = provider.LoadTempData(context.HttpContext);
                //ALMACENAMOS LA INFORMACIÓN COMO SIEMPRE 
                tempdata["controller"] = controller;
                tempdata["action"] = action;
                if (context.RouteData.Values.ContainsKey("idlibro"))
                {
                    idlibro = context.RouteData.Values["idlibro"].ToString();
                    tempdata["idlibro"] = idlibro;
                }

                //ALMACENAMOS EL TEMPDATA DENTRO DE PROVIDER
                provider.SaveTempData(context.HttpContext, tempdata);

                context.Result = this.GetRouteRedirect("Manage", "Login");
            }
       
        }

        private RedirectToRouteResult GetRouteRedirect(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                controller = controller,
                action = action,
            });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }

    }
}
