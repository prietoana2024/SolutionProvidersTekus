using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DAL.Repository.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : class
    {

        Task<TModel> Get(Expression<Func<TModel, bool>> filtro);

        Task<TModel> Create(TModel model);

        Task<bool> Update(TModel model);

        Task<bool> Delete(TModel model);

        Task<IQueryable<TModel>> GetAll(Expression<Func<TModel, bool>> filtro = null);

        Task<bool> Add(TModel model);

    }
}
