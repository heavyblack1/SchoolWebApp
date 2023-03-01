using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Models;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers {
    public class SubjectsController : Controller {
        public SubjectService _service;
        public SubjectsController(SubjectService service) {
            this._service = service;
        }
        public async Task<IActionResult> Index() {
            var allSubjects = await _service.GetAllAsync();
            return View(allSubjects);
        }

        // public async Task<IActionResult> Details(int id) {
        //     var subject = await _service.GetByIdAsync(id);
        //     if (subject != null) {
        //         return View("Details", subject);
        //     }
        //     return View();
        // }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Subject newSubject) {
            if (ModelState.IsValid) {
                await _service.CreateAsync(newSubject);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id) {
            var subjectToEdit = await _service.GetByIdAsync(id);
            if (subjectToEdit == null) {
                return NotFound();
            }
            return View(subjectToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Subject updatedSubject) {
            if (id != updatedSubject.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    await _service.UpdateAsync(id, updatedSubject);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex) {
                    ModelState.AddModelError("DeleteInEditing", ex.Message);
                    var subject = await _service.GetAllAsync();
                    return View(nameof(Index), subject);

                }
                //  DbUpdateConcurrencyException, it means that another user has updated the same subject in the meantime
                catch (DbUpdateConcurrencyException) {
                    if (await _service.GetByIdAsync(id) == null) {
                        return NotFound();
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "The subject has been modified by someone else. Please try again.");
                        var subject = await _service.GetAllAsync(); // TODO:not Dry
                        return View(nameof(Edit), subject);
                    }
                }
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id) {
            var subjectToDelete = await _service.GetByIdAsync(id);
            if (subjectToDelete == null) {
                return NotFound();
            }
            try {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                // Log the error or handle it appropriately
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", ViewBag.ErrorMessage);
            }
        }
    }
}
