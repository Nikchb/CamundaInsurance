using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services
{
    public abstract class ServiceBase
    {
        protected SContentResponce<T> Ok<T>()
        {
            return new SContentResponce<T>(true);
        }

        protected SContentResponce<T> Ok<T>(T content)
        {
            return new SContentResponce<T>(content);
        }

        protected SContentResponce<T> Error<T>()
        {
            return new SContentResponce<T>(false);
        }

        protected SContentResponce<T> Error<T>(params string[] messages)
        {
            return new SContentResponce<T>(messages);
        }

        protected SResponce Ok()
        {
            return new SResponce(true);
        }

        protected SResponce Error()
        {
            return new SResponce(false);
        }

        protected SResponce Error(params string[] messages)
        {
            return new SResponce(messages);
        }
    }

    public class SContentResponce<T> : SResponce
    {       
        public T Content { get; set; }

        public SContentResponce(bool succeeded) : base(succeeded)
        {
        }

        public SContentResponce(T content)
        {
            Succeeded = true;
            Content = content;
        }

        public SContentResponce(params string[] messages) : base(messages)
        {
        }
    }

    public class SResponce
    {
        public bool Succeeded { get; set; }

        public string[] Messages { get; set; } = Array.Empty<string>();

        public SResponce(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public SResponce(params string[] messages)
        {
            Succeeded = false;
            Messages = messages;
        }
    }

    
}
