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
using Microsoft.AspNetCore.Http;

namespace ENSEK_Technical_Test.Controllers
{
    public class ReadingsController : ApiController
    {
        private ENSEK_Technical_TestContext db = new ENSEK_Technical_TestContext();

        // GET: api/Readings
        public IQueryable<Readings> GetReadings()
        {
            return db.Readings;
        }

        // GET: api/Readings/5
        [ResponseType(typeof(Readings))]
        public IHttpActionResult GetReadings(int id)
        {
            Readings readings = db.Readings.Find(id);
            if (readings == null)
            {
                return NotFound();
            }

            return Ok(readings);
        }


        // POST: api/Readings
        [ResponseType(typeof(Tuple<int, int>))]
        public async Task<IHttpActionResult> PostReadings()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Now lets process the CSV file
            char[] sep = { ',' };
            string[] line;
            int index = 0;

            //Create a list for our valid readings
            List<Readings> readings = new List<Readings>();
            
            //read the content as stream
            Stream requestStream = await Request.Content.ReadAsStreamAsync();
            StreamReader sr = new StreamReader(requestStream);

            //Skip headings
            string data = sr.ReadLine();
            if(data == null)
            {
                return BadRequest("Data is null.");
            }
            else if (!data.StartsWith("AccountId"))
            {
                //missing headers, might be bad file format. Reject.
                return BadRequest("Invalid file headers. Please load CSV file in correct format.");
            }

            //Lets create a unique upload ID
            //Create a new instance of the ReadingUploads class
            ReadingUploads Uploads = new ReadingUploads();
            //Sate the upload time
            Uploads.UploadDateTime = DateTime.Now;
            //Add it to the ReadingUploads database
            db.ReadingUploads.Add(Uploads);
            //Save the changes so that the key UploadID gets updated
            db.SaveChanges();

            while ((data = sr.ReadLine()) != null)
            {
                index++;
                //Split the line by ','
                line = data.Split(sep, StringSplitOptions.None);

                //Create a new Readings class and pass the list of strings
                Readings r = new Readings(line);

                //Set the upload id
                r.UploadId = Uploads.UploadId;

                //Within the Readings class we have another validation check
                if(r.isValid)
                {
                    readings.Add(r);
                }
            }

            //Normally I would perform the following in SQL Server with a Merge query with SQL handling the account checks ect.
            //Filter out any older values in the upload.
            var latest = (from r in readings
                         group r by r.AccountId into g
                         select g.OrderByDescending(o => o.MeterReadingDateTime).First()).ToList();

            //Check if the account exists
            var records = latest.Where(s => db.UserAccounts.Any(f => f.AccountId == s.AccountId)).ToList();
           
            //Check if there is an older version or no version in the database
            var finalUpload = (from r in latest
                              join re in db.Readings
                              on r.AccountId equals re.AccountId into j
                              from lj in j.DefaultIfEmpty() 
                              where r.MeterReadingDateTime > (lj?.MeterReadingDateTime ?? DateTime.MinValue)
                              select r).ToList();
            
            db.Readings.AddRange(finalUpload);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Uploads.UploadId }, new Tuple<int, int>(finalUpload.Count,index - finalUpload.Count) );
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReadingsExists(int id)
        {
            return db.Readings.Count(e => e.Id == id) > 0;
        }
    }
}