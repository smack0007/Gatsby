using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class PluginManager
    {
        Dictionary<Type, object> instances;

        List<Type> pluginTypes;
        List<Type> beforeGeneratorsHooks;
		List<Type> afterGeneratorsHooks;

        public PluginManager()
        {
            this.instances = new Dictionary<Type, object>();
            this.pluginTypes = new List<Type>();
            this.beforeGeneratorsHooks = new List<Type>();
			this.afterGeneratorsHooks = new List<Type>();
        }

        internal void Register(Type type)
        {
            this.pluginTypes.Add(type);

            foreach (Type @interface in type.GetInterfaces())
            {
				if (@interface == typeof(IBeforeGeneratorsHook))
				{
					this.beforeGeneratorsHooks.Add(type);
				}
				else if (@interface == typeof(IAfterGeneratorsHook))
				{
					this.afterGeneratorsHooks.Add(type);
				}
            }
        }

        private object GetInstance(Type type)
        {
            if (!this.instances.ContainsKey(type))
            {
                this.instances[type] = Activator.CreateInstance(type);
            }

            return this.instances[type];
        }

        internal void BeforeGenerators(Site site)
        {
            foreach (Type type in this.beforeGeneratorsHooks)
            {
                IBeforeGeneratorsHook hook = (IBeforeGeneratorsHook)this.GetInstance(type);
                hook.BeforeGenerators(site);
            }
        }

		internal void AfterGenerators(Site site)
		{
			foreach (Type type in this.afterGeneratorsHooks)
			{
				IAfterGeneratorsHook hook = (IAfterGeneratorsHook)this.GetInstance(type);
				hook.AfterGenerators(site);
			}
		}

        public T Get<T>()
        {
            Type type = typeof(T);

            if (!this.pluginTypes.Contains(type))
                throw new GatsbyException(string.Format("No plugin of type \"{0}\" available.", type));

            return (T)this.GetInstance(type);
        }
    }
}
