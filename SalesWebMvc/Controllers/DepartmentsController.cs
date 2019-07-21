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
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.FindAllWebApiAsync();

            return View(departments);
        }

        public IActionResult GridView()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> GetDepartments(DataSourceLoadOptions loadOptions)
        {
            var departments = await _departmentService.FindAllWebApiAsync();

            object result = await Task.Run(() => DataSourceLoader.Load(departments, loadOptions));

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> EditDepartment(int key, string values)
        {
            try
            {
                await _departmentService.UpdateAsync(key, values);

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
                await _departmentService.InsertAsync(values);

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
                await _departmentService.DeleteAsync(key);

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

            var department = await _departmentService.FindByIdAsync(id.Value);

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
                await _departmentService.InsertAsync(jsonValues);

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

            var department = await _departmentService.FindByIdAsync(id.Value);

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
                    await _departmentService.UpdateAsync(id, jsonValues);
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
            var department = await _departmentService.FindByIdAsync(id);
            return department == null ? false : true;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentService.FindByIdAsync(id.Value);

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
            await _departmentService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
