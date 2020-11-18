using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HemoControl.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace HemoControl.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(this IQueryable<T> query, PagedRequest request, CancellationToken cancellationToken = default)
        {
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            return new PagedResponse<T>(items, request.Limit, request.Offset, total);
        }
    }
}