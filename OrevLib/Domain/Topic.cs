using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orev.Domain
{
    public class Topic : IOrevEntity
    {
        public virtual int Id { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }
        public virtual String Narrator { get; set; }
        public virtual String Language { get; set; } // a language identifier string, en-US for example
        public virtual String SubmitterEmail { get; set; }
    }
}
