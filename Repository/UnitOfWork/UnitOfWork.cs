using DAL.dbcontext;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ecommercedatabase _context;
        private Dictionary<Type, object> _repositories;
        public UnitOfWork(ecommercedatabase context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }
    }
}
