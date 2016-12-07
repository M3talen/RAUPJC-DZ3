using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoRepo.Interfaces;
using TodoRepo.Models;
using TrecaDz.Models;

namespace TrecaDz.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {

        private ITodoRepository _repository;
        private UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var todoItems = _repository.GetActive(Guid.Parse(currentUser.Id));

            return View(todoItems);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewToDo(AddTodoViewModel m)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var item = new TodoItem(m.Text, Guid.Parse(currentUser.Id));
                _repository.Add(item);
                return RedirectToAction("Index");
            }
            return View("Add", m);
        }

        public async Task<IActionResult> SeeCompleted()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var todoItems = _repository.GetCompleted(Guid.Parse(currentUser.Id));

            return View(todoItems);
        }

        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(id, Guid.Parse(currentUser.Id));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkAsInCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsIncompleted(id, Guid.Parse(currentUser.Id));

            return RedirectToAction("SeeCompleted");
        }

        public async Task<IActionResult> SeeActive()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var todoItems = _repository.GetActive(Guid.Parse(currentUser.Id));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Delete(id, Guid.Parse(currentUser.Id));

            return RedirectToAction("SeeCompleted");
        }

    }
}