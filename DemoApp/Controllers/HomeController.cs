using DemoApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace DemoApp.Controllers
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
    }
    public class HomeController : Controller
    {
        IUser_Repository _IUser_Repository = new User_Repository(new MPEntities());
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetOrderNo()
        {
            return Json(_IUser_Repository.getOrderNo(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllOrders()
        {
            var result = (from el in _IUser_Repository.getAll()
                          select new
                          {
                              Order_ID = el.Order_ID,
                              Order_No = el.Order_No,
                              GstAmount = el.GstAmount,
                              PayableAmount = el.PayableAmount,
                              TotalAmount = el.TotalAmount
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOrders(TBL_Order_MST mstObj, List<TBL_Order_Detail> detailObj)
        {
            //mstObj.Order_No = "ORD103";
            //mstObj.TotalAmount = 100000;
            //mstObj.GstAmount = 10;
            //mstObj.Discount = 5;
            //mstObj.PayableAmount = 94500;

            //detailObj = new List<TBL_Order_Detail>();
            //detailObj.Add(new TBL_Order_Detail { Product_Id = 3, Qty = 3, Price = 8000 });
            //detailObj.Add(new TBL_Order_Detail { Product_Id = 4, Qty = 4, Price = 12000 });
            //detailObj.Add(new TBL_Order_Detail { Product_Id = 5, Qty = 7, Price = 4000 });

            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    _IUser_Repository.Upsert_TBL_Order_MST(mstObj);
                    _IUser_Repository.CommitTransaction();
                    foreach (var item in detailObj)
                    {
                        item.Order_ID = mstObj.Order_ID;
                        _IUser_Repository.Upsert_TBL_Order_Detail(item);
                        _IUser_Repository.CommitTransaction();
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json( new { Status = true ,Message = "Inserted succefully..."}, JsonRequestBehavior.AllowGet);
        }


    }
}