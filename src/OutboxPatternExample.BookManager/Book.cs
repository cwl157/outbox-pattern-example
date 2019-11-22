using System;
using System.Collections.Generic;
using System.Text;

namespace OutboxPatternExample.BookManager
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
