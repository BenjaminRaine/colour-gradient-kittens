using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGKUpdated.Data;
using CGKUpdated.Models;
using System.Drawing;
using CGKProject.Models;
using System.Drawing.Imaging;

namespace CGKUpdated.Controllers
{
    public class ImagePairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagePairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImagePairs
        public async Task<IActionResult> Index()
        {
              return _context.ImagePair != null ? 
                          View(await _context.ImagePair.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ImagePair'  is null.");
        }

        // GET: ImagePairs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ImagePair == null)
            {
                return NotFound();
            }

            var imagePair = await _context.ImagePair
                .FirstOrDefaultAsync(m => m.title == id);
            if (imagePair == null)
            {
                return NotFound();
            }

            return View(imagePair);
        }

        // GET: ImagePairs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImagePairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImagePairViewModel imagePairView)
        {
            ImagePair pair = new ImagePair();
            if (ModelState.IsValid)
            {
                // TODO: manage path and server side location once hosted
                string origPath = String.Format("wwwroot/pairs/original/{0}.jpg", imagePairView.title);
                string gradPath = String.Format("wwwroot/pairs/gradient/{0}.jpg", imagePairView.title);

                FileStream origStream = new FileStream(origPath, FileMode.Create);

                await imagePairView.original.CopyToAsync(origStream);
                origStream.Close();

                origStream = new FileStream(origPath, FileMode.Open, FileAccess.ReadWrite);

                Bitmap gradMap = new Bitmap(origStream);
                Gradient.GenGradient(gradMap);
                gradMap.Save(gradPath, ImageFormat.Jpeg);
                
                origStream.Close();

                pair.title = imagePairView.title;
                pair.user = imagePairView.user;
                pair.addedAt = DateTime.Now;

                _context.Add(pair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pair);
        }

        // GET: ImagePairs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ImagePair == null)
            {
                return NotFound();
            }

            var imagePair = await _context.ImagePair.FindAsync(id);
            if (imagePair == null)
            {
                return NotFound();
            }
            return View(imagePair);
        }

        // POST: ImagePairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("title,user,addedAt")] ImagePair imagePair)
        {
            if (id != imagePair.title)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagePair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagePairExists(imagePair.title))
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
            return View(imagePair);
        }

        // GET: ImagePairs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ImagePair == null)
            {
                return NotFound();
            }

            var imagePair = await _context.ImagePair
                .FirstOrDefaultAsync(m => m.title == id);
            if (imagePair == null)
            {
                return NotFound();
            }

            return View(imagePair);
        }

        // POST: ImagePairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ImagePair == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ImagePair'  is null.");
            }
            var imagePair = await _context.ImagePair.FindAsync(id);
            if (imagePair != null)
            {
                _context.ImagePair.Remove(imagePair);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagePairExists(string id)
        {
          return (_context.ImagePair?.Any(e => e.title == id)).GetValueOrDefault();
        }
    }
}
