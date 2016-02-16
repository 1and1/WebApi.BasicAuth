using System.Configuration;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// A set of <see cref="User"/>s for HTTP Basic Authentication.
    /// </summary>
    public class UserCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new User();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((User)element).Username;
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName == "users";
        }
    }
}