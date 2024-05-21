﻿using Domain.Interfaces.Generics;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastructure.Repository.Generics
{
    public class RepositoryGenerics<T> : IGenerics<T>, IDisposable where T : class
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositoryGenerics()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task Add(T entity)
        {
            using (var data = new ContextBase(this._OptionsBuilder))
            {
                await data.Set<T>().AddAsync(entity);
                await data.SaveChangesAsync();
            }
        }

        public async Task Delete(T entity)
        {
            using (var data = new ContextBase(this._OptionsBuilder))
            {
                data.Set<T>().Remove(entity);
                await data.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAll()
        {
            using (var data = new ContextBase(this._OptionsBuilder))
            {
                return await data.Set<T>().ToListAsync();
            }
        }

        public async Task<T> GetEntityById(int id)
        {
            using (var data = new ContextBase(this._OptionsBuilder))
            {
                return await data.Set<T>().FindAsync(id);
            }
        }

        public async Task Update(T entity)
        {
            using (var data = new ContextBase(this._OptionsBuilder))
            {
                data.Set<T>().Update(entity);
                await data.SaveChangesAsync();
            }
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose

        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
    }
}