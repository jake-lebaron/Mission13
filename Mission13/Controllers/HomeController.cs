using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        private BowlingLeagueDbContext _context { get; set; }

        //Constructor
        public HomeController(BowlingLeagueDbContext temp)
        {
            _context = temp;
        }

        public IActionResult Index()
        {
            var blah = _context.Bowlers
                //.Include(x => x.TeamID)
                //.Select(x => x.BowlerID)
                .ToList();

            ViewBag.Teams = _context.Teams.ToList();

            return View(blah);
        }
        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = _context.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                _context.Bowlers.Add(b);
                _context.SaveChanges();
                var x = _context.Bowlers.ToList();
                return View("Index", x);
            }

            else
            {
                ViewBag.Teams = _context.Teams.ToList();

                return View(b);
            }

        }

        [HttpGet]
        public IActionResult EditBowler(int BowlerId)
        {
            ViewBag.Teams = _context.Teams.ToList();

            var input = _context.Bowlers.Single(x => x.BowlerID == BowlerId);

            return View("AddBowler", input);
        }

        [HttpPost]
        public IActionResult EditBowler(Bowler bowler)
        {
            _context.Update(bowler);
            _context.SaveChanges();
            var x = _context.Bowlers.ToList();
            return View("Index", x);
        }

        [HttpGet]
        public IActionResult DeleteBowler(int BowlerId)
        {
            Bowler input = _context.Bowlers.Single(x => x.BowlerID == BowlerId);
            _context.Bowlers.Remove(input);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FilterBowler(int TeamId, int BowlerId)
        {
            ViewBag.Teams = _context.Teams.ToList();
            var x = _context.Bowlers.Select(x => x.TeamID == TeamId || x.BowlerID == BowlerId);
            return View("Index", x);
        }
    }
}
