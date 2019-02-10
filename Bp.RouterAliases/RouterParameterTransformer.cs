using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Bp.RouterAliases
{
    /// <summary>
    /// Transforms Routers URI.
    /// Removes the keyword 'Router'/'router' from the url, converts the url to kebab-case
    /// </summary>
    public class RouterParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// Transforms the specified route value to a string for inclusion in a URI.
        /// Removes the keyword 'Router'/'router' from the url, converts the url to kebab-case
        /// </summary>
        /// <param name="value">The route value to transform.</param>
        /// <returns>The transformed value.</returns>
        public string TransformOutbound(object value)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value.ToString().Replace("Router", ""), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}