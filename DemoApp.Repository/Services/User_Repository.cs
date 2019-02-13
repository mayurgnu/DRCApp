using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Repository
{
    public class User_Repository : IUser_Repository
    {
        private static MPEntities context;
        public User_Repository(MPEntities _context) {
            context = _context;
        }
        public string getOrderNo()
        {
            var Id= context.TBL_Order_MST.ToList().Max(x=>x.Order_ID);
            if (Id == 0)
            {
                return "ORD_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + 1;
            }
            else
            {
                return "ORD_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" +Id;
            }
        }
        public List<TBL_Order_MST> getAll()
        {
            return context.TBL_Order_MST.ToList();
        }
        public void Upsert_TBL_Order_MST(TBL_Order_MST obj)
        {
            if (obj != null && obj.Order_ID == 0)
            {
                context.TBL_Order_MST.Add(obj);
            }
            else
            {
                context.Entry<TBL_Order_MST>(obj).State = EntityState.Modified;
            }
        }
        public void Upsert_TBL_Order_Detail(TBL_Order_Detail obj)
        {
            if (obj != null && obj.ID == 0)
            {
                context.TBL_Order_Detail.Add(obj);
            }
            else
            {
                context.Entry<TBL_Order_Detail>(obj).State = EntityState.Modified;
            }
        }
        public void CommitTransaction()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
