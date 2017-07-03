using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using Model;

namespace BLL
{
    public static class BllWFA 
    {
        /// <summary> 
        /// Select Paging Data 
        /// </summary> 
        /// <param name="obj">TableWFA Model</param> 
        /// <param name="errMsg">错误信息</param> 
        /// <returns>DataSet</returns> 
        public static DataSet SelectPaging(ModelWFA obj, out string errMsg) 
        {
            object returnObj = MainDal.DoDal(obj, "TableWFA", new List<object> { SPActionsWFA.SelectPaging }, out errMsg); 
            if (!string.IsNullOrEmpty(errMsg) || returnObj == null) 
            {
                return null;
            }
            return (DataSet)returnObj;
        }

        /// <summary> 
        /// Select All Data 
        /// </summary> 
        /// <param name="obj">TableWFA Model</param> 
        /// <param name="errMsg">错误信息</param> 
        /// <returns>DataSet</returns> 
        public static DataSet SelectAll(ModelWFA obj, out string errMsg) 
        {
            object returnObj = MainDal.DoDal(obj, "TableWFA", new List<object> { SPActionsWFA.SelectAll }, out errMsg); 
            if (!string.IsNullOrEmpty(errMsg)) 
            {
                return null;
            }
            return (DataSet)returnObj;
        }

        /// <summary> 
        /// Insert Unique Data 
        /// </summary> 
        /// <param name="obj">TableWFA Model</param> 
        /// <param name="errMsg">错误信息</param> 
        /// <returns>long</returns> 
        public static long InsertUnique(ModelWFA obj, out string errMsg) 
        {
            object returnObj = MainDal.DoDal(obj, "TableWFA", new List<object> { SPActionsWFA.Unique, SPActionsWFA.Insert }, out errMsg); 
            if (!string.IsNullOrEmpty(errMsg)) 
            {
                return -1;
            }
            return Convert.ToInt64(returnObj);
        }

        /// <summary> 
        /// Update Unique Data 
        /// </summary> 
        /// <param name="obj">TableWFA Model</param> 
        /// <param name="errMsg">错误信息</param> 
        public static void UpdateUnique(ModelWFA obj, out string errMsg) 
        {
            MainDal.DoDal(obj, "TableWFA", new List<object> { SPActionsWFA.Unique, SPActionsWFA.Update }, out errMsg); 
        }

        /// <summary> 
        /// Delete Data 
        /// </summary> 
        /// <param name="obj">TableWFA Model</param> 
        /// <param name="errMsg">错误信息</param> 
        public static void Delete(ModelWFA obj, out string errMsg) 
        {
            MainDal.DoDal(obj, "TableWFA", new List<object> { SPActionsWFA.Delete }, out errMsg); 
        }
    }
}

