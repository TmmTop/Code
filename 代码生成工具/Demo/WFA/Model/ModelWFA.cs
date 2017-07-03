using System;

namespace Model
{ 
    public enum SPActionsWFA 
    {
        Insert,
        Update,
        Delete,
        SelectPaging,
        SelectAll,
        Unique
    }

    public class ModelWFA 
    { 
        /// <summary> 
        ///   
        /// </summary> 
        public int ID {get; set;}

        /// <summary> 
        ///   
        /// </summary> 
        public string Value {get; set;}


        /// <summary>
        /// 读取数据的开始索引
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 读取数据每页的条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 存储过程返回值类型
        /// </summary>
        public AppEnum.SPReturnTypes SPReturnType { get; set; }

        /// <summary>
        /// 存储过程动作
        /// </summary>
        public string SPAction {get; set;}
    }
} 

