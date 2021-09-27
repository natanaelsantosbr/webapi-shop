using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{

    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(category);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id,
        [FromServices] DataContext context)
        {
            /*
            AsNoTracking = Pega os dados puros ou seja ele nao cria um proxy da categoria (Versao de atualizacao, criacao, deleção que eh de uso exclusivo do EF)
            */
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Post([FromBody] Category model,
        [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Não foi possível criar a categoria {ex.Message}" });
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
        {
            //Verifica se o ID informado é o mesmo do modelo            
            if (model.Id != id)
                return NotFound(new { message = "Categoria não encontrada" });

            //Verifica se os dados sao validos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });
            }
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Delete(int id,
        [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return BadRequest(new { message = "Categoria nao encontrada" });

            try
            {
                context.Remove(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch
            {
                return BadRequest(new { message = "Nao foi possivel remover uma categoria" });
            }
        }
    }
}
