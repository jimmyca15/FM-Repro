using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace repro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFeatureManagerSnapshot _featureManager;

        public IndexModel(ILogger<IndexModel> logger, IFeatureManagerSnapshot featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        public async Task OnGet()
        {
            ViewData["Message"] = $"Home is {(await _featureManager.IsEnabledAsync("Home") ? "Enabled" : "Disabled")}.";
        }
    }
}
