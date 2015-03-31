using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsFormsApplication1
{
    public class DirectoryHelper
    {
       
        /// <summary>
        /// 根据当前日组合文件夹名称
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static string BuildDateDirectoryName(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BuildTodayDirectoryName()
        {
            return BuildDateDirectoryName(DateTime.Now);
        }
        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string directory)
        {
            return !Directory.Exists(directory) ? string.Empty : new DirectoryInfo(directory).Name;
        }

        /// <summary>
        /// 获取目录文件夹下的所有子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filePattern"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory, int maxCount)
        {
            var subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }
            if (maxCount <= 0)
            {
                return subDirectories;
            }
            string[] directories = Directory.GetDirectories(directory);
            foreach (string subDirectory in directories)
            {
                if (subDirectories.Count == maxCount)
                {
                    break;
                }
                subDirectories.Add(subDirectory);
            }
            return subDirectories;
        }
        public static List<string> FindSubDirectories(string directory)
        {
            return Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).ToList<string>();
        }
        /// <summary>
        /// 根据时间查询子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory, int maxCount, int days)
        {
            List<string> subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }
            if (maxCount <= 0)
            {
                return subDirectories;
            }
            string[] directories = Directory.GetDirectories(directory);
            DateTime lastTime = DateTime.Now.AddDays(-Math.Abs(days));
            foreach (string subDirectory in directories)
            {
                if (subDirectories.Count == maxCount)
                {
                    break;
                }
                DirectoryInfo dirInfo = new DirectoryInfo(subDirectory);
                if (dirInfo.LastWriteTime >= lastTime)
                {
                    subDirectories.Add(subDirectory);
                }
            }
            return subDirectories;
        }
        /// <summary>
        /// 将原目录移动到目标目录
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public static bool MoveDirectory(string sourceDirectory, string targetDirectory)
        {
            if (string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(targetDirectory))
            {
                return false;
            }
            string laseMoveDirectory = string.Format("{0}\\{1}", targetDirectory, DirectoryHelper.GetDirectoryName(sourceDirectory));
            while (Directory.Exists(laseMoveDirectory))
            {
                laseMoveDirectory = DirectoryHelper.Rename(laseMoveDirectory);
            }
            Directory.Move(sourceDirectory, laseMoveDirectory);
            return true;
        }
        /// <summary>
        /// 重新生成新的文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string Rename(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }
            string lastDirName = DirectoryHelper.GetDirectoryName(filePath);
            //重命名，则随机在原来文件名后面加几个随机数字进行组装成新的名字
            Random random = new Random(System.DateTime.Now.Millisecond);
            string randomData = random.Next().ToString();
            //把原文件名的名字加上随机数，组装成新的文件名（避免重名）
            string newlastDirName = lastDirName + randomData;
            string newDirPath = string.Empty;
            newDirPath = filePath.Substring(0, filePath.LastIndexOf("\\")) + "\\" + newlastDirName;
            //返回新的路径
            return newDirPath;
        }
        /// <summary>
        /// 获取指定目录下的所有文件和文件夹大小
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>string，返回所有文件夹名字</returns>
        public static FileInfo[] GetDirectorySize(string path)
        {
            return new DirectoryInfo(path).GetFiles("*.jpg");
        }
    }
}
