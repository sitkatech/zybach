using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;

namespace Zybach.API.Controllers
{
    public abstract class SitkaApiController<T> : ControllerBase
    {
        protected readonly ZybachDbContext _dbContext;
        protected readonly ILogger<T> _logger;

        protected SitkaApiController(ZybachDbContext dbContext, ILogger<T> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}