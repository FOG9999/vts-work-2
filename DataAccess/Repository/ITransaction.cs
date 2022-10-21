using System;

namespace DataAccess
{
    /// <summary>
    /// Interface Transaction
    /// </summary>
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
