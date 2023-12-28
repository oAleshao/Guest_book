using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guest_book.Models;
using Guest_book.Repository;

namespace Guest_book.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IRepository repo;


        public MessagesController(IRepository repo)
        {
            this.repo = repo;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {

			var model = new ActiveUserModel();
            try
            {
                var user = await repo.GetUserById(Convert.ToInt32(HttpContext.Session.GetString("userId")));
                if (user != null)
                    model.user = user;
            }catch (Exception ex) { }
            model.messages =  await repo.GetListMessages();
            return View(model);
        }


		// GET: Messages/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await repo.GetListMessages() == null)
            {
                return NotFound();
            }
            var message = await repo.GetMessage((int)id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, message, UserId")] Message newMessage)
        {
			newMessage.user = await repo.GetUserById(newMessage.UserId);
			if (ModelState.IsValid)
            {
                newMessage.time = DateTime.Now;
                await repo.Create(newMessage);
                await repo.Save();

                var model = new ActiveUserModel() { user = newMessage.user };
                model.messages = await repo.GetListMessages();
                return View("Index", model);
            }

            return View("~/Views/Account/Home.cshtml", newMessage); //new ViewResult { ViewName = "~/Views/Account/Home.cshtml" };
		}

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await repo.GetListMessages() == null)
            {
                return NotFound();
            }
            var message = await repo.GetMessage((int)id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,message,UserId")] Message EditMessage)
        {
            if (id != EditMessage.Id)
            {
                return NotFound();
            }

			EditMessage.user = await repo.GetUserById(EditMessage.UserId);

			if (ModelState.IsValid)
            {
                try
                {
					EditMessage.time = DateTime.Now;
                    repo.UpdateMessage(EditMessage);
                    await repo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(EditMessage);
        }

        private async Task<bool> MessageExists(int id)
        {
            List<Message> list = await repo.GetListMessages();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await repo.GetListMessages() == null)
            {
                return NotFound();
            }

            var student = await repo.GetMessage((int)id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await repo.GetListMessages() == null)
            {
                return Problem("Entity set 'StudentContext.Students'  is null.");
            }
            var student = await repo.GetMessage(id);
            if (student != null)
            {
                await repo.DeleteMessage(id);
            }

            await repo.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
