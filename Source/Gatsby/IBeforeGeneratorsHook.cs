﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public interface IBeforeGeneratorsHook
    {
        void BeforeGenerators(Site site);
    }
}