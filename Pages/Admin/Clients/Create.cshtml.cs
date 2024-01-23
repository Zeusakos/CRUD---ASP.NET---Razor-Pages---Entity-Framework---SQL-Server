using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Pages.Admin.Clients
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment enviroment;

        [BindProperty]
        public ClientDto ClientDto { get; set; } = new ClientDto();

        public CreateModel(IWebHostEnvironment environment,ApplicationDbContext context) 
        {
            this.enviroment = environment;
            this.context = context;
        }

        public void OnGet()
        {
        }
        public string errorMessage = "";
        public string successMessage = "";

        public void OnPost() 
        {
            if (ClientDto.ImageFile == null)
            {
                ModelState.AddModelError("ClientDto.ImageFile", "The image file is required");
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            // save image on server

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ClientDto.ImageFile!.FileName);

            string imageFullPath = enviroment.WebRootPath + "/clients/" + newFileName;

            using(var stream = System.IO.File.Create(imageFullPath))
            {
                ClientDto.ImageFile.CopyTo(stream);
            }


            //database save

            Client client = new Client()
            {
                Name = ClientDto.Name,
                Email = ClientDto.Email,
                Phone = ClientDto.Phone,
                Address = ClientDto.Address,
                ImageFileName = newFileName,
                Created_at = DateTime.Now,
            };

            context.Clients.Add(client);
            context.SaveChanges();

            // clear the Form


            ClientDto.Name = "";
            ClientDto.Email = "";
            ClientDto.Phone = "";
            ClientDto.Address = "";
            ClientDto.ImageFile = null;

            ModelState.Clear();

            successMessage = "Product created successfully";

            Response.Redirect("/Admin/Clients/Index");

        }
    }
}
