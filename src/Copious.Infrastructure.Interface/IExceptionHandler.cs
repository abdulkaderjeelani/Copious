using System;

namespace Copious.Infrastructure.Interface
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}