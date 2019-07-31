using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity {

    // contains paging functionality for certain Views
    public class PaginatedList<T> : List<T> {

        // a property representing a page's numerical position
        public int PageIndex { get; private set; }

        // a property denoting the total number of pages for a given View
        public int TotalPages { get; private set; }

        // standard constructor that accepts arguments
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);  // store the given items into the current PaginatedList object
        }

        // a property that tracks whether or not a previous page exists
        public bool HasPreviousPage {
            get {
                return (PageIndex > 1);
            }
        }

        // a property that tracks whether or not there is another page beyond the current page
        public bool HasNextPage {
            get {
                return (PageIndex < TotalPages);
            }
        }

        // creates a List containing the user's desired page
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize) {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            // return a new List that only has the user's chosen page
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
