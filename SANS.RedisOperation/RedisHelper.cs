using SANS.Config;
using SANS.Log;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SANS.RedisOperation
{
    public class RedisHelper
    {
        //单例模式
        public static RedisCommon Default { get { return new RedisCommon(); } }
        public static RedisCommon One { get { return new RedisCommon(1, "127.0.0.1:6379"); } }
        public static RedisCommon Two { get { return new RedisCommon(2, "127.0.0.1:6379"); } }
    }
    ///
    /// Redis操作类
    /// Net Core使用StackExchange.Redis的nuget包
    ///
    public class RedisCommon
    {
        //redis数据库连接字符串
        private string _conn = SiteConfig.AppSetting("Redis", "Url") ?? "127.0.0.1:6379";
        private int _db = int.Parse(SiteConfig.AppSetting("Redis", "db"));
        private string CustomKey;
        //静态变量 保证各模块使用的是不同实例的相同链接
        private static ConnectionMultiplexer connection;
        public RedisCommon() { }
        ///
        /// 构造函数
        ///
        /// 
        /// 
        public RedisCommon(int db, string connectStr)
        {
            _conn = connectStr;
            _db = db;
        }
        ///

        /// 缓存数据库，数据库连接
        ///
        public ConnectionMultiplexer CacheConnection
        {
            get
            {
                try
                {
                    if (connection == null || !connection.IsConnected)
                    {
                        connection = ConnectionMultiplexer.Connect(_conn);
                    }
                }
                catch (Exception ex)
                {
                    //Log.LogError("RedisHelper->CacheConnection 出错\r\n" + ex.ToString());
                    return null;
                }
                return connection;
            }
        }
        ///
        /// 缓存数据库
        ///
        public IDatabase CacheRedis => CacheConnection.GetDatabase(_db);
        public IServer RedisServer => CacheConnection.GetServer(CacheConnection.GetEndPoints()[0]);
        #region --KEY/VALUE存取--
        /// <summary>
        /// 单条存值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool StringSet(string key, string value)
        {
            return CacheRedis.StringSet(key, value);
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return CacheRedis.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="arr">key</param>
        /// <returns></returns>
        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] arr)
        {
            return CacheRedis.StringSet(arr);
        }
        /// <summary>
        /// 批量存值
        /// </summary>
        /// <param name="keysStr">key</param>
        /// <param name="valuesStr">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool StringSetMany(string[] keysStr, string[] valuesStr)
        {
            var count = keysStr.Length;
            var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[count];
            for (int i = 0; i < count; i++)
            {
                keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keysStr[i], valuesStr[i]);
            }

            return CacheRedis.StringSet(keyValuePair);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = JsonConvert.SerializeObject(obj);
            return CacheRedis.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 追加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void StringAppend(string key, string value)
        {
            ////追加值，返回追加后长度
            long appendlong = CacheRedis.StringAppend(key, value);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public RedisValue GetStringKey(string key)
        {
            return CacheRedis.StringGet(key);
        }
        /// <summary>
        /// 根据Key获取值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>System.String.</returns>
        public string StringGet(string key)
        {
            try
            {
                return CacheRedis.StringGet(key);
            }
            catch (Exception ex)
            {
                //Log.LogError("RedisHelper->StringGet 出错\r\n" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public RedisValue[] GetStringKey(List<RedisKey> listKey)
        {
            return CacheRedis.StringGet(listKey.ToArray());
        }
        /// <summary>
        /// 批量获取值
        /// </summary>
        public string[] StringGetMany(string[] keyStrs)
        {
            var count = keyStrs.Length;
            var keys = new RedisKey[count];
            var addrs = new string[count];

            for (var i = 0; i < count; i++)
            {
                keys[i] = keyStrs[i];
            }
            try
            {

                var values = CacheRedis.StringGet(keys);
                for (var i = 0; i < values.Length; i++)
                {
                    addrs[i] = values[i];
                }
                return addrs;
            }
            catch (Exception ex)
            {
                //Log.LogError("RedisHelper->StringGetMany 出错\r\n" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetStringKey<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(CacheRedis.StringGet(key));
        }
        public List<T> GetStringListKey<T>(string key)
        {
            List<T> addrs = new List<T>();
            try
            {
                var redisResult = CacheRedis.ScriptEvaluate(LuaScript.Prepare(
                    //Redis的keys模糊查询：
                    " local res = redis.call('KEYS', @keypattern) " +
                    " return res "), new { @keypattern = key });
                if (!redisResult.IsNull)
                {
                    var values = CacheRedis.StringGet((RedisKey[])redisResult);
                    for (var i = 0; i < values.Length; i++)
                    {
                        addrs.Add(JsonConvert.DeserializeObject<T>(values[i]));
                    }
                }
            }
            catch (Exception er)
            {
                Log4netHelper.Error(this, er);
            }
            return addrs;
        }
        public List<T> GetStringListScan<T>(string key)
        {
            List<T> addrs = new List<T>();
            var redisResult = RedisServer.Keys(_db, key, 10000);
            if (redisResult != null)
            {
                foreach (var k in redisResult)
                {
                    addrs.Add(GetStringKey<T>(k));
                }
            }
            return addrs;
        }
        #endregion

        #region SortedSet
        public double SortedSetObject<T>(string key, T obj, double sort)
        {
            string json = JsonConvert.SerializeObject(obj);
            return CacheRedis.SortedSetIncrement(key, json, sort);
        }
        public long SortedSetCountKey(string key)
        {
            return CacheRedis.SortedSetLength(key);
        }
        public List<T> SortedGetRangeByRankObject<T>(string key, int start, int stop = -1)
        {
            List<T> addrs = new List<T>();
            try
            {
                var ls = CacheRedis.SortedSetRangeByScoreAsync(key, start, stop);
                if (ls != null && ls.Result.Length > 0)
                {
                    for (var i = 0; i < ls.Result.Length; i++)
                    {
                        addrs.Add(JsonConvert.DeserializeObject<T>(ls.Result[i]));
                    }
                }
            }
            catch (Exception er)
            {
                Log4netHelper.Error(this, er);
            }
            return addrs;
        }
        public List<T> SortedGetRangeByRankObjectTask<T>(string key, int pageSize)
        {
            List<T> addrs = new List<T>();
            try
            {
                var count = CacheRedis.SortedSetLength(key);
                var pageCount = count / pageSize;
                if (count % pageSize > 0)
                {
                    pageCount += 1;
                }
                List<RedisValue> redisResults = new List<RedisValue>();
                var batch = CacheRedis.CreateBatch();
                var tasks = new List<Task<RedisValue[]>>();
                for (var i = 0; i < pageCount; i++)
                {
                    tasks.Add(CacheRedis.SortedSetRangeByScoreAsync(key, i * pageSize, (i + 1) * pageSize));
                }
                batch.Execute();
                Task.WhenAll(tasks.ToArray());
                foreach (var v in tasks)
                {
                    if (v != null && v.Result.Length > 0)
                    {
                        redisResults.AddRange(v.Result);
                    }
                }
                for (var i = 0; i < redisResults.Count; i++)
                {
                    addrs.Add(JsonConvert.DeserializeObject<T>(redisResults[i]));
                }
            }
            catch (Exception er)
            {
                Log4netHelper.Error(this, er);
            }
            return addrs;
        }
        public List<T> SortedGetRangeByScoreObjectTask<T>(string key, int pageSize, double start, double stop)
        {
            List<T> addrs = new List<T>();
            var count = CacheRedis.SortedSetLength(key);
            var pageCount = count / pageSize;
            if (count % pageSize > 0)
            {
                pageCount += 1;
            }
            List<RedisValue> redisResults = new List<RedisValue>();
            var batch = CacheRedis.CreateBatch();
            var tasks = new List<Task<RedisValue[]>>();
            for (var i = 0; i < pageCount; i++)
            {
                tasks.Add(CacheRedis.SortedSetRangeByScoreAsync(key, start, stop, skip: i * pageSize, take: (i + 1) * pageSize));
            }
            batch.Execute();
            Task.WhenAll(tasks.ToArray());
            foreach (var v in tasks)
            {
                if (v != null && v.Result.Length > 0)
                {
                    redisResults.AddRange(v.Result);
                }
            }
            if (redisResults != null && redisResults.Count > 0)
            {
                for (var i = 0; i < redisResults.Count; i++)
                {
                    addrs.Add(JsonConvert.DeserializeObject<T>(redisResults[i]));
                }
            }
            return addrs;
        }
        public List<T> SortedGetRangeByScoreObject<T>(string key, double start, double stop)
        {
            List<T> addrs = new List<T>();
            var ls = CacheRedis.SortedSetRangeByScore(key, start, stop);
            if (ls != null && ls.Length > 0)
            {
                for (var i = 0; i < ls.Length; i++)
                {
                    addrs.Add(JsonConvert.DeserializeObject<T>(ls[i]));
                }
            }
            return addrs;
        }
        public void SortedSetRemove(string key, string value)
        {
            //string json = JsonConvert.SerializeObject(value);
            //var score = CacheRedis.SortedSetScore(key, json);
            //var a = CacheRedis.SortedSetRemove(key, json, CommandFlags.None);
            //var items = CacheRedis.SortedSetRangeByRank(key, 0, -1);
            //var b = CacheRedis.SortedSetRemoveRangeByRank(key, 0, 0);
            //var items1 = CacheRedis.SortedSetRangeByRank(key, 0, -1);
            //CacheRedis.KeyDelete(key);

        }
        #endregion

        #region --删除设置过期--
        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            return CacheRedis.KeyDelete(key);
        }
        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(RedisKey[] keys)
        {
            return CacheRedis.KeyDelete(keys);
        }
        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return CacheRedis.KeyExists(key);
        }
        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            return CacheRedis.KeyRename(key, newKey);
        }
        /// <summary>
        /// 删除hasekey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool HaseDelete(RedisKey key, RedisValue hashField)
        {
            return CacheRedis.HashDelete(key, hashField);
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashRemove(string key, string dataKey)
        {
            return CacheRedis.HashDelete(key, dataKey);
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public void SetExpire(string key, DateTime datetime)
        {
            CacheRedis.KeyExpire(key, datetime);
        }
        #endregion

        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomKey ?? "KN";
            return prefixKey + oldKey;
        }

        #region 发布订阅
        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            string json = JsonConvert.SerializeObject(msg);
            return CacheRedis.Publish(channel, json);
        }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var subscriber = CacheConnection.GetSubscriber();
            subscriber.Subscribe(subChannel, (chanel, message) =>
            {
                if (handler != null)
                {
                    handler(chanel, message);
                }
            });
        }
        /// <summary>
        /// 取消指定频道的订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            var sub = CacheConnection.GetSubscriber();
            sub.Unsubscribe(channel);
        }
        /// <summary>
        /// 取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            var sub = CacheConnection.GetSubscriber();
            sub.UnsubscribeAll();
        }
        #endregion


        #region Hash
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            return CacheRedis.HashExists(key, dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T dataValue)
        {
            var json = ConvertJson(dataValue);
            return CacheRedis.HashSet(key, dataKey, json);
        }
        /// <summary>
        /// 批量新增到hash表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public void HashSet(string key, IEnumerable<HashEntry> list)
        {
            CacheRedis.HashSet(key, list.ToArray());
        }
        /// <summary>
        /// 存储对象到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void HashSetFromObject<T>(string key, T obj)
        {
            var hashEntrys = from p in obj.GetType().GetProperties()//获取对象所有属性
                             let name = p.Name
                             let value = ConvertJson(p.GetValue(obj))
                             select new HashEntry(name, value);
            HashSet(key, hashEntrys);
        }

        /// <summary>
        /// 存储列表到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <param name="getDataKey">这个方法用于根据item定义dataKey名称</param>
        public void HashSetFromList<T>(string key, List<T> list, Func<T, string> getDataKey)
        {
            var hashEntrys = from item in list
                             let name = getDataKey(item)
                             let value = ConvertJson(item)
                             select new HashEntry(name, value);
            HashSet(key, hashEntrys);
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
            string dataValue = CacheRedis.HashGet(key, dataKey);
            return ConvertObj<T>(dataValue);
        }
        /// <summary>
        /// 获取字符串类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public string HashGet(string key, string dataKey)
        {
            string dataValue = CacheRedis.HashGet(key, dataKey);
            return dataValue;
        }

        /// <summary>
        /// 从hash表获取指定多个dataKey的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="listDataKey"></param>
        /// <returns></returns>
        public List<T> HashGet<T>(string key, List<string> listDataKey)
        {
            var dataKeys = ConvertRedisValues(listDataKey);
            var dataValues = CacheRedis.HashGet(key, dataKeys);
            return (from redisValue in dataValues where redisValue.HasValue && !redisValue.IsNullOrEmpty select ConvertObj<T>(redisValue)).ToList();
        }

        /// <summary>
        /// 从hash表获取指定多个dataKey的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="listDataKey"></param>
        /// <returns></returns>
        public List<T> HashGet<T>(string key, List<RedisValue> listDataKey)
        {
            var dataKeys = listDataKey.ToArray();
            var dataValues = CacheRedis.HashGet(key, dataKeys);
            return ConvetList<T>(dataValues);
        }

        /// <summary>
        /// 移除hash表的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            return CacheRedis.HashDelete(key, dataKey);
        }

        /// <summary>
        /// 移除hash表的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listDataKey"></param>
        /// <returns></returns>
        public long HashDelete(string key, List<string> listDataKey)
        {
            var dataKeys = ConvertRedisValues(listDataKey);
            return CacheRedis.HashDelete(key, dataKeys);
        }

        /// <summary>
        /// 移除hash表的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listDataKey"></param>
        /// <returns></returns>
        public long HashDelete(string key, List<RedisValue> listDataKey)
        {
            var dataKeys = listDataKey.ToArray();
            return CacheRedis.HashDelete(key, dataKeys);
        }

        /// <summary>
        /// 获取hash表的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HashLength(string key)
        {
            return CacheRedis.HashLength(key);
        }

        /// <summary>
        /// 获取hash表所有dataKey所有dataValue,以HashEntry[]形式返回
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashEntry[] HashAll(string key)
        {
            return CacheRedis.HashGetAll(key);
        }

        /// <summary>
        /// 获取hash表所有dataValue
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashListAll<T>(string key)
        {
            var hashEntrys = CacheRedis.HashGetAll(key);
            return (from item in hashEntrys where !item.Value.IsNullOrEmpty select ConvertObj<T>(item.Value)).ToList();
        }

        /// <summary>
        /// 获取hash表所有dataKey所有dataValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> HashDictAll<T>(string key)
        {
            var hashEntrys = CacheRedis.HashGetAll(key);
            var dists = hashEntrys
                .Where(item => !item.Value.IsNullOrEmpty)
                .ToDictionary(item => item.Name.ToString(), item => ConvertObj<T>(item.Value));
            return dists;
        }

        /// <summary>
        /// 获取hash表所有dataKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> HashKeys(string key)
        {
            var keys = CacheRedis.HashKeys(key);
            return ConvetList<string>(keys);
        }

        /// <summary>
        /// 获取hash表所有dataValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashValues<T>(string key)
        {
            var values = CacheRedis.HashValues(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1)
        {
            return CacheRedis.HashIncrement(key, dataKey, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1)
        {
            return CacheRedis.HashDecrement(key, dataKey, val);
        }
        private static string ConvertJson<T>(T value)
        {
            var result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }
        private static T ConvertObj<T>(RedisValue value)
        {
            var result = value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default(T);
            return result;
        }

        private static List<T> ConvetList<T>(IEnumerable<RedisValue> values)
        {
            var result = values.Select(ConvertObj<T>).ToList();
            return result;
        }
        private static RedisKey[] ConvertRedisKeys(IEnumerable<string> redisKeys)
        {
            var result = redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
            return result;
        }
        private static RedisValue[] ConvertRedisValues(IEnumerable<string> redisValues)
        {
            var result = redisValues.Select(redisValue => (RedisValue)redisValue).ToArray();
            return result;
        }

        #endregion

        #region List

        #region Queue
        /// <summary>
        /// 入队(推进)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value)
        {
            CacheRedis.ListRightPush(key, ConvertJson(value));
        }

        /// <summary>
        /// 出队(弹出)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>从队列中拉出最先进队列的数据值,先进先出原则</returns>
        public T ListRightPop<T>(string key)
        {
            var value = CacheRedis.ListRightPop(key);
            return ConvertObj<T>(value);
        }

        #endregion Queue

        #region Stack

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListLeftPush<T>(string key, T value)
        {
            CacheRedis.ListLeftPush(key, ConvertJson(value));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>从栈中拉出最后进栈的数据值,后进先出原则</returns>
        public T ListLeftPop<T>(string key)
        {
            var value = CacheRedis.ListLeftPop(key);
            return ConvertObj<T>(value);
        }

        #endregion Stack

        /// <summary>
        /// 移除list的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public long ListRemove<T>(string key, T value)
        {
            return CacheRedis.ListRemove(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取list的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return CacheRedis.ListLength(key);
        }

        /// <summary>
        /// 获取list的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key)
        {
            var values = CacheRedis.ListRange(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取list的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key, int start, int count)
        {
            var values = CacheRedis.ListRange(key, start, start + count - 1);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取list的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<T> ListPager<T>(string key, int pageIndex, int pageSize)
        {
            var start = pageSize * (pageIndex - 1);
            return ListRange<T>(key, start, pageSize);
        }
        #endregion
    }

}
