using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocStore.Data.Models
{
   public class DocStoreContext : DbContext
    {
        public DocStoreContext(DbContextOptions<DocStoreContext> options)
            : base(options)
        {

        }

        public DbSet<Doc> Docs { get; set; }
    }
}
