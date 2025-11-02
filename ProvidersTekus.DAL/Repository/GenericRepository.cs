using Microsoft.EntityFrameworkCore;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DAL.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {

        private readonly BdprovidersContext _dbcontext;

        public GenericRepository(BdprovidersContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public async Task<TModel> Get(Expression<Func<TModel, bool>> filtro)
        {
            try
            {
                TModel modelo = await _dbcontext.Set<TModel>().Where(filtro).FirstOrDefaultAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModel>> GetAll(Expression<Func<TModel, bool>> filtro = null)
        {
            try
            {

                IQueryable<TModel> queryModel = filtro == null ? _dbcontext.Set<TModel>() : _dbcontext.Set<TModel>().Where(filtro);
                return queryModel;
            }
            catch { throw; }
        }

        public async Task<bool> Add(TModel model)
        {
            try
            {

                _dbcontext.Set<TModel>().Add(model);

                await _dbcontext.SaveChangesAsync();
                return true;

            }
            catch { throw; }
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Add(model);
                await _dbcontext.SaveChangesAsync();

                return model;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Update(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Delete(TModel model)
        {
            try
            {

                _dbcontext.Set<TModel>().Remove(model);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            catch { throw; }
        }


    }
}
