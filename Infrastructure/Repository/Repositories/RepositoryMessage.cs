using Domain;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositoryMessage()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }
    }
}