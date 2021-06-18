using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gabinet.Extension
{
    public static class PagingExtension
    {
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> data, int currentPage, int itemOnPage) {
            return data.Skip((currentPage - 1) * itemOnPage).Take(itemOnPage).AsQueryable();
        }

    }
}
