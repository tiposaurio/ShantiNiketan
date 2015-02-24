//-----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// Web API Application class
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        #region Protected Methods

        /// <summary>
        /// Starts the application.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //// Tells WebApi to use our custom Ioc (Ninject)
            IocConfig.RegisterIoc(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //// Web API template created these 3
            GlobalConfig.CustomizeConfig(GlobalConfiguration.Configuration);
        }

        #endregion
    }
}
