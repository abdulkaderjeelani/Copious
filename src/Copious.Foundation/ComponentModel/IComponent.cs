namespace Copious.Foundation.ComponentModel
{
    using System;

   
    public interface IComponent  :  IComponent<Guid>
    {        
        
    }

    public interface IComponent<TKey> : Identifiable<TKey>
    {        
        int Version { get; set; }
        string CompName { get; }
    }
}