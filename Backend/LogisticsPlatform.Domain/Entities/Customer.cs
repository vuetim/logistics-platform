using LogisticsPlatform.Domain.Common;
using LogisticsPlatform.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public CustomerBillingInfo Billing { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<CustomerAddress> Addresses { get; private set; } = [];
    public ICollection<CustomerContact> Contacts { get; private set; } = [];
    public ICollection<CustomerNote> Notes { get; private set; } = [];


    private Customer() { } // EF Core

    public Customer(
     string name,
     string? email,
     string? phone,
     bool isActive,
     CustomerBillingInfo billing)
    {
        Name = name;
        Email = email;
        Phone = phone;
        IsActive = isActive;
        Billing = billing;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    public void UpdateBasicInfo(string name, string? email, string? phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public void UpdateBilling(CustomerBillingInfo billing)
    {
        Billing = billing;
    }

    public void AddAddress(CustomerAddress address)
    {
        if (address.IsPrimary)
        {
            foreach (var a in Addresses)
                a.IsPrimary = false;
        }
        Addresses.Add(address);
    }

    public void AddContact(CustomerContact contact)
    {
        if (contact.IsPrimary)
        {
            foreach (var c in Contacts)
                c.IsPrimary = false;
        }
        Contacts.Add(contact);
    }

    public void AddNote(CustomerNote note)
    {
        Notes.Add(note);
    }
}
