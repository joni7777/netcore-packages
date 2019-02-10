using Microsoft.AspNetCore.Mvc;

namespace Bp.RouterAliases
{
    /// <summary>
    /// Indicates that a type and all derived types are used to serve HTTP API responses.
    /// <para>
    /// Routers decorated with this attribute are configured with features and behavior targeted at improving the
    /// developer experience for building APIs.
    /// </para>
    /// <para>
    /// When decorated on an assembly, all routers in the assembly will be treated as routers with API behavior.
    /// </para>
    /// </summary>
    public class ApiRouterAttribute : ApiControllerAttribute
    {
    }
}