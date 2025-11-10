using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace BaroJunk
{
  public partial class ConfigCore : IConfigLikeContainer, IDirectlyLocatable, IReactiveLocatable
  {
    static ConfigCore() => ProjectInfo.Add(new PackageInfo()
    {
      Name = "Config",
      Version = new Version(0, 0, 0) { Branch = "BaroJunk", },

      Dependencies = new List<PackageInfo>(){
        new PackageInfo(){
          Name = "Logger",
          Version = new Version(0, 0, 0){ Branch = "BaroJunk", },
        },
        new PackageInfo(){
          Name = "SimpleParser",
          Version = new Version(0, 0, 0){ Branch = "BaroJunk", },
        },
        new PackageInfo(){
          Name = "SimpleResult",
          Version = new Version(0, 0, 0){ Branch = "BaroJunk", },
        }
      }
    });
  }
}