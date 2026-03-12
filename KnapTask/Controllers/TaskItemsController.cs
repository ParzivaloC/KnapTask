using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KnapTask.Data;
using KnapTask.Models;
using KnapTask.Services;

namespace KnapTask.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OptimizationService _optimizationService;

        public TaskItemsController(ApplicationDbContext context, OptimizationService optimizationService)
        {
            _context = context;
            _optimizationService = optimizationService;
        }


        // GET: TaskItems
        // Считываем все задачи из базы данных, рассчитываем процент выполнения и статистику по важности, затем передаем эти данные в представление для отображения(Подготовка данных в Контроллере)
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TaskItems.ToListAsync(); // считываем все задачи из базы данных
            
            int totalCount = tasks.Count;
            int completedCount = tasks.Count(t => t.IsCompleted);
            ViewBag.ProgressPercent = totalCount > 0 ? (int)((double)completedCount / totalCount * 100) : 0; // передаем процент выполнения в ViewBag для отображения в представлении

            var stats = tasks.GroupBy(t => t.Value).Select(g => new {Priority = g.Key, Count = g.Count()}).OrderBy(g => g.Priority).ToList(); // группируем задачи по важности и считаем количество задач для каждой важности, затем сортируем по важности

            ViewBag.StatLabels = stats.Select(s => $"Важность {s.Priority}").ToArray();
            ViewBag.StatData = stats.Select(s => s.Count).ToArray();

            return View(tasks);
        }

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Weight,Value,IsCompleted")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Weight,Value,IsCompleted")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
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
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }

        
        public async Task<IActionResult> Plan(int hours = 8)
        {
            var allTasks = await _context.TaskItems.ToListAsync();
            var optimizedTasks = _optimizationService.GetOptimizedPlan(allTasks, hours);
            
            ViewBag.MaxHours = hours;
            return View(optimizedTasks);
        }
    }
}
