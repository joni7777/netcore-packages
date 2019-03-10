namespace Bp.Common
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string SwaggerUrl => $"/api/swagger/{Version}/swagger.json";

        public ServiceInfo()
        {
        }
    }
}