using System;

using Entities.Models;

namespace DataAccess
{
    /// <summary>
    /// Quản lý các repository và context kết nối đến csdl
    /// Khởi tạo transaction
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {



        private Context _context;

        public Context Context
        {
            get { return this._context; }
            set { this._context = value; }
        }

        public UnitOfWork()
        {
            InitializeContext();
        }

        /// <summary>
        /// Khởi tạo kết nối
        /// </summary>
        protected void InitializeContext()
        {
            _context = new Context();
            _context.Configuration.LazyLoadingEnabled = true;
            //_context.Database.Connection.ConnectionString = OracleHelper.GetConnectionString();
        }

        #region REPOSITORY
       // public ILocalRepository Local { get { return new LocalRepository(); } }
        #endregion




        #region TRANSACTION
        /// <summary>
        /// Lấy thông tin về transaction hiện tai
        /// </summary>
        /// <returns></returns>
        public ITransaction BeginTransaction()
        {
            return new Transaction(this);
        }

        /// <summary>
        /// Kết thúc trangsactions
        /// </summary>
        /// <param name="transaction"></param>
        public void EndTransaction(ITransaction transaction)
        {
            if (transaction != null)
            {
                (transaction as IDisposable).Dispose();
                transaction = null;
            }
        }
        #endregion TRANSACTION

        #region SAVE AND DISPOSE DATA

        /// <summary>
        /// Lưu dữ liệu xuống server
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        /// <summary>
        /// Lưu vào giải phóng kết nối
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion SAVE AND DISPOSE DATA
    }
}
