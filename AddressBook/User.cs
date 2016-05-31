using System;

namespace AddressBook
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime TimeAdded { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
}