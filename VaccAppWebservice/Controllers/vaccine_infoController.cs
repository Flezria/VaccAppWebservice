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

    }
}