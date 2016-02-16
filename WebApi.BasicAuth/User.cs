using System.Configuration;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// A specific user for HTTP Basic Authentication.
    /// </summary>
    public class User : ConfigurationElement
    {
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username => (string)base["username"];

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password => (string)base["password"];

        [ConfigurationProperty("roles", IsRequired = true)]
        [ConfigurationCollection(typeof(RoleCollection), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate, AddItemName = "role")]
        public RoleCollection Roles => (RoleCollection)base["roles"];
    }
}