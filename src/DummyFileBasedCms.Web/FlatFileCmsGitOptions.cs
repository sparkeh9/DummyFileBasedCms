using System.Collections.Generic;

namespace DummyFileBasedCms.Web
{
    public class FlatFileCmsGitOptions
    {
        public List<Directory> Directories { get; set; } = new List<Directory>();

        public class Directory
        {
            public string FilePath { get; set; }
            public string RepositoryUrl { get; set; }
            public string Branch { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
            public string Passphrase { get; set; }
        }
    }
}