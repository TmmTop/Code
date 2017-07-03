using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;

/// <summary>
///LogManage 的摘要说明
/// </summary>
public class LogManage
{
	public LogManage()
	{
		
	}


    private static String m_logPath  = String.Empty;
    /// <summary>
    /// 保存日志的文件夹
    /// </summary>
    public static string LogPath 
    {
        get
        {
            if (m_logPath ==string.Empty )
            {
                if (System.Web.HttpContext.Current == null)
                {
                    m_logPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else 
                {
                    m_logPath=AppDomain.CurrentDomain.BaseDirectory+@"Log\";  
                }
            }
            return m_logPath;
        }
        set { m_logPath = value; }
    }

    private static String m_logFielPrefix = String.Empty;
    
    /// <summary>
    /// 日志文件前缀
    /// </summary>
    public static String LogFielPrefix
    {
        get { return LogManage.m_logFielPrefix; }
        set { LogManage.m_logFielPrefix = value; }
    }

    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="logFile"></param>
    /// <param name="msg"></param>
    public static void WriteLog(string logFile, string msg)
    {
        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }
        System.IO.StreamWriter sw = System.IO.File.AppendText(LogPath + LogFielPrefix + logFile + " " + DateTime.Now.ToString("yyyyMMdd") + ".Log");
        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + "\n  \n" + msg+"\n  \n");
        sw.Close();
    }

    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="logFile"></param>
    /// <param name="msg"></param>
    public static void WriteLog(LogFile logFile, string msg)
    {
        WriteLog(logFile.ToString(), msg);
    }

   






    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL,
    }



    
//--系统日志
//create table SystemLog
//(
//    LogID int not null identity(1,1) primary key,
//    LogType nvarchar(20),		--日志类型
//    Severity nvarchar(20),		--严重性
//    Message nvarchar(max),		--日志内容
//    Exception nvarchar(max),	--异常内容
//    IPAddress nvarchar(20),		--IP地址
//    UserName nvarchar(20),		--用户名
//    UserType nvarchar(20),		--用户类型
//    PageURL nvarchar(200),		--页面地址
//    ReferrerURL nvarchar(200),	--页面引用地址
//    CreateTime datetime,		--创建时间
	
//) 


}


