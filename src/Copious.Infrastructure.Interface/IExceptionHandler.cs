using System;
using System.Threading.Tasks;

namespace Copious.Infrastructure.Interface {
    public interface IExceptionHandler {
        void HandleException (Exception ex);
    }

    public static class ExceptionHandlerExtensions {
        public static async Task AttachAndRun (this IExceptionHandler exceptionHandler, Func<Task> task) {
            try {
                await task?.Invoke ();
            } catch (Exception ex) {
                exceptionHandler.HandleException (ex);
            }

        }
    }
}