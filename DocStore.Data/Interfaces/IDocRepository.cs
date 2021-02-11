using DocStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocStore.Data.Interfaces
{
    public interface IDocRepository
    {
        List<Doc> GetAllDocs();

        List<Doc> GetAllSortedDocs();
        Doc GetDoc(string location);
     
        bool AddNewDoc(Doc doc);
        bool RemoveDoc(string location);


    }
}
