using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.UI.Core;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// Executes a stream on actions on the dispatcher thread.
    /// </summary>
    internal class DispatcherQueueProcessor : DispatcherQueueProcessorBase
    {
        #region Properties

        /// <summary>
        /// This class is a singleton class. Below is the instance.
        /// </summary>
        static readonly Lazy<DispatcherQueueProcessor> _instance = new Lazy<DispatcherQueueProcessor>(() => new DispatcherQueueProcessor(), true);
        /// <summary>
        /// Gets the instance of the singleton class
        /// </summary>
        public static DispatcherQueueProcessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// The Application Dispatcher
        /// </summary>
        CoreDispatcher Dispatcher = null;

        #endregion

        #region DispatcherQueueProcessor

        /// <summary>
        /// Private constructor for this singleton class. Use the Instance property to
        /// get an instance of this class.
        /// </summary>
        protected DispatcherQueueProcessor() : base()
        {
            ActionQueue = new BlockingCollection<Action>();
            SubscriberQueue = new ConcurrentDictionary<WeakReference, object>();
            if (!CheckIfDispatcherCreated())
            {
                // The application domain hasn't been created yet, poll at 10Hz until it's created
                var timer = new System.Timers.Timer(100);
                timer.Elapsed += (sender, e) => {
                    if (CheckIfDispatcherCreated())
                    {
                        timer.Enabled = false;
                    }
                };
                timer.Enabled = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CheckDispatcherAccess()
        {
            return Dispatcher.CheckAccess();
        }

        /// <summary>
        /// Checks if the dispatcher has been created yet
        /// </summary>
        /// <returns></returns>
        protected override bool CheckIfDispatcherCreated()
        {
            if (Dispatcher != null)
            {
                return true;
            }
            else
            {
                lock (StartQueueLock)
                {
                    if (Dispatcher == null)
                    {
                        if (Application.Current != null)
                        {
                            Dispatcher = Application.Current.Dispatcher;
                            Dispatcher.ShutdownStarted += (s, e) => Dispatcher = null;
                            if (Dispatcher != null)
                                StartQueueProcessing();
                        }
                    }
                    return Dispatcher != null;
                }
            }
        }

        /// <summary>
        /// Starts the thread that processes the queue of actions that are to be
        /// executed on the dispatcher thread.
        /// </summary>
        protected override void StartQueueProcessing()
        {
            var Keys = SubscriberQueue.Keys;
            SubscriberQueue = null;

            foreach (var i in Keys)
            {
                var subscribe = i.Target as Action;
                subscribe?.Invoke();
            }
            Keys = null;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!ActionQueue.IsCompleted)
                    {
                        // Get action from queue in this background thread.
                        while (true)
                        {
                            lock (ActionWaitingLock)
                            {
                                try
                                {
                                    Action action = null;
                                    if (ActionQueue.TryTake(out action, 1))
                                    {
                                        ActionWaiting = action;
                                        break;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                }
                            }
                            try
                            {
                                Thread.Sleep(1);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        // Wait to join to the dispatcher thread
                        Dispatcher?.Invoke((Action)(() =>
                        {
                            lock (ActionWaitingLock)
                            {
                                // Action might have already been executed by UI thread in Add method.
                                if (ActionWaiting != null)
                                {
                                    ActionWaiting();
                                    ActionWaiting = null;
                                }
                                // Clear the more of the action queue, up to 100 items at a time.
                                // Batch up processing into lots of 100 so as to give some
                                // responsiveness if the collection is being bombarded.
                                int countDown = 100;
                                Action nextCommand = null;
                                // Note that countDown must be tested first, otherwise we throw away a queue item
                                while (countDown > 0 && ActionQueue.TryTake(out nextCommand))
                                {
                                    --countDown;
                                    nextCommand();
                                }
                            }
                        }));
                    }
                }
                catch (Exception)
                {
                    //TODO: Some diagnostics
                    //Assume render thread is dead, so exit
                }
            });
        }

        /// <summary>
        /// Adds an action to the processing queue
        /// </summary>
        /// <remarks>
        /// If we are running on the dispatcher thread, we could call the
        /// action directly, but then we've got the problem with queue
        /// jumping. It's desirable to immediately update the view model,
        /// as if we don't the code that added the item won't see it
        /// if an iteration is done over the collection, which would confuse
        /// the person using this collection.
        ///
        /// So, we need to add it to the queue and then process the queue
        /// so the view is consistent with the Add action.
        /// </remarks>
        /// <param name="Action"></param>
        public override void Add(Action Action) {

            // We're on a background thread
            if (!IsDispatcherThread)
                ActionQueue.Add(Action); //Just add action to queue.
            else // We're on UI thread
            {
                lock (ActionWaitingLock)
                {
                    // Add action to queue
                    ActionQueue.Add(Action);

                    // Background thread might have set next action to execute.
                    if (ActionWaiting != null)
                    {
                        ActionWaiting();
                        ActionWaiting = null;
                    }

                    // Clear the more of the action queue, up to 100 items at a time.
                    // Batch up processing into lots of 100 so as to give some
                    // responsiveness if the collection is being bombarded.
                    var CountDown = 300;
                    Action NextCommand = null;

                    // Note that countDown must be tested first, otherwise we throw away a queue item
                    while (CountDown > 0 && ActionQueue.TryTake(out NextCommand))
                    {
                        --CountDown;
                        NextCommand();
                    }
                }
            }
        }

        /// <summary>
        /// Adds a subscribe action to the subscriber queue
        /// </summary>
        /// <remarks>
        /// Subscriber queue is set to null after the 
        /// Dispatcher has been created. So subscriptions 
        /// can be handled directly once the dispatcher 
        /// queue is being processed.
        /// </remarks>
        /// <param name="SubscribeAction"></param>
        public override IDisposable QueueSubscribe(Action SubscribeAction)
        {
            if(SubscriberQueue != null)
            {
                try
                {
                    var WeakReference = new WeakReference(SubscribeAction);
                    SubscriberQueue[WeakReference] = null;

                    //Return a disposable for removing subscriber from the queue
                    return new DisposeDelegate(SubscribeAction, () => 
                    {
                        //Copy to avoid race condition
                        var subscriberQueue = SubscriberQueue;
                        if(subscriberQueue != null)
                        {
                            object Dummy;
                            subscriberQueue.TryRemove(WeakReference, out Dummy);
                        }
                    });
                }
                catch
                {
                    //SubscriberQueue may have been set to null on another thread
                    if (SubscriberQueue == null)
                        SubscribeAction();
                }
            }
            else SubscribeAction();

            return new DisposeDelegate();
        }

        #endregion
    }
}
