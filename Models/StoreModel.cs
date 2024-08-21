using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultisiteEventing.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        public int DisplayId {get;set;}

        [Required(ErrorMessage = "Store name is required.")]
        [StringLength(100, ErrorMessage = "Store name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }

    [ComplexType]
    public class Location
    {
        [Key]
        public int Id { get; set; }

        public string? LocationName { get; set; }

        [Required(ErrorMessage = "Display ID is required.")]
        public int DisplayId {get;set;}

        [Required(ErrorMessage = "Address is required.")]
        public Address Address { get; set; }

        public int StoreId { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }

    [ComplexType]
    public class Address
    {
        [Required(ErrorMessage = "Street is required.")]
        [StringLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(2, ErrorMessage = "State must be 2 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        [StringLength(10, ErrorMessage = "Zip Code cannot be longer than 10 characters.")]
        public string ZipCode { get; set; }
    }

    [ComplexType]
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Contact name is required.")]
        [StringLength(100, ErrorMessage = "Contact name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot be longer than 50 characters.")]
        public string Role { get; set; }
    }
}


