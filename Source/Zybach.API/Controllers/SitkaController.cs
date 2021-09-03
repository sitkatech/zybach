using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    public abstract class SitkaController<T> : ControllerBase
    {
        protected readonly ZybachDbContext _dbContext;
        protected readonly ILogger<T> _logger;
        protected readonly KeystoneService _keystoneService;
        protected readonly ZybachConfiguration _zybachConfiguration;

        protected SitkaController(ZybachDbContext dbContext, ILogger<T> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _keystoneService = keystoneService;
            _zybachConfiguration = zybachConfiguration.Value;
        }

        protected ActionResult RequireNotNullThrowNotFound(object theObject, string objectType, object objectID)
        {
            return ThrowNotFound(theObject, objectType, objectID, out var actionResult) ? actionResult : Ok(theObject);
        }

        protected bool ThrowNotFound(object theObject, string objectType, object objectID, out ActionResult actionResult)
        {
            if (theObject == null)
            {
                var notFoundMessage = $"{objectType} with ID {objectID} does not exist!";
                _logger.LogError(notFoundMessage);
                {
                    actionResult = NotFound(notFoundMessage);
                    return true;
                }
            }

            actionResult = null;
            return false;
        }
    }
}