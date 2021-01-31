using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TodoListAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace TodoListAPI.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemContext _context;

        public TodoItemsController(TodoItemContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet("{sessionUserId}/{status}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(long sessionUserId, int status)
        {
            return await _context.TodoItems.Where(e => e.todoUserId == sessionUserId && e.todoStatus == status).ToListAsync();
        }

        // GET: api/TodoItems
        [HttpGet("{sessionUserId}/{pageno}/{limit}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(long sessionUserId, int pageno, int limit)
        {
            int offset = (pageno-1) * limit;
            var todoitems = await _context.TodoItems.Where(e => e.todoUserId == sessionUserId && e.todoStatus == 0).Skip(offset).Take(limit).ToListAsync();
            return todoitems;
        }

        // GET: api/TodoItems/GetSortTodoTitle
        [HttpGet("GetSortTodoTitle/{sessionUserId}/{pageno}/{limit}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetSortTodoTitle(long sessionUserId, int pageno, int limit)
        {
            int offset = (pageno - 1) * limit;
            return await _context.TodoItems.OrderBy(e => e.todoTitle).Where(e => e.todoUserId == sessionUserId && e.todoStatus == 0).Skip(offset).Take(limit).ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.todoId)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            var todoitem = await _context.TodoItems.Where(e => e.todoDate == todoItem.todoDate).ToListAsync();
            todoItem.todoCreateDate = System.DateTime.UtcNow;
            if (todoitem.Any())
            {
                return null;
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.todoId }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        // POST: api/TodoItems/DeleteMultiTodoItem
        [HttpPost("DeleteMultiTodoItem")]
        public async Task<ActionResult<string>> DeleteMultiTodoItem(long[] id)
        {
            Object[] obj = new object[id.Length];
            int i = 0;
            foreach(long user_id in id)
            {
                if (TodoItemExists(user_id))
                {
                    obj[i] = await DeleteTodoItem(user_id);
                    i++;
                }
            }
            string message = "Items Deleted.";
            return message;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Where(e => e.todoStatus == 0).Any(e => e.todoId == id);
        }


        // GET: api/TodoItems/GetTodoItemViaTitle/todotitle/todopriority/tododate
        [HttpGet("GetTodoItemViaSearch/{sessionUserId}/{todotitle}/{todopriority}/{tododate}/{pageno}/{limit}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemViaSearch(long sessionUserId, string todotitle, int? todopriority, DateTime? tododate, int pageno, int limit)
        {
            int offset = (pageno - 1) * limit;
            var todoItem = await _context.TodoItems.Where(e => e.todoUserId == sessionUserId && e.todoStatus == 0).Skip(offset).Take(limit).ToListAsync();
            if (todopriority != 0)
            {
                todoItem = todoItem.Where(e => e.todoPriority == todopriority).ToList();
            }
            if(tododate != null)
            {
                todoItem = todoItem.Where(e => e.todoDate.Date == tododate).ToList();
            }
            if (todotitle != null)
            {
                todoItem = todoItem.Where(e => e.todoTitle.ToLower().StartsWith(todotitle)).ToList();
            }
            return todoItem;
        }

        // GET: api/TodoItems/GetTodoItemViaTitle/startDate/endDate
        [HttpGet("GetTodoItemViaAdvSearch/{sessionUserId}/{startDate}/{endDate}/{pageno}/{limit}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemViaAdvSearch(long sessionUserId, DateTime startDate, DateTime endDate, int pageno, int limit)
        {
            int offset = (pageno - 1) * limit;
            var todoItem = await _context.TodoItems.Where(e => e.todoUserId == sessionUserId && e.todoDate >= startDate && e.todoDate <= endDate && e.todoStatus == 0).Skip(offset).Take(limit).ToListAsync();
            return todoItem;
        }

        // GET: api/TodoItems/GetUserTodoItemList/{userid}
        [HttpGet("GetUserTodoItemList/{sessionUserId}")]
        public async Task<ActionResult<List<TodoItem>>> GetUserTodoItemList(long sessionUserId)
        {
            var todoItem = await _context.TodoItems.Where(e => e.todoUserId == sessionUserId && e.todoStatus == 0).ToListAsync();
            return todoItem;
        }


    }
}
