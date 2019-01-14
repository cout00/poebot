using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApplication2.Logger;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.LockedAction {

    public static class ActionLockedExtensions {
        public static void Wait(this ActionLockerFactory self, int ms) {
            self.Execute(new ActionWaitParameter() { Interval = ms }, typeof(ActionWait));
        }
    }

    public class LockObserver {

        List<ISupportLock> Supporters = new List<ISupportLock>();

        public bool Locked {
            get {
                return Supporters.Where(a => a.Locked).Any();
            }
        }

        public void Add(ISupportLock supporter) {
            Supporters.Add(supporter);
        }

    }

    public class ActionLockerFactory : ISupportLock {
        static ActionLockerFactory factory;

        public static ActionLockerFactory GetFactory() {
            return factory;
        }

        class ActionInfo {
            public Task ExecuteTask { get; set; }
            public ISupportLock Locker { get; set; }
        }

        private readonly ConcurrentQueue<Func<Task>> _processingQueue = new ConcurrentQueue<Func<Task>>();
        private readonly ConcurrentDictionary<int, Task> _runningTasks = new ConcurrentDictionary<int, Task>();
        private readonly ILogger logger;
        private TaskCompletionSource<bool> _tscQueue = new TaskCompletionSource<bool>();

        public ActionLockerFactory(ILogger logger) {
            factory = this;
            this.logger = logger;
        }

        public void Queue(Func<Task> futureTask) {
            _processingQueue.Enqueue(futureTask);
        }

        async Task Process() {
            var t = _tscQueue.Task;
            StartTasks();
            await t;
        }

        public void ProcessBackground(Action<Exception> exception = null) {
            Locked = true;
            Task.Run(Process).ContinueWith(t => {
                exception?.Invoke(t.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        void StartTasks() {
            Func<Task> futureTask;
            if (!_processingQueue.TryDequeue(out futureTask)) {
                Locked = false;
                return;
            }

            var t = Task.Run(futureTask);
            logger.WriteLog("started " + t.Id.ToString());
            _runningTasks.TryAdd(t.GetHashCode(), t);
            t.ContinueWith((t2) => {
                Task _temp;
                _runningTasks.TryRemove(t2.GetHashCode(), out _temp);
                logger.WriteLog("ended " + _temp.Id.ToString());
                StartTasks();
            });

            if (_processingQueue.IsEmpty && _runningTasks.IsEmpty) {
                var _oldQueue = Interlocked.Exchange(
                    ref _tscQueue, new TaskCompletionSource<bool>());
                _oldQueue.TrySetResult(true);
            }
        }

        public bool Locked { get; set; }

        public void Execute<TParam>(TParam param, Type lockedActionType) where TParam : LockedActionParam, new() {
            ILockedAction<TParam> lockedAction = (ILockedAction<TParam>)Activator.CreateInstance(lockedActionType);
            var task = lockedAction.DoLockedAction(param);
            _processingQueue.Enqueue(task);
            if (!Locked) {
                ProcessBackground();
            }
        }

    }
}
