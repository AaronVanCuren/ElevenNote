using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Note
        public ActionResult Index()
        {
            var model = new NoteListItem[0];
            return View(model);
        }

        //GET
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate note)
        {
            if (ModelState.IsValid)
            {
                _db.Notes.Add(note);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(note);
        }
    }
}