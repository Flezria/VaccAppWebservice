﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WebGrease.Css.Extensions;

namespace VaccAppWebservice.Controllers
{
    public class user_childsController : ApiController
    {
        private VaccineContext db = new VaccineContext();

        public List<user_childs> ChildList { get; set; }

        // GET: api/user_childs
        public IQueryable<user_childs> Getuser_childs()
        {
            return db.user_childs;
        }

        [Route("GetChilds/{api_key}")]
        [HttpGet]
        public async Task<List<user_childs>> GetChildByApi(String api_key)
        {
            if (await db.users.AnyAsync(x => x.api_key == api_key))
            {
                var ListDB = from child in db.user_childs
                             where child.api_key == api_key
                             select child;

                ChildList = new List<user_childs>(ListDB);

            }

            return ChildList;
        }

        // GET: api/user_childs/5
        [ResponseType(typeof(user_childs))]
        public async Task<IHttpActionResult> Getuser_childs(int id)
        {
            user_childs user_childs = await db.user_childs.FindAsync(id);
            if (user_childs == null)
            {
                return NotFound();
            }

            return Ok(user_childs);
        }

        // PUT: api/user_childs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putuser_childs(int id, user_childs user_childs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_childs.child_id)
            {
                return BadRequest();
            }

            db.Entry(user_childs).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!user_childsExists(id))
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

        [Route("ChildBool/{api_key}")]
        [HttpGet]
        public async Task<bool> DoesChildsExist(String api_key)
        {
            
            return await db.user_childs.AnyAsync(x => x.api_key == api_key);
        }

        [Route("PostChild/{api_key}")]
        [ResponseType(typeof(user_childs))]
        [HttpPost]
        public async Task<IHttpActionResult> PostChild(String api_key, user_childs child)
        {
            if (await db.users.AnyAsync(x => x.api_key == api_key))
            {
                users parent = await db.users.FirstAsync(x => x.api_key == api_key);
                vaccine_program programID = await db.vaccine_program.FirstAsync(x => (x.program_date.Year == child.birthday.Year) || (x.program_date > child.birthday));

                child.user_id = parent.user_id;
                child.program_id = programID.program_id;

                var VacListProgram = db.vaccinations.Where(x => x.program_id == child.program_id);

                foreach (var item in VacListProgram)
                {
                    db.vaccination_check.Add(new vaccination_check()
                    {
                        vac_check_id = 0,
                        is_vac_done = false,
                        is_notification_sent = false,
                        child_id = child.child_id,
                        vaccine_id = item.vaccine_id,
                    });
                }

                if(!ModelState.IsValid)
                {
                    return BadRequest();
                }

                db.user_childs.Add(child);
                await db.SaveChangesAsync();

                return Ok();

            }

            return BadRequest();
        }

        // POST: api/user_childs
        [ResponseType(typeof(user_childs))]
        public async Task<IHttpActionResult> Postuser_childs(user_childs user_childs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.user_childs.Add(user_childs);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user_childs.child_id }, user_childs);
        }

        // DELETE: api/user_childs/5
        [ResponseType(typeof(user_childs))]
        public async Task<IHttpActionResult> Deleteuser_childs(int id)
        {
            user_childs user_childs = await db.user_childs.FindAsync(id);
            if (user_childs == null)
            {
                return NotFound();
            }

            db.user_childs.Remove(user_childs);
            await db.SaveChangesAsync();

            return Ok(user_childs);
        }

        [Route("DeleteChild/{id}/{api_key}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int id, String api_key)
        {
            if (await db.users.AnyAsync(x => x.api_key == api_key))
            {
                user_childs user_childs = await db.user_childs.FindAsync(id);

                if (user_childs == null)
                {
                    return NotFound();
                }

                db.user_childs.Remove(user_childs);
                await db.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool user_childsExists(int id)
        {
            return db.user_childs.Count(e => e.child_id == id) > 0;
        }
    }
}