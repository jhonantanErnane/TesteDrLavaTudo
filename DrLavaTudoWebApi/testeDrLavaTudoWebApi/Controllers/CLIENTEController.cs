using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using testeDrLavaTudoWebApi.Models;

namespace testeDrLavaTudoWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CLIENTEController : ApiController
    {
        private BDLAVATUDOEntities db = new BDLAVATUDOEntities();

        // GET: api/CLIENTEs
        public List<dynamic> GetCLIENTE(int id)
        {
            var oscliente = from cliente in db.CLIENTE
                            join os in db.OS
                            on cliente.IDCLIENTE equals os.IDCLIENTE
                            join itens in db.OSITENS
                            on os.IDOS equals itens.IDOS into egroup
                            from itens in egroup.DefaultIfEmpty()
                            where cliente.IDCLIENTE == id
                            select new { cliente.IDCLIENTE, cliente.NOME, os.IDOS, os.DATAEMISSAO, itens.DATAEXECUCAO, itens.DATACONTRATACAO };

            List<dynamic> result = new List<dynamic>();
            result.Add(oscliente.Distinct());

            return result;
        }

        // PUT: api/CLIENTEs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCLIENTE(int id, CLIENTE cLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cLIENTE.IDCLIENTE)
            {
                return BadRequest();
            }

            db.Entry(cLIENTE).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CLIENTEExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CLIENTEs
        [ResponseType(typeof(CLIENTE))]
        public IHttpActionResult PostCLIENTE(CLIENTE cLIENTE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CLIENTE.Add(cLIENTE);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CLIENTEExists(cLIENTE.IDCLIENTE))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cLIENTE.IDCLIENTE }, cLIENTE);
        }

        // DELETE: api/CLIENTEs/5
        [ResponseType(typeof(CLIENTE))]
        public IHttpActionResult DeleteCLIENTE(int id)
        {
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return NotFound();
            }

            db.CLIENTE.Remove(cLIENTE);
            db.SaveChanges();

            return Ok(cLIENTE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CLIENTEExists(int id)
        {
            return db.CLIENTE.Count(e => e.IDCLIENTE == id) > 0;
        }
    }
}