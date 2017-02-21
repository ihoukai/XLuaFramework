using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace XLuaFramework
{
    public class ThreadManager : Manager
    {
        public const int minThreads = 1;
        public const int maxThreads = 3;
        private static int numThreads = 0;
        public static int NumThreads
        {
            get { return numThreads; }
        }

        private static bool init;
        private static ThreadManager instance;

        void Awake()
        {
            instance = this;
            Init();
        }

        private static void Init()
        {
            if (!init)
            {
                ThreadPool.SetMinThreads(minThreads, minThreads);
                ThreadPool.SetMaxThreads(maxThreads, maxThreads);
                init = true;
            }
        }

        private struct ActionInfo
        {
            public Action action;
            public float delayTime;

            public ActionInfo(Action action, float delayTime = 0f)
            {
                this.action = action;
                this.delayTime = delayTime;
            }
        }
        private List<ActionInfo> _actions = new List<ActionInfo>();
        private List<ActionInfo> _delayed = new List<ActionInfo>();
        private List<ActionInfo> _currentDelayed = new List<ActionInfo>();

        public static void RunOnMainThread(Action action, float time = 0f)
        {
            if (action == null)
            {
                return;
            }

            if (time != 0)
            {
                lock (instance._delayed)
                {
                    instance._delayed.Add(new ActionInfo(action, Time.time + time));
                }
            }
            else
            {
                lock (instance._actions)
                {
                    instance._actions.Add(new ActionInfo(action));
                }
            }
        }

        public static Thread RunAsync(Action a)
        {
            if (a == null)
            {
                return null;
            }

            Init();
            //while (numThreads >= maxThreads)
            //{
            //    Thread.Sleep(200);
            //}
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, new ActionInfo(a));
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ActionInfo actionInfo = (ActionInfo)action;
                actionInfo.action();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
            
        }

        List<ActionInfo> _currentActions = new List<ActionInfo>();

        // Update is called once per frame
        void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            foreach (var a in _currentActions)
            {
                try
                {
                    a.action();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.delayTime <= Time.time));
                foreach (var item in _currentDelayed)
                {
                    _delayed.Remove(item);
                }
            }
            foreach (var delayed in _currentDelayed)
            {
                try
                {
                    delayed.action();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}
