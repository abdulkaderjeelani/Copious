using System;

namespace Copious.Infrastructure.Interface {
    public static class Aspects {
        public static Action AddExceptionHandlingAspect (this Action action, IExceptionHandler exHandler) => () => {
            try {
                action?.Invoke ();
            } catch (Exception ex) {
                exHandler.HandleException (ex);
            }
        };

        public static Action AddRetryAspect (this Action action, int tries) => () => {
            var tried = 0;
            do {
                try {
                    tried++;
                    action?.Invoke ();
                    break;
                } catch (Exception) {
                    if (tried == tries)
                        throw;
                }
            } while (true);
        };
    }
}