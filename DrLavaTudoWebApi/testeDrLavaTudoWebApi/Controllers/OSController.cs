using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using testeDrLavaTudoWebApi.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace testeDrLavaTudoWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class OSController : ApiController
    {
        private BDLAVATUDOEntities db = new BDLAVATUDOEntities();

        // GET: api/OS
        public List<dynamic> GetOS(int id)
        {
            var cli = from os in db.OS 
                         join cliente in db.CLIENTE
                         on os.IDCLIENTE equals cliente.IDCLIENTE
                         where os.IDOS == id
                         select new { cliente.NOME,cliente.TELRES,cliente.TELCEL,cliente.EMAIL,cliente.DATANASC };

            var ordemservico = from os in db.OS 
                               join cliente in db.CLIENTE
                               on os.IDCLIENTE equals cliente.IDCLIENTE
                               where os.IDOS == id
                         select new { os.DATAEMISSAO,os.IDOS, cliente.NOME };

            var servicos = from os in db.OS 
                         join cliente in db.CLIENTE
                         on os.IDCLIENTE equals cliente.IDCLIENTE
                         join itens in db.OSITENS
                         on os.IDOS equals itens.IDOS
                         join servico in db.SERVICO
                         on itens.IDSERVICO equals servico.IDSERVICO
                         where os.IDOS == id
                         select new { servico.IDSERVICO,servico.NOME,servico.VALORFINAL, itens.DATAEXECUCAO };


            var end = from os in db.OS 
                         join cliente in db.CLIENTE
                         on os.IDCLIENTE equals cliente.IDCLIENTE
                         join endereco in db.ENDERECO
                         on cliente.CEP equals endereco.CEP
                         where os.IDOS == id
                         select new { endereco.RUA,cliente.NUMEROIMOVEL,endereco.BAIRRO,endereco.CIDADE,endereco.CEP,endereco.UF };

            List<dynamic> result = new List<dynamic>();
            result.Add(cli);
            result.Add(ordemservico);
            result.Add(servicos);
            result.Add(end);

            return result;
        }

        // PUT: api/OS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOS(int id, OS oS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oS.IDOS)
            {
                return BadRequest();
            }

            db.Entry(oS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OSExists(id))
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

        // POST: api/OS
        [ResponseType(typeof(OS))]
        public IHttpActionResult PostOS(OS oS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OS.Add(oS);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = oS.IDOS }, oS);
        }

        // DELETE: api/OS/5
        [ResponseType(typeof(OS))]
        public IHttpActionResult DeleteOS(int id)
        {
            OS oS = db.OS.Find(id);
            if (oS == null)
            {
                return NotFound();
            }

            db.OS.Remove(oS);
            db.SaveChanges();

            return Ok(oS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OSExists(int id)
        {
            return db.OS.Count(e => e.IDOS == id) > 0;
        }
    }
}