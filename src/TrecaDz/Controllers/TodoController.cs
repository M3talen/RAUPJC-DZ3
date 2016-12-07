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
  //  [Authorize]
    public class TodoController : Controller
    {

        private ITodoRepository _repository;
        private UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();
          
            return View(_repository.GetActive(Guid.Parse(user.Id)));
        }

        public async void AddNewToDo(object sender, EventArgs e)
        {
            var user =  await _userManager.GetUserAsync(HttpContext.User);
            var item = new TodoItem("TEST 1", Guid.Parse(user.Id));
            _repository.Add(item);
        }
       
    }
}