using System;

namespace Gatsby
{
	public interface IAfterGeneratorsHook
	{
		void AfterGenerators(Site site);
	}
}
