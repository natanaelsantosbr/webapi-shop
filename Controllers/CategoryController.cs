using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    //Endpoint a mesma coisa de uma URL
    //https://localhost:5051/categories/
    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [Route("")]
        public string MeuMetodo()
        {
            return "Ola Mundo";
        }
    }
}