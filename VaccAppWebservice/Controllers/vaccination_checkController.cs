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
    public class vaccination_checkController : ApiController
    {
        private VaccineContext db = new VaccineContext();

        // GET: api/vaccination_check
        public IQueryable<vaccination_check> Getvaccination_check()
        {
            return db.vaccination_check;
        }

        // GET: api/vaccination_check/5
        [ResponseType(typeof(vaccination_check))]
        public async Task<IHttpActionResult> Getvaccination_check(int id)
        {
            vaccination_check vaccination_check = await db.vaccination_check.FindAsync(id);
            if (vaccination_check == null)
            {
                return NotFound();
            }

            return Ok(vaccination_check);
        }

        // PUT: api/vaccination_check/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putvaccination_check(int id, vaccination_check vaccination_check)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vaccination_check.vac_check_id)
            {
                return BadRequest();
            }

            db.Entry(vaccination_check).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vaccination_checkExists(id))
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

        // POST: api/vaccination_check
        [ResponseType(typeof(vaccination_check))]
        public async Task<IHttpActionResult> Postvaccination_check(vaccination_check vaccination_check)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vaccination_check.Add(vaccination_check);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vaccination_check.vac_check_id }, vaccination_check);
        }

        // DELETE: api/vaccination_check/5
        [ResponseType(typeof(vaccination_check))]
        public async Task<IHttpActionResult> Deletevaccination_check(int id)
        {
            vaccination_check vaccination_check = await db.vaccination_check.FindAsync(id);
            if (vaccination_check == null)
            {
                return NotFound();
            }

            db.vaccination_check.Remove(vaccination_check);
            await db.SaveChangesAsync();

            return Ok(vaccination_check);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vaccination_checkExists(int id)
        {
            return db.vaccination_check.Count(e => e.vac_check_id == id) > 0;
        }
    }
}