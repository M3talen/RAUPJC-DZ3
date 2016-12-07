using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoRepo.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Nullable date time.
        /// DateTime is value type and won’t allow nulls.
        /// DateTime? is nullable DateTime and will accept nulls.
        /// Use null when todo completed date does not exist (e.g. todo is still not completed) 
        /// </summary> 
        public DateTime? DateCompleted { get; set; }

        public DateTime DateCreated { get; set; }

        /// <summary>
        /// User id that owns this TodoItem
        /// </summary> 
        public Guid UserId { get; set; }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid(); // Generates new unique identifier
            Text = text;
            IsCompleted = false;
            UserId = userId;
            DateCreated = DateTime.Now; // Set creation date as current time UserId = userId;
        }

        public TodoItem()
        {
            // entity framework needs this one
            // not for use :)
        }

        public void UpdateItem(TodoItem todoItem)
        {
            Text = todoItem.Text;
            IsCompleted = todoItem.IsCompleted;
            DateCreated = todoItem.DateCreated;
            DateCompleted = todoItem.DateCompleted;
        }

        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                DateCompleted = DateTime.Now;
            }
        }

        public void MarkAsIncomplete()
        {
            if (IsCompleted)
            {
                IsCompleted = false;
                DateCreated = DateTime.Now;
                DateCompleted = null;
            }
        }
    }
}