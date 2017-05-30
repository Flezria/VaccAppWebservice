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
    public class vaccine_infoController : ApiController
    {
        private VaccineContext db = new VaccineContext();

        // GET: api/vaccine_info
        public IQueryable<vaccine_info> Getvaccine_info()
        {
            return db.vaccine_info;
        }

        // GET: api/vaccine_info/5
        [ResponseType(typeof(vaccine_info))]
        public async Task<IHttpActionResult> Getvaccine_info(int id)
        {
            vaccine_info vaccine_info = await db.vaccine_info.FindAsync(id);
            if (vaccine_info == null)
            {
                return NotFound();
            }

            return Ok(vaccine_info);
        }

        [Route("GetVacInfo/{api_key}/{VacInfoID}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVacInfo(String api_key, int VacInfoID)
        {
            if (await db.users.AnyAsync(x => x.api_key == api_key))
            {
                vaccine_info VacInfo = await db.vaccine_info.FindAsync(VacInfoID);

                return Ok(VacInfo);
            }

            return NotFound();
        }

        // PUT: api/vaccine_info/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putvaccine_info(int id, vaccine_info vaccine_info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vaccine_info.vaccineinfo_id)
            {
                return BadRequest();
            }

            db.Entry(vaccine_info).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vaccine_infoExists(id))
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

        // POST: api/vaccine_info
        [ResponseType(typeof(vaccine_info))]
        public async Task<IHttpActionResult> Postvaccine_info(vaccine_info vaccine_info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vaccine_info.Add(vaccine_info);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vaccine_info.vaccineinfo_id }, vaccine_info);
        }

        // DELETE: api/vaccine_info/5
        [ResponseType(typeof(vaccine_info))]
        public async Task<IHttpActionResult> Deletevaccine_info(int id)
        {
            vaccine_info vaccine_info = await db.vaccine_info.FindAsync(id);
            if (vaccine_info == null)
            {
                return NotFound();
            }

            db.vaccine_info.Remove(vaccine_info);
            await db.SaveChangesAsync();

            return Ok(vaccine_info);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vaccine_infoExists(int id)
        {
            return db.vaccine_info.Count(e => e.vaccineinfo_id == id) > 0;
        }
    }
}