using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Todos.Models;

namespace Todos.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly MyDbContext db;
        private readonly UserManager<MyUser> manager;

        public TodoController()
        {
            db = new MyDbContext();
            manager = new UserManager<MyUser>(new UserStore<MyUser>(db));
        }

        // GET: /Todo/
        public async Task<ActionResult> Index()
        {
            var currentUser = await GetCurrentUser();
            return View(await db.Todos.Where(t => t.User.Id == currentUser.Id).ToListAsync());
        }

        // GET: /Todo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Todo todo = await db.Todos.FindAsync(id);
            if (todo == null)
                return HttpNotFound();
            
            return View(todo);
        }

        // GET: /Todo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Todo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Description,IsDone")] Todo todo)
        {
            var currentUser = await GetCurrentUser();

            if (ModelState.IsValid)
            {
                todo.User = currentUser;
                db.Todos.Add(todo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        // GET: /Todo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Todo todo = await db.Todos.FindAsync(id);
            if (todo == null)
                return HttpNotFound();
            
            return View(todo);
        }

        // POST: /Todo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,IsDone")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        // GET: /Todo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Todo todo = await db.Todos.FindAsync(id);
            if (todo == null)
                return HttpNotFound();
            
            return View(todo);
        }

        // POST: /Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Todo todo = await db.Todos.FindAsync(id);
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> All()
        {
            return View(await db.Todos.ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            
            base.Dispose(disposing);
        }


        private async Task<MyUser> GetCurrentUser()
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            return currentUser;
        }
    }
}
