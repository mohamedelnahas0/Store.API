using Store.Data.Context;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositores;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()

          => await _context.SaveChangesAsync();
        

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (_repositores is null)
                _repositores = new Hashtable();

            var entiteyKey = typeof(TEntity).Name;  //Repository<Product , int>

            if (!_repositores.ContainsKey(entiteyKey))
            {
                var repositoryType = typeof(GenericRepository<,>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity),typeof(TKey)), _context);

                _repositores.Add(entiteyKey, repositoryInstance);
            }
            return (IGenericRepository<TEntity,TKey>) _repositores[entiteyKey];
        }
    }
}
