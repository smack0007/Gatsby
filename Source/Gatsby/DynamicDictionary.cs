using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class DynamicDictionary : DynamicObject
    {
        IDictionary<string, object> data;

		public dynamic this[string index]
		{
			get
			{
				if (!this.data.ContainsKey(index))
					return null;

				return this.data[index];
			}

			set { this.data[index] = value; }
		}

        public DynamicDictionary()
            : base()
        {
            this.data = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
			result = null;

			if (this.data.ContainsKey(binder.Name))
				result = this.data[binder.Name];

			return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.data[binder.Name] = value;
            return true;
        }
    }
}
