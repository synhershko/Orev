using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orev.Domain
{
    public class Corpus : IOrevEntity
    {
        public virtual int Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Language { get; set; } // a language identifier string, en-US for example
        public virtual String Path { get; set; }
    }
}
