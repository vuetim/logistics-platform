namespace LogisticsPlatform.Application.Interfaces.Repositories.Customers
{
    public interface ICustomerContactRepository
    {
        Task<IEnumerable<CustomerContact>> GetByCustomerAsync(Guid customerId);

        Task<CustomerContact?> GetByIdAsync(Guid id);

        Task<List<CustomerContact>> GetPrimaryByCustomerAsync(Guid customerId);

        Task AddAsync(CustomerContact contact);

        void Update(CustomerContact contact);

        void Remove(CustomerContact contact);
    }

}
