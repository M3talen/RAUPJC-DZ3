using System;
using System.Runtime.Serialization;

namespace TodoRepo.Exceptions
{
    [Serializable]
    internal class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string msg) : base("User not owner of item : {" + msg + "}") { }
    }
}