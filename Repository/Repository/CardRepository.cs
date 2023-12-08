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
    public class CardRepository : GenericRepository<Card> , ICardRepository
    {
        protected readonly ecommercedatabase _context;
        public CardRepository(ecommercedatabase _context) : base(_context)
        {
            this._context = _context;
        }
    }
}
