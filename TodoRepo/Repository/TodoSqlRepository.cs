using System;
using System.Collections.Generic;
using System.Linq;
using TodoRepo.Exceptions;
using TodoRepo.Interfaces;
using TodoRepo.Models;

namespace TodoRepo.Repository
{
    public class TodoSqlRepository : ITodoRepository
    {
        private TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.FirstOrDefault(s => s.Id.Equals(todoId));
            if (item == null) return null;
            if (item.UserId.Equals(userId)) return item;
            else throw new TodoAccessDeniedException("Permission denied : User does not own this item.");
        }

        public void Add(TodoItem todoItem)
        {
            if (todoItem == null) throw new ArgumentNullException();
            if (_context.TodoItems.Any(t => t.Text.Equals(todoItem.Text))) throw new DuplicateTodoItemException(todoItem.Id);

            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var item = Get(todoId, userId);
            if (item == null)
                return false;
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem == null) throw new ArgumentNullException();

            var item = Get(todoItem.Id, userId);

            if (item != null)
                item.UpdateItem(todoItem);
            else
                Add(todoItem);
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            if (!_context.TodoItems.Any(t => t.Id.Equals(todoId))) return false;
            Get(todoId, userId).MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId.Equals(userId)).OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId.Equals(userId) && (t.IsCompleted == false))
                .OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId.Equals(userId) && t.IsCompleted)
                .OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            if (filterFunction == null) throw new ArgumentNullException();
            return _context.TodoItems.Where(filterFunction)
                .Where(t => t.UserId.Equals(userId))
                .OrderByDescending(t => t.DateCreated).ToList();
        }
    }
}