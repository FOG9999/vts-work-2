using System;
using System.Transactions;

namespace DataAccess
{
    /// <summary>
    /// Quản trí transaction
    /// Rollback hay commit dữ liệu khi cần thiết
    /// </summary>
    public class Transaction : ITransaction
    {
        protected UnitOfWork uow { get; private set; }
        protected TransactionScope ts { get; private set; }

        /// <summary>
        /// Khởi tạo với unitofwork
        /// </summary>
        /// <param name="u"></param>
        public Transaction(UnitOfWork u)
        {
            this.uow = u;
            this.ts = new TransactionScope();
        }

        /// <summary>
        /// Lưu dữ liệu vào csdl
        /// </summary>
        public void Commit()
        {
            this.uow.Save();
            this.ts.Complete();
        }

        /// <summary>
        /// Xóa hết những gì đã tác động đến CSDL trong TransactionScope này
        /// </summary>
        public void Rollback()
        {
            //when thrown exception it does ifself in Dispose
        }

        /// <summary>
        /// Loại bỏ Transaction
        /// </summary>
        public void Dispose()
        {
            if (this.ts != null)
            {
                (this.ts as IDisposable).Dispose();
                this.ts = null;
                this.uow = null;
            }
        }
    }
}
