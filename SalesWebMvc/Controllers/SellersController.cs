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

        public async Task<IActionResult> Index()
        {
            List<Seller> list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _departmentsService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            // check the validate result from js script in the client side (DataAnotations checks)
            if (!ModelState.IsValid)
            {
                List<Department> departments = await _departmentsService.FindAllAsync();
                SellerFormViewModel viewModel = new SellerFormViewModel() { Departments = departments, Seller = seller };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException iEx)
            {
                return RedirectToAction(nameof(Error), new { message = iEx.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id not provided." }); }

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if (seller == null) { return RedirectToAction(nameof(Error), new { message = "Id not found." }); }

            List<Department> department = await _departmentsService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Departments = department, Seller = seller };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            // check the validate result from js script in the client side (DataAnotations checks)
            if (!ModelState.IsValid)
            {
                List<Department> departments = await _departmentsService.FindAllAsync();
                SellerFormViewModel viewModel = new SellerFormViewModel() { Departments = departments, Seller = seller };
                return View(viewModel);
            }

            if (id != seller.Id) { return RedirectToAction(nameof(Error), new { message = "Id missmatch." }); }

            try
            {
                await _sellerService.UpdateAsync(seller);

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
