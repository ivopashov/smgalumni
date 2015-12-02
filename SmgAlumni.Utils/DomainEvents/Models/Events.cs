using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System;

namespace SmgAlumni.Utils.DomainEvents.Models
{
    public class AddCauseDomainEvent : ListingDomainEvent
    {
        public string Body { get; set; }
    }

    public class AddNewsDomainEvent : ListingDomainEvent
    {
        public string Body { get; set; }
    }

    public class AddListingDomainEvent : ListingDomainEvent
    {
        public string Body { get; set; }
    }

    public class DeleteCauseDomainEvent : ListingDomainEvent
    {
    }

    public class DeleteNewsDomainEvent : ListingDomainEvent
    {
    }

    public class DeleteListingDomainEvent : ListingDomainEvent
    {
    }

    public class ModifyCauseDomainEvent : ListingDomainEvent
    {
    }

    public class ModifyNewsDomainEvent : ListingDomainEvent
    {
    }

    public class VerifyUserEvent : IDomainEvent
    {
        public string UserName { get; set; }
        public User User { get; set; }
    }

    public class ForgotPasswordEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string RequestScheme { get; set; }
        public string RequestAuthority { get; set; }
    }

    public interface IDomainEvent { }

    public interface IHandleDomainEvent<T> where T : IDomainEvent
    {
        void Handle(T args);
    }

    public class ListingDomainEvent :IDomainEvent
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Heading { get; set; }
    }

    public class RegisterUserDomainEvent : IDomainEvent
    {
        public User User { get; set; }
        public DateTime RegisteredOn{ get; set; }
    }
}
