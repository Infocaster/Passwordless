using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;

namespace Infocaster.Umbraco.Passwordless
{
    public class PasswordlessController
        : SurfaceController
    {
        public PasswordlessController(IUmbracoContextAccessor umbracoContextAccessor,
                                      IUmbracoDatabaseFactory databaseFactory,
                                      ServiceContext services,
                                      AppCaches appCaches,
                                      IProfilingLogger profilingLogger,
                                      IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        { }

        [HttpGet]
        public IActionResult FinishLogin(string? returnUrl)
        {

            return LocalRedirect(returnUrl ?? Url.GetBackOfficeUrl() ?? "/");
        }
    }
}
