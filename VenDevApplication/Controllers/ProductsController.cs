using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VenDevApplication.Models;
using VenDevApplication.Services.Interfaces;

namespace VenDevApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Iproduct _productService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductsController(Iproduct productService,IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetListProduct());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile img)
        {
            if (img != null)
            {
                try
                {
                    var uniqueImgName = SetImageName(img.FileName);
                    var images = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var imagePath = Path.Combine(images, uniqueImgName);
                    img.CopyTo(new FileStream(imagePath, FileMode.Create));
                    product.Image = uniqueImgName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }
            if (ModelState.IsValid)
            {
              var success =  await _productService.AddProduct(product);
                if (success)
                    return RedirectToAction(nameof(Index));
            }
            ViewBag.Message = "Probléme au niveau de serveur";
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile img)
        {
            if (img != null)
            {
                try
                {
                    var uniqueImgName = SetImageName(img.FileName);
                    var images = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var newImagePath = Path.Combine(images, uniqueImgName);
                    img.CopyTo(new FileStream(newImagePath, FileMode.Create));
                    product.Image = uniqueImgName;
                    //try to delete old image if exist
                    var oldImageName = await _productService.GetProductImageName(product.Id);
                    if (!string.IsNullOrEmpty(oldImageName))
                    {
                        var oldimagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", oldImageName);
                        System.IO.File.Delete(oldimagePath);
                       
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
            if (ModelState.IsValid)
            {
                try
                {
                   var success = await _productService.UpdateProduct(product);
                    if(success)
                        return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException e)
                {
                    var exist = await ProductExists(product.Id);
                    if (!exist)
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                }
               
            }
            ViewBag.Message = "Probléme au niveau de serveur";
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductById(id);
            var ImageName = await _productService.GetProductImageName(id);
            var success = await _productService.DeleteProductById(id);
            if(success)
            {
                //try to delete old image if exist
                if (!string.IsNullOrEmpty(ImageName))
                {
                    try
                    {
                        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", ImageName);
                        System.IO.File.Delete(imagePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
               
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Message = "Probléme au niveau de serveur";
            return View(product);
        }
        public async Task<bool> ProductExists(int id)
        {
            var product = await _productService.GetProductById(id);
            return product == null;
        }
        public string SetImageName(string name)
        {
            name = Path.GetFileName(name);
            return Path.GetFileNameWithoutExtension(name)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(name);
        }
      
    }
}
