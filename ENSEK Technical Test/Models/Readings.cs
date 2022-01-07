using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ENSEK_Technical_Test.Models
{
    public class Readings
    {
        public Readings() { }
        
        public Readings(string[] values) {
            isValid = true;

            //If there are not enough values
            if(values.Length < 3)
            {
                isValid = false;
                raw = values;
                return;
            }

            int accountId;
            DateTime meterReadingDateTime;
            int meterReadingValue;

            //Check to see if the strings parse correctly
            if(!int.TryParse(values[0], out accountId) || !DateTime.TryParse(values[1], out meterReadingDateTime) || !int.TryParse(values[2], out meterReadingValue))
            {
                isValid = false;
            }
            else
            {
                AccountId = accountId;
                MeterReadingDateTime = meterReadingDateTime;
                MeterReadingValue = meterReadingValue;
            }

            raw = values;
        }



        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int UploadId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadingValue { get; set; }

        //We don't want the following to be mapped
        [NotMapped]
        public string[] raw { get; set; }

        [NotMapped]
        private bool _isValid;

        [NotMapped]
        public bool isValid
        {
            get
            {
                //If we have already established that the data is invalid just return _isValid
                if(!_isValid) return _isValid;
                
                //Check length
                if (MeterReadingValue.ToString().Length > 5) return false;
                //Check 
                if (MeterReadingValue < 0) return false;

                isValid = false;

                return true;
            }
            private set
            {
                _isValid = value;
            }
        }

    }
}