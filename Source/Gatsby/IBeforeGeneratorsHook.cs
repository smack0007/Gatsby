using System;

namespace Gatsby
{
    public interface IBeforeGeneratorsHook
    {
        void BeforeGenerators(Site site);
    }
}
