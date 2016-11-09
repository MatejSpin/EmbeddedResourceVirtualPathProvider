using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace EmbeddedResourceVirtualPathProvider
{
    public class EmbeddedResourceVirtualDirectory : VirtualDirectory
    {
        List<EmbeddedResourceVirtualFile> files;
        List<EmbeddedResourceVirtualDirectory> directories;
        List<VirtualFileBase> children;
        public EmbeddedResourceVirtualDirectory(string virtualPath, IDictionary<string, List<EmbeddedResource>> resources, Func<EmbeddedResource, EmbeddedResourceCacheControl> cacheControl) : base(virtualPath)
        {
            files = new List<EmbeddedResourceVirtualFile>();
            directories = new List<EmbeddedResourceVirtualDirectory>();

            var tmpVirtualPath = virtualPath.Replace("~/", "").Replace("/", ".").ToUpper();

            var validResourcePaths = resources.Where(x => x.Key.StartsWith(tmpVirtualPath));

            var filePaths = validResourcePaths.Where(x => x.Key.Replace(tmpVirtualPath, "").Count(y => y == '.') == 1);
            foreach (var item in filePaths)
            {
                foreach (var value in item.Value)
                {
                    var lastDotPosition = item.Key.LastIndexOf(".");
                    var tmpPath = "~/"+item.Key.Substring(0, lastDotPosition).Replace(".", "/") + item.Key.Substring(lastDotPosition);
                    files.Add(new EmbeddedResourceVirtualFile(tmpPath, value, cacheControl(value)));
                }
            }


            var dirPaths = validResourcePaths.Where(x => x.Key.Replace(tmpVirtualPath, "").Count(y => y == '.') > 1);
            foreach (var validPath in dirPaths)
            {
                var lastDotPosition = validPath.Key.LastIndexOf(".");
                var tmpPath = "~/" + validPath.Key.Substring(0, lastDotPosition).Replace(".", "/") + validPath.Key.Substring(lastDotPosition);
                var lastSlashPosition = tmpPath.LastIndexOf("/");
                directories.Add(new EmbeddedResourceVirtualDirectory(tmpPath.Replace(tmpVirtualPath, "").Remove(lastSlashPosition+1), resources, cacheControl));
            }

            children = new List<VirtualFileBase>();
            children.AddRange(files);
            children.AddRange(directories);
        }

        public override IEnumerable Children
        {
            get
            {
                return children;
            }
        }

        public override IEnumerable Directories
        {
            get
            {
                return directories;
            }
        }

        public override IEnumerable Files
        {
            get
            {
                return files;
            }
        }

    }


}
