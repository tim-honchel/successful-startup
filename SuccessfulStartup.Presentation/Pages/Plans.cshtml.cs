
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuccessfulStartup.Data.Entities;

namespace SuccessfulStartup.Presentation.Pages
{
    public class PlansModel : PageModel
    {
        public List<BusinessPlan> plans = new List<BusinessPlan>();
        public void OnGet()
        {
        }
    }
}
