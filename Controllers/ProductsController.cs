using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyFirstApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            return new List<string> { "Apple", "Banana", "Orange" };
        }
        [HttpGet("featured")]//http://localhost:5000/api/products/featured
        public string GetFeaturedProduct() => "Orange";
        [HttpPost]
        public ActionResult<string> Post([FromBody] string newProduct)
        {
            return $"Added: {newProduct}";
        }
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string updatedProduct)
        {
            return $"Updated product {id} to: {updatedProduct}";
        }
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return $"Deleted product with ID: {id}";

        }
        [HttpGet("boom")]//internal server error test: http://localhost:5000/api/products/boom
        public IActionResult Boom()
        {
            throw new Exception("Test exception");
        }
    }

}
