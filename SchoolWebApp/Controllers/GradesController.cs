using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Services;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Controllers {
    public class GradesController : Controller {
        GradesService _service;
        public GradesController(GradesService service) {
            _service = service;
        }
        public async Task<IActionResult> Index() {
            var allGrades = await _service.GetAllAsync();
            return View(allGrades);
        }
        public async Task<IActionResult> CreateAsync() {
            var gradesDropdownsData = await _service.GetGradesDropdownsValues();
            ViewBag.Students = new SelectList(gradesDropdownsData.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent request from other sites(http req tool) from being processed require Token
        public async Task<IActionResult> Create(GradesVM newGrade) {
            if (ModelState.IsValid) {
                await _service.CreateAsync(newGrade);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // HttpGet
        // retrieves the information of the grade with the specified id from the service layer, and then creates a GradesVM object that contains the information of the grade and the dropdown values for the students and subjects.
        //The GradesVM object is then passed to the Edit.cshtml view for rendering, so that the user can view and edit the grade.
        public async Task<IActionResult> Edit(int id) {
            var gradeToEdit = await _service.GetByIdAsync(id);

            if (gradeToEdit == null) {
                return NotFound();
            }

            GradesVM grade = new GradesVM() {
                Id = gradeToEdit.Id,
                StudentId = gradeToEdit.Student.Id,
                SubjectId = gradeToEdit.Subject.Id,
                What = gradeToEdit.What,
                Mark = gradeToEdit.Mark,
                Date = gradeToEdit.Date
            };

            // get the dropdown values for the students and subject
            var gradesDropdownsData = await _service.GetGradesDropdownsValues();
            ViewBag.Students = new SelectList(gradesDropdownsData.Students, dataValueField: "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");

            return View(grade);
        }

        // HttpPost
        // Is called when the user submits the updated grade information through the Edit.cshtml view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradesVM updatedGrade) {
            if (id != updatedGrade.Id) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                try {
                    await _service.UpdateAsync(id, updatedGrade);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex) {
                    ModelState.AddModelError("DeleteInEditing", ex.Message);
                    var grades = await _service.GetAllAsync();
                    return View(nameof(Index), grades);
                }
                //  DbUpdateConcurrencyException, it means that another user has updated the same Grade in the meantime
                catch (DbUpdateConcurrencyException) {
                    if (await _service.GetByIdAsync(id) == null) {
                        return NotFound();
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "The grade has been modified by someone else. Please try again.");
                        var grades = await _service.GetAllAsync(); // TODO:not Dry
                        return View(nameof(Edit), grades);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id) {
            var gradeToDelete = await _service.GetByIdAsync(id);
            if (gradeToDelete == null) {
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
