namespace Copious.Foundation.ComponentModel
{
    using System;

    public interface IComponent
    {
        Guid Id { get; set; }
        int Version { get; set; }
        string CompName { get; }
    }
}