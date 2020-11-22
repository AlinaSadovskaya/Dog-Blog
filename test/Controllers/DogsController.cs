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
using test.Domain.Core;
using test.Infrastructure.Data;
using test.Services.BusinessLogic;


namespace test.Controllers
{
    public class DogsController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ImageService _imageService;
        private readonly Repository<Dog,int> _dogRepository;
        public DogsController( IWebHostEnvironment appEnvironment, ImageService imageService, Repository<Dog, int> dogRepository)
        {
                _appEnvironment = appEnvironment;
                _imageService = imageService;
                _dogRepository = dogRepository;
        }

        // GET: Dogs
        public async Task<IActionResult> Index()
        {
            return View(await _dogRepository.FindAll());
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var dog = await _dogRepository.getSet().FirstOrDefaultAsync(m => m.DogId == id);
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
                if (uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                {
                    dog.DogImage = await _imageService.SaveImageAsync(uploadedFile, 0);
                }
                else
                {
                    ModelState.AddModelError("Image", "Некорректный формат");
                    return View(dog);
                }
                await _dogRepository.Create(dog);
                return RedirectToAction(nameof(Index));
            }
            return View(dog);
        }

        // GET: Dogs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dog = await _dogRepository.getSet().FirstOrDefaultAsync(m => m.DogId == id);

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
        public async Task<IActionResult> Edit(int id, [Bind("DogId,DogName,DogDescription,DogImage")] Dog dog, IFormFile uploadedFile)
        {
            if (id != dog.DogId)
            {
                return NotFound();
            }

            var dog1 = await _dogRepository.getSet().FirstOrDefaultAsync(m => m.DogId == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (dog.DogDescription != dog1.DogDescription && dog1.DogDescription != null)
                        dog1.DogDescription = dog.DogDescription;
                    if (dog.DogName != dog1.DogName && dog1.DogName != null)
                        dog1.DogName = dog.DogName;
                    if (dog.DogImage != dog1.DogImage && dog1.DogImage != null && uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                    {
                        dog1.DogImage = await _imageService.SaveImageAsync(uploadedFile, 0);
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "Некорректный формат");
                        return View(dog);
                    }
                    await _dogRepository.Update(dog1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.DogId))
                    {
                        return RedirectPermanent("~/Error/Index?statusCode=404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectPermanent("~/Dogs");
            }
            return View(dog);
        }

        // GET: Dogs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dog = await _dogRepository.getSet().FirstOrDefaultAsync(m => m.DogId == id);

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
            var dog = await _dogRepository.getSet().FirstOrDefaultAsync(m => m.DogId == id);
            await _dogRepository.Remove(dog);
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(int id)
        {
            return _dogRepository.getSet().Any(e => e.DogId == id);
        }
    }
}
