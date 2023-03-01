using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Models;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers {
    public class StudentsController : Controller {
        public StudentService _service;
        public StudentsController(StudentService service) {
            this._service = service;
        }
        public async Task<IActionResult> Index() {
            var allStudents = await _service.GetAllAsync();
            return View(allStudents);
        }

        public async Task<IActionResult> Details(int id) {
            var student = await _service.GetByIdAsync(id);
            return View("Details", student);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent request from other sites(http req tool) from being processed require Token
        public async Task<IActionResult> Create(Student newStudent) {
            if (ModelState.IsValid) {
                await _service.CreateAsync(newStudent);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id) {

            var studentToEdit = await _service.GetByIdAsync(id);
            if (studentToEdit == null) {
                return NotFound();
            }
            return View(studentToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent request from other sites(http req tool) from being processed require Token
        // [Bind("Id,FirstName,LastName,DateOfBirth")] is used to whitelist the properties that can be bound to the Student
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, DateOfBirth")] Student updatedStudent) {
            if (id != updatedStudent.Id) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                try {
                    await _service.UpdateAsync(id, updatedStudent);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex) {
                    ModelState.AddModelError("DeleteInEditing", ex.Message);
                    var students = await _service.GetAllAsync();
                    return View(nameof(Index), students);
                }
                //  DbUpdateConcurrencyException, it means that another user has updated the same student in the meantime
                catch (DbUpdateConcurrencyException) {
                    if (await _service.GetByIdAsync(id) == null) {
                        return NotFound();
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "The student has been modified by someone else. Please try again.");
                        var students = await _service.GetAllAsync(); // TODO:not Dry
                        return View(nameof(Edit), students);
                    }
                }
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id) {
            var studentToDelete = await _service.GetByIdAsync(id);
            if (studentToDelete == null) {
                return NotFound();
            }

            try {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                // Log the error or handle it appropriately
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
