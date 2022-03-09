namespace OzonEdu.MerchandiseService.Infrastructure.Configuration
{
    public class KafkaConfiguration
    {
        public string GroupId { get; set; }
        
        public string Topic { get; set; }
        
        public string BootstrapServers { get; set; }
    }
}