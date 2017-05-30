using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VaccAppWebservice;

namespace VaccAppWebservice.Controllers
{
    public class vaccine_programController : ApiController
    {
        private VaccineContext db = new VaccineContext();

        // GET: api/vaccine_program
        public IQueryable<vaccine_program> Getvaccine_program()
        {
            return db.vaccine_program;
        }

        // GET: api/vaccine_program/5
        [ResponseType(typeof(vaccine_program))]
        public async Task<IHttpActionResult> Getvaccine_program(int id)
        {
            vaccine_program vaccine_program = await db.vaccine_program.FindAsync(id);
            if (vaccine_program == null)
            {
                return NotFound();
            }

            return Ok(vaccine_program);
        }

        // PUT: api/vaccine_program/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putvaccine_program(int id, vaccine_program vaccine_program)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vaccine_program.program_id)
            {
                return BadRequest();
            }

            db.Entry(vaccine_program).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vaccine_programExists(id))
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

        // POST: api/vaccine_program
        [ResponseType(typeof(vaccine_program))]
        public async Task<IHttpActionResult> Postvaccine_program(vaccine_program vaccine_program)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vaccine_program.Add(vaccine_program);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vaccine_program.program_id }, vaccine_program);
        }

        // DELETE: api/vaccine_program/5
        [ResponseType(typeof(vaccine_program))]
        public async Task<IHttpActionResult> Deletevaccine_program(int id)
        {
            vaccine_program vaccine_program = await db.vaccine_program.FindAsync(id);
            if (vaccine_program == null)
            {
                return NotFound();
            }

            db.vaccine_program.Remove(vaccine_program);
            await db.SaveChangesAsync();

            return Ok(vaccine_program);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vaccine_programExists(int id)
        {
            return db.vaccine_program.Count(e => e.program_id == id) > 0;
        }
    }
}