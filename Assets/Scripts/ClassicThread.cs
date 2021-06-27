using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class ClassicThread : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunThread()
    {
        Thread t = new Thread(() => {
            // This is just a dummy operation.
            // note that this doesnt interact with anything outside.
            // eg: class fields, unity APIs.
            int i = 0;            
            // Sleeps this for 5 seconds
            Thread.Sleep(TimeSpan.FromSeconds(5));            
            // Then finishes the execution of the thread.
        });

        // A thread is either a background thread or a foreground thread.
        // Background threads are identical to foreground threads, except
        // that background threads do not prevent a process from terminating.
        // Once all foreground threads belonging to a process have terminated,
        // the common language runtime ends the process. Any remaining background
        // threads are stopped and do not complete.
        // by default is false.
        t.IsBackground = true;
        t.Start();
    }

    public void RunThreadBlockingMainThread()
    {
        Thread t = new Thread(() => {
            int i = 0;
            // Sleeps this for 5 seconds
            Thread.Sleep(TimeSpan.FromSeconds(5));
            // Then finishes the execution of the thread.
        });
        t.IsBackground = true;
        t.Start();

        // Blocks the calling thread until the thread represented by this instance terminates.
        // Join is a synchronization method that blocks the calling thread (that is, the thread that calls the method) until the thread whose Join method is called has completed. Use this method to ensure that a thread has been terminated. The caller will block indefinitely if the thread does not terminate.
        // https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.join?view=net-5.0
        // in this example main thread will block for about 5 seconds.
        t.Join();
    }

    public void RunThreadNonThreadSafe()
    {
        Thread t = new Thread(() => {
            // This is just a dummy operation.
            // ################################
            // WARNING ########################
            // ################################
            // Note that this is not thread safe since it uses the Unity APIs
            // Unfortunately however, the Unity API should be called ONLY from the main thread.
            // this could create race condition, also keep in mind that this may work in your use case
            // but definetly there will be a day in which a very strangenous bug will appear and debug that will be
            // almost impossible without basic knowledge of multithreading.
            // TODO: Explain what is multithreading and basic problems with it.

            // Note however that when you try to run this code Unity will throw an exception like:
            // UnityException: get_gameObject can only be called from the main thread.
            // for more information read:
            // https://support.unity.com/hc/en-us/articles/208707516-Why-should-I-use-Threads-instead-of-Coroutines-
            gameObject.transform.position = new Vector3(0, 0, 10);
            // Then finishes the execution of the thread.
        });
        t.IsBackground = true;
        t.Start();
    }
}
