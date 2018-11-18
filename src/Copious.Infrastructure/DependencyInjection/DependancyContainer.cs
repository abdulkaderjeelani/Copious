using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Infrastructure.DependencyInjection {
    public class DependancyContainer : IDisposable {
        bool disposed = false;

        protected IDisposable Container { get; set; }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing) {
            if (!disposed) {
                if (disposing)
                    // release managed resources
                    Container.Dispose ();

                // release unmanaged resources if any
            }
            disposed = true;
        }
    }
}