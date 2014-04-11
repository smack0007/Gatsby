using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
	public class DynamicValue : DynamicObject
	{
		object value;

		public DynamicValue(object value)
		{
			if (value is DynamicValue)
				value = ((DynamicValue)value).value;

			this.value = value;
		}

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			if (binder.Type.IsAssignableFrom(this.value.GetType()))
			{
				result = this.value;
				return true;
			}

			return base.TryConvert(binder, out result);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var property = this.value.GetType().GetProperties().SingleOrDefault(x => x.Name == binder.Name);

			if (property == null)
				throw new GatsbyException(string.Format("Property {0} does not exist on object type {1}.", binder.Name, this.value.GetType()));

			result = property.GetValue(this.value);

			return true;
		}
	}
}
