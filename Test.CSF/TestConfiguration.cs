using System;
using System.Configuration;

namespace Test.CSF
{
  /// <summary>
  /// Configuration related to the testing environment itself.
  /// </summary>
  public class TestConfiguration : ConfigurationSection
  {
    #region properties

    /// <summary>
    /// Gets or sets a filesystem path to serve as the root for tests which require access to the filesystem.
    /// </summary>
    /// <value>The filesystem testing root path.</value>
    [ConfigurationProperty(@"FilesystemTestingRootPath", IsRequired = true)]
    public virtual string FilesystemTestingRootPath
    {
      get {
        return (string) this["FilesystemTestingRootPath"];
      }
      set {
        this["FilesystemTestingRootPath"] = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the root path for filesystem testing.
    /// </summary>
    /// <returns>The filesystem testing root path.</returns>
    public System.IO.DirectoryInfo GetFilesystemTestingRootPath()
    {
      return new System.IO.DirectoryInfo(this.FilesystemTestingRootPath);
    }

    #endregion
  }
}

