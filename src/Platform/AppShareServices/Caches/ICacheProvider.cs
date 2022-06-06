using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Caches
{
    public interface ICacheProvider
    {
        void Set<T>(string key, T data) where T : class;
        void Set<T>(string key, T data, int expiredTimeInSeconds) where T : class;
        T Get<T>(string key) where T : class;
        T Get<T>(string key, Func<T> callback) where T : class;
        T Get<T>(string key, Func<T> callback, int expiredTimeInSeconds) where T : class;

        bool CheckExist(string key);
        void Remove(string key);
        void Remove(List<string> keys);
    }
}
