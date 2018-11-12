using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApiDemo.Common
{
    public static class Function
    {
        public static string WriteLog(string strList)
        {
            try
            {
                //如果传过strList无内容，直接返回，不写日志  
                if (string.IsNullOrEmpty(strList.ToString())) return "";
                //获取本地服务器路径  
                string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/log/");
                //string strDicPath =  "/log/";

                //创建日志路径  
                string strPath = strDicPath + string.Format("{0:yyyy年-MM月-dd日}", DateTime.Now) + "日志记录.txt";
                //如果服务器路径不存在，就创建一个  
                if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
                //如果日志文件不存在，创建一个  
                if (!File.Exists(strPath)) using (FileStream fs = File.Create(strPath)) ;
                //读取日志文件中的信息  
                string str = File.ReadAllText(strPath);
                StringBuilder sb = new StringBuilder();
                //将错误信息写入sb  

                sb.Append("\r\n" + DateTime.Now.ToString() + "-----" + strList + "");

                //将错误信息写入txt  
                File.WriteAllText(strPath, sb.ToString() + "\r\n-----z-----\r\n" + str);
                return "";
            }
            catch (Exception ex) { return ex.ToString(); }
        }
    }
}