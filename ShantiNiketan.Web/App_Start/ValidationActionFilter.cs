//-----------------------------------------------------------------------
// <copyright file="ValidationActionFilter.cs" company="Shanti Niketan">
//     Copyright (c) Shanti Niketan. All rights reserved.
// </copyright>
// <author>Agustín Cassani</author>
//-----------------------------------------------------------------------
namespace ShantiNiketan.Web
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Validation Action Filter class
    /// </summary>
    public class ValidationActionFilter : ActionFilterAttribute
    {
        #region Public Methods

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="context">Contains information for the executing action.</param>
        public override void OnActionExecuting(HttpActionContext context)
        {
            var modelState = context.ModelState;

            if (modelState.IsValid)
            {
                return;
            }

            var errors = new JObject();

            foreach (var key in modelState.Keys)
            {
                var state = modelState[key];

                if (state.Errors.Any())
                {
                    errors[key] = state.Errors.First().ErrorMessage;
                }
            }

            context.Response = context.Request.CreateResponse<JObject>(HttpStatusCode.BadRequest, errors);
        }

        #endregion
    }
}