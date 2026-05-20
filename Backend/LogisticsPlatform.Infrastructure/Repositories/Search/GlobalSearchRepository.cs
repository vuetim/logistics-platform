using LogisticsPlatform.Application.DTOs.Search;
using LogisticsPlatform.Application.Interfaces.Repositories.Search;
using LogisticsPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Infrastructure.Repositories.Search;

public class GlobalSearchRepository : IGlobalSearchRepository
{
    private readonly AppDbContext _db;

    public GlobalSearchRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<GlobalSearchResultDto>> SearchLoadsAsync(string query, int take)
    {
        var lowered = query.ToLower();

        return _db.Loads
            .AsNoTracking()
            .Where(load =>
                !load.IsArchived &&
                (load.LoadNumber.ToLower().Contains(lowered) ||
                 (load.TrackingNumber != null && load.TrackingNumber.ToLower().Contains(lowered)) ||
                 load.Customer.Name.ToLower().Contains(lowered) ||
                 (load.Carrier != null && load.Carrier.Name.ToLower().Contains(lowered))))
            .OrderByDescending(load => load.CreatedAt)
            .Take(take)
            .Select(load => new GlobalSearchResultDto
            {
                Type = "Load",
                Id = load.Id,
                Title = load.LoadNumber,
                Subtitle = load.Customer.Name + " - " + (load.Origin ?? "-") + " to " + (load.Destination ?? "-"),
                Route = "/loads/" + load.Id
            })
            .ToListAsync();
    }

    public Task<List<GlobalSearchResultDto>> SearchCustomersAsync(string query, int take)
    {
        var lowered = query.ToLower();

        return _db.Customers
            .AsNoTracking()
            .Where(customer =>
                customer.Name.ToLower().Contains(lowered) ||
                (customer.Email != null && customer.Email.ToLower().Contains(lowered)) ||
                (customer.Phone != null && customer.Phone.ToLower().Contains(lowered)))
            .OrderByDescending(customer => customer.CreatedAt)
            .Take(take)
            .Select(customer => new GlobalSearchResultDto
            {
                Type = "Customer",
                Id = customer.Id,
                Title = customer.Name,
                Subtitle = customer.Email ?? string.Empty,
                Route = "/customers/" + customer.Id
            })
            .ToListAsync();
    }

}
