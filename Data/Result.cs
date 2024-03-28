using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mygallery.Data
{
    public class Result
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Result()
        {
            Status = false;
            Message = string.Empty;
        }
        public Result(bool Status)
        {
            this.Status = Status;
            Message = string.Empty;
        }
        public Result(bool Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }

    }

    public class Result<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }

        public Result()
        {
            Status = false;
            Message = string.Empty;
            Value = default;
        }
        public Result(bool Status)
        {
            this.Status = Status;
            Message = string.Empty;
        }
        public Result(bool Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }
        public Result(bool Status, string Message, T t)
        {
            this.Status = Status;
            this.Message = Message;
            Value = t;
        }
    }
}