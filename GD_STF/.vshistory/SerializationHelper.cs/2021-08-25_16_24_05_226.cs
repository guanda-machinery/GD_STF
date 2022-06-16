using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO.Compression;

namespace GD_STD
{
    /// <summary>
    /// 二進制序列化處理幫手
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// 序列化二進制檔案 (壓縮)
        /// </summary>
        /// <param name="obj">要序列化二進制檔案的物件</param>
        /// <param name="path">存放路徑</param>
        public static void GZipSerializeBinary(object obj, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (GZipStream zip = new GZipStream(fileStream, CompressionMode.Compress))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(zip, obj);
                }
            }
        }
        /// <summary>
        /// 反序列化檔案 (壓縮)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">存放路徑</param>
        /// <returns>找不到檔案回傳 Null</returns>
        public static T GZipDeserialize<T>(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    using (GZipStream zip = new GZipStream(fileStream, CompressionMode.Compress))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        return (T)binaryFormatter.Deserialize(zip);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("找不到檔案路徑", "錯誤", MessageBoxButton.OK);
                return default(T);
            }
        }
        /// <summary>
        /// 序列化二進制檔案
        /// </summary>
        /// <param name="obj">要序列化二進制檔案的物件</param>
        /// <param name="path">存放路徑</param>
        public static void SerializeBinary(object obj, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, obj);
            }
        }
        /// <summary>
        /// 反序列化檔案
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">存放路徑</param>
        /// <returns>找不到檔案回傳 Null</returns>
        public static T Deserialize<T>(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(fileStream);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("找不到檔案路徑", "錯誤", MessageBoxButton.OK);
                return default(T);
            }
        }
    }

    /// <summary>
    /// 具有序列化與反序列化功能的泛行物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable()]
    [DataContract]
    public class SerializationHelper<T>
    {
        /// <summary>
        /// 序列化二進制檔案
        /// </summary>
        /// <param name="path">檔案存放路徑</param>
        public void SerializeBinary(string path)
        {
            SerializationHelper.SerializeBinary(this, path);
        }

        /// <summary>
        /// 反序列化檔案
        /// </summary>
        /// <param name="path">要反序列化的物件路徑</param>
        /// <returns>找不到檔案回傳 Null</returns>
        public T Deserialize(string path)
        {
            try
            {
                return SerializationHelper.Deserialize<T>(path);
            }
            catch (Exception)
            {
                MessageBox.Show("找不到檔案路徑", "錯誤", MessageBoxButton.OK);
                return default(T);
            }
        }

    }
}
