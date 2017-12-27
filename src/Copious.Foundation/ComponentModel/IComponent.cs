namespace Copious.Foundation.ComponentModel
{
    using System;

    public interface IUnique : Identifiable<Guid>
    {        
    }

    public interface IComponent  :  IUnique
    {        
        int Version { get; set; }
        string CompName { get; }
    }
}