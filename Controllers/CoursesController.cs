using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zavrsni.Models;

namespace Zavrsni.Controllers
{

    public class CoursesController : Controller
    {
        private readonly ZavrsniRadBizUpContext _context;

        public CoursesController(ZavrsniRadBizUpContext context)
        {
            _context = context;
        }

        // GET: Courses
        [Authorize(Policy = "LoggedIn")]
        public async Task<IActionResult> Index()
        {
            var zavrsniRadBizUpContext = _context.Courses.Include(c => c.Teacher);
            return View(await zavrsniRadBizUpContext.ToListAsync());
        }

        // GET: Courses
        [Authorize(Policy = "LoggedIn")]
        public async Task<IActionResult> TeacherCourses(String email)
        { //da teacher moze da vidi samo svoje kurseve

            var user = HttpContext.User;
            if (user.HasClaim(c => c.Type == "Role" && c.Value == "Teacher"))
            {
                var userLogged = _context.Users.SingleOrDefault(u => u.Email == email);
                int userID = userLogged.UserID;

                var courses = _context.Courses.Where(c => c.TeacherID == userID);

                return View(await courses.ToListAsync());

            }
            else
                return NotFound();

        }

        [Authorize(Policy = "LoggedIn")]

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        [Authorize(Policy = "AdminTeacher")]
        // GET: Courses/Create
        public IActionResult Create()
        {

            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "Introduction");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Name,TeacherID,Description,Duration,Price")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "Introduction");
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "Introduction");
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Name,TeacherID,Description,Duration,Price")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
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
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "Introduction");
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ZavrsniRadBizUpContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "AdminTeacher")]
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
