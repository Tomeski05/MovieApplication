using Microsoft.AspNetCore.Mvc;
using MovieApp.Domain;
using MovieApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _movieService.GetAllProducts());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _movieService.GetProductById(id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movies product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _movieService.CreateProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await _movieService.GetProductById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movies movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbMovies = await _movieService.GetProductById(id);
                    if (await TryUpdateModelAsync<Movies>(dbMovies))
                    {
                        await _movieService.UpdateProductAsync(dbMovies);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbMovies = await _movieService.GetProductById(id);
                if (dbMovies != null)
                {
                    await _movieService.DeleteProductAsync(dbMovies);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to delete. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
