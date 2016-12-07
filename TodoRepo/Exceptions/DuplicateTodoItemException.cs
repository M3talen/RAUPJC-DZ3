using System;
using System.Runtime.Serialization;

namespace TodoRepo.Exceptions
{
    [Serializable]
    internal class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(Guid id) : base("duplicate id: {" + id.ToString() + "}") { }
    }
}