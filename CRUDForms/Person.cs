//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUDForms
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> ClientTypeId { get; set; }
        public Nullable<int> ContactTypeId { get; set; }
        public string SupportStaff { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool Enabled { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual ClientType ClientType { get; set; }
        public virtual ContactType ContactType { get; set; }
    }
}