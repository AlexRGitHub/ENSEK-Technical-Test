using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ENSEK_Technical_Test.Data;
using ENSEK_Technical_Test.Models;

namespace ENSEK_Technical_Test.Controllers
{
    public class UserAccountsController : ApiController
    {
        private ENSEK_Technical_TestContext db = new ENSEK_Technical_TestContext();

       
        // POST: api/UserAccounts
        [ResponseType(typeof(UserAccount))]
        public async Task<IHttpActionResult> PostUserAccount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            char[] sep = { ',' };
            string[] line;

            List<UserAccount> userAccount = new List<UserAccount>();

            Stream requestStream = await Request.Content.ReadAsStreamAsync();
            StreamReader sr = new StreamReader(requestStream);

            string data = sr.ReadLine();

            while ((data = sr.ReadLine()) != null)
            {
                line = data.Split(sep, StringSplitOptions.None);

                userAccount.Add(new UserAccount() { AccountId = int.Parse(line[0]), FirstName = line[1], LastName = line[2] });

            }

            db.UserAccounts.AddRange(userAccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = 0 }, userAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAccountExists(int id)
        {
            return db.UserAccounts.Count(e => e.AccountId == id) > 0;
        }
    }
}