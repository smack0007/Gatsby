using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentGlue;

namespace Gatsby
{
    public class Program
    {
        public Program()
        {
        }

        public int Run(string[] args)
        {
            return 0;
        }

        public static int Main(string[] args)
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IProcessorPathFilter>().To<ProcessorPathFilter>().AsTransient();

            var program = container.Resolve<Program>();
            return program.Run(args);
        }
    }
}
