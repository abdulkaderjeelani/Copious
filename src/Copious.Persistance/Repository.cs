using Copious.Persistance.Interface;

namespace Copious.Persistance {
    /// <summary>
    /// Root of our repoistory Hierarchy
    /// Any common functions between different kinds of repository (EF/NOSQL)
    /// Should go here,
    /// </summary>
    public class Repository : IRepository { }
}