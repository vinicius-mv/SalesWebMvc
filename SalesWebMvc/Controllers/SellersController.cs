using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using System.Collections.Generic;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using System;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellersService _sellerService;
        private readonly DepartmentsService _departmentsService;

        public SellersController(SellersService sellersService, DepartmentsService departmentsService)
        {
            _sellerService = sellersService;
            _departmentsService = departmentsService;
        }

        public IActionResult Index()
        {
            List<Seller> list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            List<Department> departments = _departmentsService.FindAll();
            var viewModel = new SellerFormViewModel() { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = _sellerService.FindById(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = _sellerService.FindById(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            return View(seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = _sellerService.FindById(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            List<Department> department = _departmentsService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Departments = department, Seller = seller };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id) { return RedirectToAction(nameof(Error), new { message = "Id missmatch." }); }

            try
            {
                _sellerService.Update(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel()
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
