using Microsoft.AspNetCore.Mvc;

namespace Bp.RouterAliases
{
    /// <summary>
    /// An alias for the base class for an MVC controller without view support.
    /// Adds the attribute for the base route: api/[router] and ApiRouter attribute
    /// </summary>
    [Route("api/[controller]"), ApiRouter]
    public class RouterBase : ControllerBase
    {
    }
}