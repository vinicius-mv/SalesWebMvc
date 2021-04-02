using SalesWebMvc.Models;
using System.Linq;
using System.Collections.Generic;

namespace SalesWebMvc.Services
{
    public class DepartmentsService
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentsService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            return _context.Departments.OrderBy(d => d.Name).ToList();
        }

    }
}
