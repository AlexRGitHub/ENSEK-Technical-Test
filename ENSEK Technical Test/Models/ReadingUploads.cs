using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENSEK_Technical_Test.Models
{
    public class ReadingUploads
    {
        [Key]
        public int UploadId { get; set; }
        public DateTime UploadDateTime { get; set; }
    }
}