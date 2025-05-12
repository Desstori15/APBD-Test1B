
namespace WorkshopVisitApi.DTOs
{
    public class VisitCreateDto
    {
        public int VisitId { get; set; }
        public int ClientId { get; set; }
        public string MechanicLicenceNumber { get; set; }
        public List<ServiceDto> Services { get; set; }
    }

    public class ServiceDto
    {
        public string ServiceName { get; set; }
        public decimal ServiceFee { get; set; }
    }
}
