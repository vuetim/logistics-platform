using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Enums;

public class CustomerContact : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; } = CustomerContactRoles.Other;
}
