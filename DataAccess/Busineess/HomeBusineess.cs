
using DataAccess.Dao;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Busineess
{
    public class HomeBusineess
    {
        private Context db = new Context();

        DantocRepository dantoc = new DantocRepository();
        UsserRepository _user = new UsserRepository();
        Login_FailRepository _lgf = new Login_FailRepository();
        Dictionary<string, object> _condition;
        BaseRepository _base = new BaseRepository();
        public Boolean BackUp_DB()
        {
            return _base.Backup_Demo();
        }
        public USERS GetByUseName(string inputUser)
        {
            USERS us = _user.GetByUseName(inputUser);
            return us;
        }
        public Boolean UpdateUser(USERS input)
        {
            return _user.Update(input);
        }
        public bool Write_LoginFail(string ip)
        {
            bool result = true;
            try
            {
                LOGIN_FAIL fail;
                _condition = new Dictionary<string, object>();
                _condition.Add("IP", ip);
                if (_lgf.GetAll(_condition).Count() == 0)
                {
                    fail = new LOGIN_FAIL();
                    fail.IFAILED = 1;
                    fail.IP = ip;
                    fail.DDATE = DateTime.Now;
                    _lgf.AddNew(fail);
                }
                else
                {

                    _condition = new Dictionary<string, object>();
                    _condition.Add("IP", ip);
                    fail = _lgf.GetAll(_condition).FirstOrDefault();
                    fail.IFAILED = fail.IFAILED + 1;
                    fail.DDATE = DateTime.Now;
                    _lgf.Update(fail);
                }
                result = true;
            }
            catch 
            {
                result = false;
            }
            return result;
           
        }
        public int Get_LoginFail(string ip)
        {
            LOGIN_FAIL fail;
            _condition = new Dictionary<string, object>();
            _condition.Add("IP", ip);
            if (_lgf.GetAll(_condition).Count() == 0)
            {
                return 0;
            }
            else
            {

                //_condition = new Dictionary<string, object>();
                //_condition.Add("IP", ip);
                fail = _lgf.GetAll(_condition).FirstOrDefault();
                //fail.IFAILED = fail.IFAILED + 1;
                //fail.DDATE = DateTime.Now;
                //_lgf.Update(fail);
                return (int)fail.IFAILED;
            }

        }
        public Boolean Remove_Loginfail(string ip, DateTime now)
        {
            
            try
            {                
                Dictionary<string, object> _fail = new Dictionary<string, object>();
                _fail.Add("IP", ip);
                var login_fail = _lgf.GetAll(_fail);
                if (login_fail.Count() > 0)
                {
                    LOGIN_FAIL failed = login_fail.FirstOrDefault();
                    _lgf.Delete(failed);
                }
                var all_fail = _lgf.GetAll().Where(x=>x.DDATE<now).ToList();
                foreach(var a in all_fail)
                {
                    _lgf.Delete(a);
                }
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }
    }
}
