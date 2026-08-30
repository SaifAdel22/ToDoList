using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ToDoList.DataAccess;
using ToDoList.Helper;
using ToDoList.Helper.ToDoList.Services;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.ViewModels;


namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUpload _fileUpload;
        private FileType GetFileType(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (imageExtensions.Contains(extension))
            {
                return FileType.Img;
            }

            return FileType.Pdf;
        }

        public HomeController(ApplicationDbContext context, IFileUpload fileUpload)
        {
            _context = context;
            _fileUpload = fileUpload;
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();

            ViewBag.UsersList = new SelectList(users, "Id", "Name");

            return View();
        }

        public IActionResult UserTasks(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = _context.Users
                .Include(u => u.ToDoTasks)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Create(int userId)
        {
            if (userId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CreateTaskVM
            {
                UserId = userId
            };

            return View(viewModel);
        }

       

        [HttpPost]
        public IActionResult Create(CreateTaskVM model)

        {
            if (model.Deadline.Date <= DateTime.Today)
            {
                ModelState.AddModelError("Deadline", "The deadline cannot be in the past.");
            }

            if (ModelState.IsValid)
            {
                string? filePath = null;
              

                if (model.TaskFile != null)
                {
                    filePath = _fileUpload.SaveFile(model.TaskFile, GetFileType(model.TaskFile));
                }

                    var task = new ToDoTask
                {
                    Title = model.Title,
                    Description = model.Description,
                    Deadline = model.Deadline,
                    FilePath = filePath,
                    IsCompleted = false,
                    UserId = model.UserId
                };

                _context.ToDoTasks.Add(task);
                _context.SaveChanges();

                return RedirectToAction(nameof(UserTasks), new { id = model.UserId });
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var task = _context.ToDoTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = !task.IsCompleted;

            _context.ToDoTasks.Update(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(UserTasks), new { id = task.UserId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var task = _context.ToDoTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var viewModel = new EditVM
            {
                Id = task.Id, 
                UserId = task.UserId,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                IsCompleted = task.IsCompleted,
                FilePath = task.FilePath
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (model.Deadline.Date <= DateTime.Today)
            {
                ModelState.AddModelError("Deadline", "The deadline cannot be in the past.");
            }

            if (ModelState.IsValid)
            {
                var existingTask = _context.ToDoTasks.FirstOrDefault(t => t.Id == model.Id);
               
                if (existingTask == null)
                {
                    return NotFound();
                }

                string? filePath = existingTask.FilePath; 

                if (model.TaskFile != null)
                {
                    filePath = _fileUpload.UpdateFile(model.TaskFile, existingTask.FilePath, GetFileType(model.TaskFile));
                }

                existingTask.Title = model.Title;
                existingTask.Description = model.Description;
                existingTask.Deadline = model.Deadline;
                existingTask.IsCompleted = model.IsCompleted;
                existingTask.FilePath = filePath;

                _context.ToDoTasks.Update(existingTask);
                _context.SaveChanges();

                return RedirectToAction(nameof(UserTasks), new { id = model.UserId });
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ToDoTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            int userId = task.UserId; 

            if (!string.IsNullOrEmpty(task.FilePath))
            {
                _fileUpload.DeleteFileLocally(task.FilePath);
            }

            _context.ToDoTasks.Remove(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(UserTasks), new { id = userId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
