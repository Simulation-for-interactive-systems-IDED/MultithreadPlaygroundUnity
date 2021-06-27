using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Threading;

public class ClassicThreadSharedVariables : MonoBehaviour
{

    /// <summary>
    /// Assume this is a very expensive value to compute.
    /// </summary>
    int result = 0;
    readonly object sync = new object();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This uses a locking mechanism as synchronization strategy,
    /// note the use of the keyworkd lock() {}
    /// 
    /// The lock keywords uses the Monitor.Enter and Monitor.Exit c# APIs
    /// so the keyword it's just syntax sugar, there are other APIs for achieving locking in c#
    /// eg: ReaderWriteLockSlim, Interlocked, but they are let to the curiosity of the student to explore.
    /// </summary>
    public void RunThread()
    {
        Thread t1 = new Thread(() => {
            Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");

            // TODO: Explain what an atomic operation is for the CPU, why the result++ is not atomic
            // which operations are atomic, and why the locking APIs achieves a high-level atomicity.

            // This will acquire a lock over the sync object
            // while this lock is acquired no other thread that needs to acquire the lock can continue
            // until it's released.
            lock (sync)
            {
                // increment 100 times.
                for (int i = 0; i < 100; i++)
                {
                    result++;
                }
            }
        });
        t1.IsBackground = true;

        Thread t2 = new Thread(() => {
            Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");
            // This will acquire a lock over the sync object
            // while this lock is acquired no other thread that needs to acquire the lock can continue
            // until it's released.
            lock (sync)
            {
                // increment 200 times.
                for (int i = 0; i < 200; i++)
                {
                    result++;
                }
            }
        });
        t2.IsBackground = true;

        t1.Start();
        t2.Start();

        Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");

        // We need to block the main thread in order to know when both threads
        // finished.
        // This could be done with other more sophisticated synchronization methods,
        // however for the sake of simplicity let's just do it simple.
        t1.Join();
        t2.Join();

        // We expect result to be equals to 300 at this point.
        Debug.Log($"result: {result}");
        Assert.IsTrue(result == 300, "result is equals to 300.");
        result = 0;
    }

    /// <summary>
    /// This is absolutely non-thread safe, note that there are two threads performing
    /// non atomic operations on a shared variable (the "result" field). This can lead to unexpected results.
    /// </summary>
    public void RunThreadNonThreadSafe()
    {
        Thread t1 = new Thread(() => {
            Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");

            // increment 100 times.
            for (int i = 0; i < 100; i++)
            {
                result++;
            }
        });
        t1.IsBackground = true;

        Thread t2 = new Thread(() => {
            Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");

            // increment 200 times.
            for (int i = 0; i < 200; i++)
            {
                result++;
            }
        });
        t2.IsBackground = true;

        // This could work, but eventually it will fail, and when it does it will be really
        // hard to debug the reason without basic multithreading knowledge.

        t1.Start();
        t2.Start();

        Debug.Log($"Thread.CurrentThread is: {Thread.CurrentThread.ManagedThreadId}");

        // We need to block the main thread in order to know when both threads
        // finished.
        // This could be done with other more sophisticated synchronization methods,
        // however for the sake of simplicity let's just do it simple.
        t1.Join();
        t2.Join();

        // We expect result to be equals to 300 at this point.
        Debug.Log($"result: {result}");
        Assert.IsTrue(result == 300, "result is equals to 300.");
        result = 0;
    }
}
