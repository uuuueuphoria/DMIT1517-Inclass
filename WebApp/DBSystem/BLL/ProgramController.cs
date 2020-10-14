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
    public class ProgramController
    {
        public Programs FindByPKID(int id)
        {
            using (var context = new ContextStarTED())
            {
                return context.Program.Find(id);
            }
        }
        public List<Programs> List()
        {
            using (var context = new ContextStarTED())
            {
                return context.Program.ToList();
            }
        }
        public List<Programs> FindByID(string id)
        {
            using (var context = new ContextStarTED())
            {
                IEnumerable<Programs> results =
                    context.Database.SqlQuery<Programs>("Programs_FindBySchool @ID"
                        , new SqlParameter("ID", id));
                return results.ToList();
            }
        }
        public int Update(Programs item)
        {
            using (var context = new ContextStarTED())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }
        public int Delete(int programid)
        {
            using (var context = new ContextStarTED())
            {
                var existing = context.Program.Find(programid);
                if (existing == null)
                {
                    throw new Exception("Record has been removed from database");
                }
                context.Program.Remove(existing);
                return context.SaveChanges();
            }
        }
        public int Add(Programs item)
        {
            using (var context = new ContextStarTED())
            {
                context.Program.Add(item);
                context.SaveChanges();
                return item.ProgramID;
            }
        }
    }
}
