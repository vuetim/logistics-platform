namespace LogisticsPlatform.Application.Interfaces.Repositories.Customers
{
    public interface ICustomerContactRepository
    {
        Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId);
        Task<CustomerContact?> GetByIdAsync(Guid id);

        Task AddAsync(CustomerContact contact);
        Task UpdateAsync(CustomerContact contact);
        Task DeleteAsync(CustomerContact contact);
        Task<List<CustomerContact>> GetPrimaryByCustomerAsync(Guid customerId);

        Task SaveChangesAsync();
    }
}
