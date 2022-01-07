using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ENSEK_Technical_Test.Data
{
    public class ENSEK_Technical_TestContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ENSEK_Technical_TestContext() : base("name=ENSEK_Technical_TestContext")
        {
        }

        public System.Data.Entity.DbSet<ENSEK_Technical_Test.Models.UserAccount> UserAccounts { get; set; }

        public DbSet<ENSEK_Technical_Test.Models.ReadingUploads> ReadingUploads { get; set; }

        public System.Data.Entity.DbSet<ENSEK_Technical_Test.Models.Readings> Readings { get; set; }
    }
}
