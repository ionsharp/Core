using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="IDisposable"/>.
    /// </summary>
    public abstract class DisposableObject : ObjectBase, IDisposable
    {
        bool disposed = false;

        /// <summary>
        /// 
        /// </summary>
        public DisposableObject()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                OnManagedDisposed();

            OnUnmanagedDisposed();

            disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnManagedDisposed()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnUnmanagedDisposed()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}