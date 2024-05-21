using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Generics
{
    public interface IGenerics<T> where T : class
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task<List<T>> GetAll();
        Task Update(T entity);
        Task<T> GetEntityById(int id);

    }
}
