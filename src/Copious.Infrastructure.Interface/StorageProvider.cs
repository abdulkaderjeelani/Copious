using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Infrastructure.Interface {
    public enum StorageProvider {
        FileSystem,
        Azure,
        Amazon,
        Google,
        Rethink
    }
}