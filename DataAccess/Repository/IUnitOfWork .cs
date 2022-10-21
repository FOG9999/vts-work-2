using System;
using DataAccess.Dao;

namespace DataAccess
{
    /// <summary>
    /// Interface UnitOfWork
    /// UnitOfWork đứng ra quản lý toàn bộ các Repository và các transaction trong 1 phiên làm việc
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        ITransaction BeginTransaction();
        void EndTransaction(ITransaction transaction);

        #region PHẦN DÀNH CHO KHAI BÁO CÁC IREPOSITORY

        //ILocalRepository Local { get; }

        #endregion PHẦN DÀNH CHO KHAI BÁO CÁC IREPOSITORY
    }
}
