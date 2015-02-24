//-----------------------------------------------------------------------
// <copyright file="GlobalConfig.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Global Configuration class
    /// </summary>
    public static class GlobalConfig
    {
        #region Public Methods

        /// <summary>
        /// Customizes global configuration
        /// </summary>
        /// <param name="config">Represents a configuration of HttpServer instances.</param>
        public static void CustomizeConfig(HttpConfiguration config)
        {
            //// Removes Xml formatters. This means when we visit an endpoint from a browser,
            //// Instead of returning Xml, it will return Json.
            //// More information from Dave Ward: http://jpapa.me/P4vdx6
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //// Configures json camelCasing per the following post: http://jpapa.me/NqC2HH
            //// Here we configure it to write JSON property names with camel casing
            //// without changing our server-side data model:
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ((DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver).IgnoreSerializableAttribute = true;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //// Adds model validation, globally
            config.Filters.Add(new ValidationActionFilter());
        }

        #endregion
    }
}