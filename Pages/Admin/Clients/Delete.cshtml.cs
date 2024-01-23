using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Services;

namespace WebApplication2.Pages.Admin.Clients
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        public DeleteModel(IWebHostEnvironment environment,ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Clients/Index");
                return;
            }

            var client = context.Clients.Find(id);
            if (client == null)
            {
                Response.Redirect("/Admin/Clients/Index");
                return;
            }

            string imageFullPath = environment.WebRootPath + "/clients/" + client.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Clients.Remove(client);
            context.SaveChanges();
            Response.Redirect("/Admin/Clients/Index");
        }
    }
}
