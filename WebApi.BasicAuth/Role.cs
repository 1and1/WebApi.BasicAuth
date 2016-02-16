using System.Configuration;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// A specific role of a <see cref="User"/>.
    /// </summary>
    public class Role : ConfigurationElement
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)base["name"];
    }
}