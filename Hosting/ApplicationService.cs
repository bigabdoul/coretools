using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CoreTools.Hosting
{
    /// <summary>
    /// Base class for implementing a long running <see cref="IHostedService"/>.
    /// </summary>
    public abstract class ApplicationService : IHostedService, IDisposable
    {
        #region fields

        private Task _executingTask;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _tokenLock = new object();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService"/> class.
        /// </summary>
        protected ApplicationService()
        {
        }

        #region properties

        /// <summary>
        /// Gets or sets the cancellation token source.
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource
        {
            get
            {
                if (_cancellationTokenSource == null)
                {
                    lock (_tokenLock)
                    {
                        if (_cancellationTokenSource == null)
                            _cancellationTokenSource = new CancellationTokenSource();
                    }
                }
                return _cancellationTokenSource;
            }
            set => _cancellationTokenSource = value;
        }

        /// <summary>
        /// Gets or sets the service's start time.
        /// </summary>
        public DateTime? Started { get; protected set; }

        /// <summary>
        /// Gets or sets the time the service was stopped.
        /// </summary>
        public DateTime? Finished { get; protected set; } 

        #endregion

        #region IHostedService

        /// <summary>
        /// Asynchronously starts the background service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            Finished = null;
            Started = DateTime.UtcNow;

            // Create a linked token so that the task can be canceled both from the service and outside of this method
            var cts = CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing
            _executingTask = ExecuteAsync(cts.Token);

            // If the task is completed then return it, otherwise it's running
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously stops the background service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));

            if (Finished == null)
            {
                Finished = DateTime.UtcNow;
            }

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        } 
        #endregion

        /// <summary>
        /// Communicates a request for cancellation and catches the <see cref="AggregateException"/>.
        /// </summary>
        public virtual void Cancel()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    _cancellationTokenSource.Cancel();
                }
                catch (AggregateException)
                {
                }
            }
        }

        /// <summary>
        /// Communicates a request for cancellation and releases resources.
        /// </summary>
        public virtual void Dispose()
        {
            Cancel();
            _cancellationTokenSource?.Dispose();
        }
        
        /// <summary>
        /// Creates a <see cref="System.Threading.CancellationTokenSource"/> that will be in the canceled state when any 
        /// of the source tokens (<see cref="CancellationTokenSource.Token"/>, and <paramref name="cancellationToken"/>)
        /// are in the canceled state.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A <see cref="System.Threading.CancellationTokenSource"/> that is linked to the source tokens.</returns>
        protected CancellationTokenSource CreateLinkedTokenSource(CancellationToken cancellationToken)
            => CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token, cancellationToken);

        /// <summary>
        /// Asynchronously execute a long running task.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns></returns>
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
