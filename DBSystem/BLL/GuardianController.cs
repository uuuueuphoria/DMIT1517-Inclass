using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using DBSystem.DAL;
using DBSystem.ENTITIES;

namespace DBSystem.BLL
{
    public class GuardianController
    {
        public Guardian FindByPKID(int id)
        {
            using (var context = new ContextFSIS())
            {
                return context.Guardians.Find(id);
            }
        }
        public List<Guardian> List()
        {
            using (var context = new ContextFSIS())
            {
                return context.Guardians.ToList();
            }
        }

    }
}
