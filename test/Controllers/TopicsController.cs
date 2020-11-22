using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Domain.Core;
using test.Infrastructure.Data;
using test.Services.BusinessLogic;

namespace test.Controllers
{
    public class TopicsController : Controller
    {
        private readonly Repository<Topic, int> _topicRepository;

        public TopicsController(Repository<Topic, int> topicRepository)
        {
            _topicRepository = topicRepository;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            return View(await _topicRepository.getSet().ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var topic = await _topicRepository.getSet().FirstOrDefaultAsync(m => m.TopicId == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,TopicName")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                await _topicRepository.Create(topic);
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _topicRepository.getSet().FirstOrDefaultAsync(m => m.TopicId == id);

            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,TopicName")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _topicRepository.Update(topic);
                //    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var topic = await _topicRepository.getSet().FirstOrDefaultAsync(m => m.TopicId == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _topicRepository.getSet().FirstOrDefaultAsync(m => m.TopicId == id);
            await _topicRepository.Remove(topic);
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _topicRepository.getSet().Any(e => e.TopicId == id);
        }
    }
}
