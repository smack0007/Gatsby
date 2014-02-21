using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class DynamicDictionary : DynamicObject, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>
    {
        IDictionary<string, object> data;

        public ICollection<string> Keys
        {
            get { return this.data.Keys; }
        }

        public ICollection<object> Values
        {
            get { return this.data.Values; }
        }

        public int Count
        {
            get { return this.data.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.data.IsReadOnly; }
        }

        public object this[string key]
        {
            get { return this.data[key]; }
            set { this.data[key] = value; }
        }

        public DynamicDictionary()
            : base()
        {
            this.data = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.data.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.data[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.data.Keys;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(string key, object value)
        {
            this.data.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this.data.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.data.TryGetValue(key, out value);
        }
                                
        public void Add(KeyValuePair<string, object> item)
        {
            this.data.Add(item);
        }

        public void Clear()
        {
            this.data.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.data.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.data.Remove(item);
        }
    }
}
