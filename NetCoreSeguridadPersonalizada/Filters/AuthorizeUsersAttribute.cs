using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadPersonalizada.Filters
{
    public class AuthorizeUsersAttribute :
        AuthorizeAttribute, IAuthorizationFilter
    {
        //ESTE METODO ES EL QUE PERMITIRA IMPEDIR EL ACCESO A LOS ACTION/CONTROLLER
        //EL FILTER SIMPLEMENTE SE ENCARGA DE INTERCEPTAR PETICIONES Y DECIDIR QUE HACER
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //EL USUARIO QUE SE HA VALIDADO EN NUESTRA APP
            //ESTARA DENTRO DE CONTEXT Y UNA PROPIEDAD LLAMADA USER
            //CUALQUIER USER ESTA COMPUESTO POR DOS CARACTERISITICAS:
            //1: SI Identity: El nombre del usuario y si es activo
            //2: Principal: El rol del usuario
            var user = context.HttpContext.User;
            //EL FILTRO SOLO PREGUNTARA SI EXISTE EL USER Y SOLO ENTRA EN ACCION SI EXSTE
            if (user.Identity.IsAuthenticated == false)
            {
                //Lo llevamos al login
                RouteValueDictionary rutalogin = new RouteValueDictionary(
                    new{
                    controller = "Managed",
                    action = "LogIn"
                    });
                //Devolvemos la peticion al login
                context.Result = new RedirectToRouteResult(rutalogin);
            }
        }
    }
}
