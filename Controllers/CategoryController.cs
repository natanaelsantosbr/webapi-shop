using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{

    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public string get()
        {
            return "get";
        }

        [HttpPost]
        [Route("")]
        public string Post()
        {
            return "Post";
        }

        [HttpPut]
        [Route("")]
        public string Put()
        {
            return "Put";
        }

        [HttpDelete]
        [Route("")]
        public string Delete()
        {
            return "Delete";
        }
    }
}