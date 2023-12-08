using DAL.dbcontext;
using Entity.EntityClass;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BookRepository : GenericRepository<Book> , IBookRepository
    {
        protected readonly ecommercedatabase _context;
        public BookRepository(ecommercedatabase _context) : base(_context)
        {
            this._context = _context;
        }

    }
}
