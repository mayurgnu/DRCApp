using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Repository
{
    public interface IUser_Repository : IDisposable
    {
        List<TBL_Order_MST> getAll();
        string getOrderNo();
        void Upsert_TBL_Order_MST(TBL_Order_MST obj);
        void Upsert_TBL_Order_Detail(TBL_Order_Detail obj);
        void CommitTransaction();
    }
}
