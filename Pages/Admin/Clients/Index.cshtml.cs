using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Services;
using WebApplication2.Models;

namespace WebApplication2.Pages.Admin.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        //pagination
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 15;
        //search

        public string search = "";
        public string column = "id";
        public string orderBy = "desc";


        public List<Client> Clients { get; set; } = new List<Client>();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int? pageIndex, string? search,string? column,string? orderBy)
        {
            IQueryable<Client> query = context.Clients;

            //search func
            if (search !=null)
            {
                this.search = search;
                query = query.Where(c => c.Name.Contains(search) || c.Email.Contains(search));
            }

            //sort

            string[] validColumns = {"id","name", "email","phone","address", "created_at"};
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumns.Contains(column))
            {
                column = "id";
            }

            if (!validOrderBy.Contains(column))
            {
                orderBy = "desc"; 
            }

            this.column = column;
            this.orderBy = orderBy;



            switch (column.ToLower())
            {
                case "name":
                    query = orderBy == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "email":
                    query = orderBy == "asc" ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email);
                    break;
                case "phone":
                    query = orderBy == "asc" ? query.OrderBy(c => c.Phone) : query.OrderByDescending(c => c.Phone);
                    break;
                case "address":
                    query = orderBy == "asc" ? query.OrderBy(c => c.Address) : query.OrderByDescending(c => c.Address);
                    break;
                case "createdat":
                    query = orderBy == "asc" ? query.OrderBy(c => c.Created_at) : query.OrderByDescending(c => c.Created_at);
                    break;
                default:
                    query = orderBy == "asc" ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
            }

            // query = query.OrderByDescending(c => c.Id);

            if (pageIndex == null || pageIndex <1)
            {
                pageIndex = 1;
            }
            this.pageIndex = (int) pageIndex;

            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count/pageSize);

            query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);

            Clients = query.ToList();
        }
    }
}
