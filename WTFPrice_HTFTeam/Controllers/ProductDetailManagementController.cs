using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductDetailManagementController : Controller
    {
        private WTFPriceEntities db = new WTFPriceEntities();

        // GET: ProductDetailManagement
        public async Task<ActionResult> Index()
        {
            var productDetails = db.ProductDetails.Include(p => p.Product);
            return View(await productDetails.ToListAsync());
        }

        // GET: ProductDetailManagement/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = await db.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // GET: ProductDetailManagement/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ProductDetailManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductId,DisplayOrder,PropertyName,PropertyValue")] ProductDetail productDetail, bool update, string details)
        {
            if (ModelState.IsValid)
            {
                if (details != null)
                {
                    var list = getProductDetails(details, productDetail.ProductId);
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            db.ProductDetails.Add(item);
                        }
                        await db.SaveChangesAsync();

                        return PartialView("_ProductDetailsRecordPartial", list);
                    }
                    return Content("");
                }
                db.ProductDetails.Add(productDetail);
                await db.SaveChangesAsync();

                if (update)
                {
                    return PartialView("_ProductDetailRecordPartial", productDetail);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productDetail.ProductId);
            return View(productDetail);
        }

        // GET: ProductDetailManagement/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = await db.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productDetail.ProductId);
            return View(productDetail);
        }

        // POST: ProductDetailManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductId,DisplayOrder,PropertyName,PropertyValue")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productDetail.ProductId);
            return View(productDetail);
        }

        // GET: ProductDetailManagement/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = await db.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: ProductDetailManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductDetail productDetail = await db.ProductDetails.FindAsync(id);
            db.ProductDetails.Remove(productDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<ProductDetail> getProductDetails(string details, int productId)
        {
            if (details.Equals(""))
            {
                return null;
            }
            try
            {
                var list = new List<ProductDetail>();
                string[] arr = details.Split('\n');
                int i = 10;
                foreach (var item in arr)
                {
                    var detail = new ProductDetail();
                    detail.ProductId = productId;
                    detail.PropertyName = item.Split('_')[0];
                    detail.PropertyValue = item.Split('_')[1];
                    detail.DisplayOrder = i;
                    list.Add(detail);
                    i++;
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
