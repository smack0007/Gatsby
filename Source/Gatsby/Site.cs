using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby
{
    public class Site
    {
		Config config;

        public PluginManager Plugins
        {
            get;
            private set;
        }

        public string BaseUrl
        {
			get { return this.config.BaseUrl; }
        }

		public string Destination
		{
			get { return this.config.Destination; }
		}

        public List<Post> Posts
        {
            get;
            private set;
        }

        public IEnumerable<string> Categories
        {
            get { return this.Posts.Select(x => x.Category).Distinct().OrderBy(x => x); }
        }
                
        public IEnumerable<string> Tags
        {
            get { return this.Posts.SelectMany(x => x.Tags).Distinct().OrderBy(x => x); }
        }

        public List<Page> Pages
        {
            get;
            private set;
        }

		public IEnumerable<SiteContent> Content
		{
			get { return this.Posts.Cast<SiteContent>().Concat(this.Pages.Cast<SiteContent>().Concat(this.GeneratorPages.Cast<SiteContent>())); }
		}

        public List<Generator> GeneratorPages
        {
            get;
            private set;
        }

        internal Site(Config config)
        {
			this.config = config;

            this.Plugins = new PluginManager();
            this.Posts = new List<Post>();
            this.Pages = new List<Page>();
            this.GeneratorPages = new List<Generator>();
        }
    }
}
