using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public record GetDataRequest
    {
        public Guid Id { get; set;}
    }


    public record GetDataResponse 
    {
        public DateTime Date {get; set;}
        public Guid Id {get; set;}
    }
}