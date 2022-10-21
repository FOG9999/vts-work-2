using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    /// <summary>
    /// Hàm được viết bởi Hoàng Dũng - BU Y Tế - Thuộc trung tâm giải pháp và phát triển phần mềm viễn thông VIETEL-ICT
    /// </summary>
    public abstract class AbstractDataProvider
    {
        public string DatabaseString;
        private static AbstractDataProvider instance = null;
        public static AbstractDataProvider Instance
        {
            get
            {
                if (instance == null) instance = new OracleDataProvider("Context");
                return instance;
            }
        }
        public abstract bool InsertUsingTransaction<T>(List<T> datainsert, string DataTableName, string constring);
        public abstract bool InsertUsingTransaction(DataTable datainsert, string DataTableName);
        public abstract bool InsertUsingTransaction<T>(List<T> datainsert, string DataTableName);

        /// <summary> Thực hiện thêm , cập nhật , xóa dữ liệu theo store produre name chỉ quan tâm đến đối số đầu vào
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract int ExecuteNonQuery(string spName, params object[] inputparameterValues);
        public abstract int ExecuteNonQueryInsert<T>(T obj);
        public abstract int ExecuteNonQueryInsert<T>(T obj, string DataTableName);
        public abstract int ExecuteNonQueryUpdate<T>(T obj, params string[] fieldCompare);
        public abstract int ExecuteNonQueryUpdate<T>(string fieldColumnUpdate, T obj, params string[] fieldCompare);
        /// <summary>  
        ///Thực hiện cập nhật dữ liệu từ một object khác vào object gốc trong CSDL . Cột có trong Table dữ liệu ghi đầu , có trong object ghi thứ 2 , phân cách nhau bằng dấu bằng = , phân định từng đối tượng thông qua dấu ','
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataTableName"></param>
        /// <param name="fieldColumnUpdate"></param>
        /// <param name="obj"></param>
        /// <param name="fieldCompare"></param>
        /// <returns></returns>
        public abstract int ExecuteNonQueryUpdate<T>(string DataTableName, string fieldColumnUpdate, T obj, params string[] fieldCompare);
        public abstract int ExecuteNonQueryUpdate<T>(List<T> dataupdate, params string[] fieldCompare);
        public abstract int ExecuteNonQueryUpdate<T>(string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare);
        /// <summary>
        /// Thực hiện cập nhật dữ liệu từ một object khác vào object gốc trong CSDL . Cột có trong Table dữ liệu ghi đầu , có trong object ghi thứ 2 , phân cách nhau bằng dấu bằng = , phân định từng đối tượng thông qua dấu ','
        /// </summary>
        /// <typeparam name="T">Đối tượng</typeparam>
        /// <param name="DataTableName">Tên bảng cần update</param>
        /// <param name="fieldColumnUpdate">Các trường cần update (Cột trong db ghi trước = cột trong object) </param>
        /// <param name="dataupdate">Danh sách đối tượng update</param>
        /// <param name="fieldCompare">Các trường cần so sánh (Cột trong db ghi trước = cột trong object)</param>
        /// <returns></returns>
        public abstract int ExecuteNonQueryUpdate<T>(string DataTableName, string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare);

        #region Bổ sung dành cho việc mở Connect sang Server khác

        public abstract int ExecuteNonQueryUpdateConnectionOther<T>(string ConnectStr, string DataTableName, string fieldColumnUpdate, List<T> dataupdate, params string[] fieldCompare);

        #endregion

        public abstract int ExecuteNonQueryDelete<T>(string DataTableName, T obj, params string[] fieldCompare);
        public abstract int ExecuteNonQueryOrtherDelete<T>(string connString, string DataTableName, T obj, params string[] fieldCompare);
        public abstract int ExecuteNonQueryDeleteAll<T>();
        public abstract int ExecuteNonQueryDeleteAll<T>(string DataTableName);
        public abstract int ExecuteNonQueryDelete<T>(params string[] fieldCompare);

        #region Thực hiện xóa dữ liệu theo trường so sánh theo một list danh sách

        public abstract int ExecuteNonQueryDelete<T>(List<T> data, params string[] fieldCompare);
        public abstract int ExecuteNonQueryDelete<T>(string DataTableName, List<T> data, params string[] fieldCompare);
        public abstract int ExecuteNonQueryOrtherConnectDelete<T>(string connString, List<T> data, params string[] fieldCompare);
        public abstract int ExecuteNonQueryOrtherConnectDelete<T>(string connString, string DataTableName, List<T> data, params string[] fieldCompare);        
        
        #endregion
                    /// <summary> Thực hiện thêm , cập nhật , xóa dữ liệu theo store produre name có dữ liệu đầu vào và một giá trị number output
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="outputparameterValue"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract object ExecuteNonQuery(out object outputparameterValue , string spName, params object[] inputparameterValues);

        /// <summary> Thực hiện thêm , cập nhật , xóa dữ liệu theo store produre name có dữ liệu đầu vào và nhiều giá trị output {number , Datatable}
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="ArrayoutparameterValues"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract object ExecuteNonQuery(out List<object> ListoutputparameterValues, string spName, params object[] inputparameterValues);
        public abstract int ExecuteNonQueryUsingCommandText(string CommandText);
        public abstract bool ExecuteNonQueryUsingCommandText(string CommandText, params object[] inputparameterValues);
        public abstract bool ExecuteNonQueryUsingCommandTextOrtherConnect(string Connectstr, string CommandText, params object[] inputparameterValues);
        /// <summary> Trả về một SYS_REFCURSOR với các giá trị input đầu vào dưới dạng dataset
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataset(string spName, params object[] inputparameterValues);
        
        /// <summary> Trả về một SYS_REFCURSOR với các giá trị input đầu vào
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract IDataReader ExecuteReader(string spName, params object[] inputparameterValues);

        /// <summary> Trả về một SYS_REFCURSOR và output number với các giá trị input đầu vào
        /// 
        /// </summary>
        /// <param name="outputparameterValue"></param>
        /// <param name="spName"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract IDataReader ExecuteReader(out object outputparameterValue, string spName, params object[] inputparameterValues);

        /// <summary> Trả về một hoặc nhiều SYS_REFCURSOR dưới dạng datatable và OracleDataReader dành cho SYS_REFCURSOR gốc , output number với các giá trị input đầu vào
        /// 
        /// </summary>
        /// <param name="ListoutputparameterValues"></param>
        /// <param name="spName"></param>
        /// <param name="inputparameterValues"></param>
        /// <returns></returns>
        public abstract IDataReader ExecuteReader(out List<object> ListoutputparameterValues, string spName, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandText(string CommandText);
        public abstract IDataReader ExecuteReaderUsingCommandText(string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextFillterColumn(object inputFillterColumn , string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextFillterColumnAndOr(object inputFillterColumn, string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextFillterColumnAndOrOtherConnection(string Connectstr, object inputFillterColumn, string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextFillterContainColumn(object inputFillterColumn, string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextFillterContainColumnAndOr(object inputFillterColumn, string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandTextOrtherConnection(string ConnectStr, string CommandText, params object[] inputparameterValues);
        public abstract IDataReader ExecuteReaderUsingCommandText(out object outputparameterValue , string CommandText );
        public abstract IDataReader ExecuteReaderUsingCommandText(out List<object> ListoutputparameterValues, string CommandText);
        public abstract IDataReader ExecuteReaderUsingCommandText<T>(List<T> listdata, params string[] fieldCompare);
        public abstract IDataReader ExecuteReaderUsingCommandText<T>(string DataTableName, List<T> listdata, params string[] fieldCompare);        
        public abstract IDataReader ExecuteReaderUsingCommandTextOrtherConnection<T>(string ConnectStr, List<T> listdata, params string[] fieldCompare);
        public abstract IDataReader ExecuteReaderUsingCommandTextOrtherConnection<T>(string ConnectStr, string DataTableName, List<T> listdata, params string[] fieldCompare);
        public abstract object ExecuteScalar(string spName, params object[] inputparameterValues);
        public abstract object ExecuteScalarUsingCommandText(string CommandText);
        public abstract object ExecuteScalarUsingCommandText(string CommandText, params object[] inputparameterValues);
        public abstract object ExecuteScalarMaxValue<T>(string inputColumnName);
        public abstract bool InsertUsingTransactionOrtherConnection(string ConnectionStr, DataTable datainsert, string DataTableName);
        public abstract bool InsertUsingTransactionConnectString<T>(List<T> datainsert, string DataTableName, string constr);
    }
}
