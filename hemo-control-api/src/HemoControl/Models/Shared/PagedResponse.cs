using System;
using System.Collections.Generic;
using System.Linq;

namespace HemoControl.Models.Shared
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }

        public PagedResponse(IEnumerable<T> items, int limit, int offset, int total)
        {
            Items = items;
            Limit = limit;
            Offset = offset;
            Total = total;
        }

        public PagedResponse<TResult> Map<TResult>(Func<T, TResult> mapFunc)
            => new PagedResponse<TResult>(Items.Select(mapFunc), Limit, Offset, Total);
    }
}