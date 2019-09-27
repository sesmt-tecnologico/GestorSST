using System;

namespace Gestor.Domain.Repositories
{
    public interface IImageRepository
    {
        void Upload<T>(Guid id, string imageBase64) where T : class;

        byte[] Get<T>(Guid id) where T : class;

        bool Has<T>(Guid id) where T : class;
    }
}