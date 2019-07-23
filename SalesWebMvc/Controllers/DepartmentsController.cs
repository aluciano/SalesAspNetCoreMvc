using System;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Extensions;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly WebApiService _webApiService;

        public DepartmentsController(WebApiService webApiService)
        {
            _webApiService = webApiService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _webApiService.FindAllAsync<Department>();

            return View(departments);
        }

        public IActionResult GridView()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            var departments = await _webApiService.FindAllAsync<Department>();

            object result = await Task.Run(() => DataSourceLoader.Load(departments, loadOptions));

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> EditDepartment(int key, string values)
        {
            try
            {
                await _webApiService.UpdateAsync<Department>(key, values);

                return Ok();
            }
            catch (ApplicationException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertDepartment(string values)
        {
            try
            {
                await _webApiService.InsertAsync<Department>(values);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int key)
        {
            try
            {
                await _webApiService.DeleteAsync<Department>(key);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _webApiService.FindByIdAsync<Department>(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                string jsonValues = JsonConvert.SerializeObject(department);
                await _webApiService.InsertAsync<Department>(jsonValues);

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _webApiService.FindByIdAsync<Department>(id.Value);

            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonValues = JsonConvert.SerializeObject(department);
                    await _webApiService.UpdateAsync<Department>(id, jsonValues);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        private async Task<bool> DepartmentExists(int id)
        {
            var department = await _webApiService.FindByIdAsync<Department>(id);
            return department == null ? false : true;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _webApiService.FindByIdAsync<Department>(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _webApiService.DeleteAsync<Department>(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
