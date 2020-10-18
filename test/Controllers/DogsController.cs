using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers
{
    public class DogsController : Controller
    {
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public DogsController(BlogContext context, IWebHostEnvironment appEnvironment)
        {
                _context = context;
                _appEnvironment = appEnvironment;
        }

        // GET: Dogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dogs.ToListAsync());
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .FirstOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Dogs/Create
        public IActionResult Create()
        {
            return View();
        }

        
        // POST: Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogID,DogName,DogDescription")] Dog dog, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/Files/dogs/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    dog.DogImage = path;
                    _context.SaveChanges();
                }
                _context.Add(dog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dog);
        }

        // GET: Dogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DogId,DogName,DogDescription,DogImage")] Dog dog)
        {
            if (id != dog.DogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.DogId))
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
            return View(dog);
        }

        // GET: Dogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .FirstOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);
            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.DogId == id);
        }
    }
}
