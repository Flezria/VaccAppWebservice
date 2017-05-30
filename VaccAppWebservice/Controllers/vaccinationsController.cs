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
    public class vaccinationsController : ApiController
    {
        private VaccineContext db = new VaccineContext();
        public List<vaccinations> VacProgram { get; set; }

        [Route("GetVacProgram/{api_key}/{program_id}/{child_id}")]
        [HttpGet]
        public async Task<List<vaccinations>> GetVacProgram(String api_key, int program_id, int child_id)
        {
            if (await db.users.AnyAsync(x => x.api_key == api_key))
            {
                var GetVacs = from vac in db.vaccinations
                                 where vac.program_id == program_id
                                 select vac;

                var RawVacProgram = new List<vaccinations>(GetVacs);

                //foreach (var Vac in RawVacProgram)
                //{
                //    var VacCheck =
                //        (vaccination_check)db.vaccination_check.Where(x => (x.vaccine_id == Vac.vaccine_id) && (x.child_id == child_id));

                //    if (VacCheck.is_vac_done != false)
                //    {
                //        VacProgram.Add(Vac);
                //    }
                //}

                var GetRelevantVacs = from RelVac in RawVacProgram
                    from dbVacCheck in db.vaccination_check
                    where dbVacCheck.child_id == child_id
                          && RelVac.vaccine_id == dbVacCheck.vaccine_id
                          && dbVacCheck.is_vac_done == false
                    select RelVac;

                VacProgram = new List<vaccinations>(GetRelevantVacs);
            }


                return VacProgram;

        }

        // GET: api/vaccinations
        public IQueryable<vaccinations> Getvaccinations()
        {
            return db.vaccinations;
        }

        // GET: api/vaccinations/5
        [ResponseType(typeof(vaccinations))]
        public async Task<IHttpActionResult> Getvaccinations(int id)
        {
            vaccinations vaccinations = await db.vaccinations.FindAsync(id);
            if (vaccinations == null)
            {
                return NotFound();
            }

            return Ok(vaccinations);
        }

        // PUT: api/vaccinations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putvaccinations(int id, vaccinations vaccinations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vaccinations.vaccine_id)
            {
                return BadRequest();
            }

            db.Entry(vaccinations).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vaccinationsExists(id))
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

        // POST: api/vaccinations
        [ResponseType(typeof(vaccinations))]
        public async Task<IHttpActionResult> Postvaccinations(vaccinations vaccinations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vaccinations.Add(vaccinations);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vaccinations.vaccine_id }, vaccinations);
        }

        // DELETE: api/vaccinations/5
        [ResponseType(typeof(vaccinations))]
        public async Task<IHttpActionResult> Deletevaccinations(int id)
        {
            vaccinations vaccinations = await db.vaccinations.FindAsync(id);
            if (vaccinations == null)
            {
                return NotFound();
            }

            db.vaccinations.Remove(vaccinations);
            await db.SaveChangesAsync();

            return Ok(vaccinations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vaccinationsExists(int id)
        {
            return db.vaccinations.Count(e => e.vaccine_id == id) > 0;
        }
    }
}