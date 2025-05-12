
using System.ComponentModel.DataAnnotations;

namespace WorkshopVisitApi.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }
        public int ClientId { get; set; }
        public int MechanicId { get; set; }
        public DateTime Date { get; set; }

        public Client Client { get; set; }
        public Mechanic Mechanic { get; set; }
        public List<VisitService> VisitServices { get; set; }
    }

    public class VisitService
    {
        public int VisitId { get; set; }
        public int ServiceId { get; set; }
        public decimal ServiceFee { get; set; }

        public Visit Visit { get; set; }
        public Service Service { get; set; }
    }

    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class Mechanic
    {
        public int MechanicId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenceNumber { get; set; }
    }

    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public decimal BaseFee { get; set; }
    }
}
