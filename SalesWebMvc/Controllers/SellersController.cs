using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly WebApiService _webApiService;

        public SellersController(WebApiService webApiService)
        {
            _webApiService = webApiService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _webApiService.FindAllAsync<Seller>();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _webApiService.FindAllAsync<Department>();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeller(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> departmentList = await _webApiService.FindAllAsync<Department>();
                SellerFormViewModel viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departmentList
                };
                return View(viewModel);
            }

            string jsonValues = JsonConvert.SerializeObject(seller);

            await _webApiService.InsertAsync<Seller>(jsonValues);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _webApiService.FindByIdAsync<Seller>(id.Value);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _webApiService.DeleteAsync<Seller>(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }            
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _webApiService.FindByIdIncludingAsync<Seller>(id.Value, "Department");
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _webApiService.FindByIdIncludingAsync<Seller>(id.Value, "Department");
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            List<Department> departmentList = await _webApiService.FindAllAsync<Department>();
            SellerFormViewModel viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departmentList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> departmentList = await _webApiService.FindAllAsync<Department>();
                SellerFormViewModel viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departmentList
                };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch." });
            }

            try
            {
                string jsonValues = JsonConvert.SerializeObject(seller);

                await _webApiService.UpdateAsync<Seller>(id, jsonValues);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}