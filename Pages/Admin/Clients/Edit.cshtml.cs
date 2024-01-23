using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Admin.Clients
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ClientDto ClientDto { get; set; } = new ClientDto();
        public Client Client { get; set; } = new Client();
        public string errorMessage = "";
        public string successMessage = "";


        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
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

            ClientDto.Name = client.Name;
            ClientDto.Email = client.Email;
            ClientDto.Phone = client.Phone;
            ClientDto.Address = client.Address;
            ClientDto.ImageFile = null;

            Client = client;
        }

        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Clients/Index");
                return;
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            var client = context.Clients.Find(id);
            if (client == null)
            {
                Response.Redirect("/Admin/Clients/Index");
                return;
            }

            string newFileName = client.ImageFileName;
            if (ClientDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ClientDto.ImageFile.FileName);


                string imageFullpath = environment.WebRootPath + "/clients/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullpath))
                {
                    ClientDto.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = environment.WebRootPath + "/clients/" + client.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);


            }


            client.Name = ClientDto.Name;
            client.Email = ClientDto.Email;
            client.Phone = ClientDto.Phone;
            client.Address = ClientDto.Address;
            client.ImageFileName = newFileName;

            context.SaveChanges();

            Client = client;

            successMessage = "Client updated succssfully";
            Response.Redirect("/Admin/Clients/Index");

        }
    }
}
