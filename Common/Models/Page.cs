using System.Collections.Generic;

namespace Models
{
    public class Page<T>
    {
        public List<T> Items { get; set; }

        public int Total { get; set; }
    }
}