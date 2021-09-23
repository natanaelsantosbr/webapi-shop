using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers
{

    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "get";
        }

        [HttpGet]
        [Route("{id:int}")]
        public string GetById(int id)
        {
            return $"get {id}";
        }

        [HttpPost]
        [Route("")]
        public Category Post([FromBody] Category model)
        {
            /*
                From Body: Model Bind - lIGAR O JSON COM O C#
            */

            return model;
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