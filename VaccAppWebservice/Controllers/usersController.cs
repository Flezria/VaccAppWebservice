using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VaccAppWebservice;

namespace VaccAppWebservice.Controllers
{
    public class usersController : ApiController
    {
        private VaccineContext db = new VaccineContext();

        //Check email if exists
        [Route("EmailCheck/{email}")]
        [HttpGet]
        public async Task<bool> CheckEmail(String email)
        {
            bool result = await db.users.AnyAsync(x => x.email == email);

            return result;

        }

        //Check login
        [Route("Login/{email}/{password}")]
        [HttpGet]
        public async Task<String> Login(String email, String password)
        {
            bool result = await db.users.AnyAsync(x => x.email == email && x.password == password);

            if (result == true)
            {
                users CheckLogin = await db.users.FirstAsync(x => x.email == email);

                if (CheckLogin.api_key == null)
                {

                    var key = new byte[32];
                    using (var generator = RandomNumberGenerator.Create())
                        generator.GetBytes(key);
                    String apikey = Convert.ToBase64String(key);
                    apikey = apikey.Replace('+', 'q').Replace('/', 'w');

                    CheckLogin.api_key = apikey;
                    db.Entry(CheckLogin).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return CheckLogin.api_key;
                }
                else
                {
                    return CheckLogin.api_key;
                }
            }
            else
            {
                return "false";
            }

        }

            // GET: api/users
        public IQueryable<users> Getusers()
        {
            return db.users;
        }

        // GET: api/users/5
        [ResponseType(typeof(users))]
        public async Task<IHttpActionResult> Getusers(int id)
        {
            users users = await db.users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusers(int id, users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.user_id)
            {
                return BadRequest();
            }

            db.Entry(users).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usersExists(id))
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

        // POST: api/users
        [ResponseType(typeof(users))]
        public async Task<IHttpActionResult> Postusers(users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.users.Add(users);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = users.user_id }, users);
        }

        // DELETE: api/users/5
        [ResponseType(typeof(users))]
        public async Task<IHttpActionResult> Deleteusers(int id)
        {
            users users = await db.users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            db.users.Remove(users);
            await db.SaveChangesAsync();

            return Ok(users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersExists(int id)
        {
            return db.users.Count(e => e.user_id == id) > 0;
        }
    }
}