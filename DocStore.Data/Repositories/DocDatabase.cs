using DocStore.Data.Interfaces;
using DocStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocStore.Data.Repositories
{
    public class DocDatabase : IDocRepository
    {
        private DocStoreContext db;

        public DocDatabase(DocStoreContext _db)
        {
            this.db = _db;
        }

        public bool AddNewDoc(Doc doc)
        {
            db.Docs.Add(doc);
            db.SaveChanges();
            return true;
        }

        public List<Doc> GetAllDocs()
        {

           return db.Docs.ToList();
        }

        public List<Doc> GetAllSortedDocs()
        {
            return db.Docs.OrderBy(d=> d.Name).ToList();
        }

        public Doc GetDoc(string location)
        {
           
            return db.Docs.FirstOrDefault(x => x.Location == location.Replace(@"\\", @"\")); // replce double back slashes
        }

        public bool RemoveDoc(string location)
        {

            var doc = GetDoc(location);

            if (doc == null)
                return false;


            db.Docs.Remove(doc);
            db.SaveChanges();
            return true;
        }
    }
}
