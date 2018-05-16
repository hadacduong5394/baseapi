using System;
using System.Collections.Generic;

namespace hdcore
{
    public class PaginationSet<T>
    {
        public int Page { set; get; }
        public int PageSize { get; set; }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.TotalCount / this.PageSize);
            }
        }

        public int TotalCount { set; get; }
        public IList<T> Items { set; get; }
    }
}