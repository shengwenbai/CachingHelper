﻿using System;
using System.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace WebApiDemo.Caching
{
    public class RedisCache: ICache
    {
        private string address;
        readonly JsonSerializerSettings jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        ConnectionMultiplexer connectionMultiplexer;
        IDatabase database;
        class CacheObject<T>
        {
            public int ExpireTime { get; set; }
            public bool ForceOutofDate { get; set; }
            public T Value { get; set; }
        }
        public RedisCache()
        {
            this.address = ConfigurationManager.AppSettings["RedisServer"];
            if (this.address == null || string.IsNullOrWhiteSpace(this.address))
                throw new ApplicationException("配置文件中未找到RedisServer的有效配置");
            connectionMultiplexer = ConnectionMultiplexer.Connect(address);
            database = connectionMultiplexer.GetDatabase();
        }

        public int TimeOut { get; set; } = 600;

        public object Get(string key)
        {
            return Get<object>(key);
        }

        public T Get<T>(string key)
        {
            var cacheValue = database.StringGet(key);
            var value = default(T);
            if (!cacheValue.IsNull)
            {
                var cacheObject = JsonConvert.DeserializeObject<CacheObject<T>>(cacheValue, jsonConfig);
                if (!cacheObject.ForceOutofDate)
                    database.KeyExpire(key, new TimeSpan(0, 0, cacheObject.ExpireTime));
                value = cacheObject.Value;
            }
            DateTime endJson = DateTime.Now;
            return value;
        }

        public void Remove(string key)
        {
            database.KeyDelete(key, CommandFlags.HighPriority);
        }

        public void Clear()
        {
            
            throw new NotImplementedException();
        }

        public void Insert(string key, object data)
        {
            var jsonData = GetJsonData(data, TimeOut, false);
            database.StringSet(key, jsonData);
        }

        public void Insert<T>(string key, T data)
        {
            var jsonData = GetJsonData<T>(data, TimeOut, false);
            database.StringSet(key, jsonData);
        }

        public void Insert(string key, object data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = GetJsonData(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = GetJsonData<T>(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public void Insert(string key, object data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = GetJsonData(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public void Insert<T>(string key, T data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = GetJsonData<T>(data, TimeOut, true);
            database.StringSet(key, jsonData, timeSpan);
        }

        public bool Exists(string key)
        {
            return database.KeyExists(key);
        }

        string GetJsonData(object data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<object>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return JsonConvert.SerializeObject(cacheObject, jsonConfig);//序列化对象
        }
        string GetJsonData<T>(T data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<T>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return JsonConvert.SerializeObject(cacheObject, jsonConfig);//序列化对象
        }
    }
}