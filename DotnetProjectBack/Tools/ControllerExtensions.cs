using DotnetProjectBack.Models;
using Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotnetProjectBack.Tools
{
    public static class ControllerExtensions
    {
        public static IActionResult GetResultFromResponse<T>(this Controller controller, GenericResponse<T> res)
        {
            if (res.Success)
            {
                return controller.Ok(res.Result);
            }
            else
            {
                return controller.BadRequest(new ErrorResponse(res.ErrorMessage));
            }
        }
    }
}
