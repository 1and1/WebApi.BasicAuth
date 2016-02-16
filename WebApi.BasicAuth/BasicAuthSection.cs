using System.Configuration;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// Configuration for HTTP Basic Authentication with a fixed list of users.
    /// </summary>
    public class BasicAuthSection : ConfigurationSection
    {
        /// <summary>
        /// The realm to report in authentication challenges.
        /// </summary>
        [ConfigurationProperty("realm", IsRequired = false)]
        public string Realm => (string)base["realm"];

        /// <summary>
        /// The users to accept for authentication.
        /// </summary>
        [ConfigurationProperty("users", IsRequired = true)]
        [ConfigurationCollection(typeof(UserCollection), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate, AddItemName = "user")]
        public UserCollection Users => (UserCollection)base["users"];

        /// <summary>
        /// Loads the configuration from the the application configuration file (Web.config or App.config).
        /// </summary>
        public static BasicAuthSection Load()
        {
            return (BasicAuthSection)ConfigurationManager.GetSection("basicAuth");
        }
    }
}