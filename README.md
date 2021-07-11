# MultithreadPlaygroundUnity
A collection of simple multithread examples in unity.

In Unity you can do all that you could do on C# related to multithreading, this is, you could use the `Thread` class, the `Task` class, etc... the only limitation is that you can’t invoke unity APIs other than `Debug.Log()` on another thread rather than the main thread. It’s important though that you apply all the precautions that you would apply on a multithreaded application (locking, synchronization mechanism, prevention of race conditions, deadlocks and other considerations)

In addition Unity implemented the **Job System** API to handle Multithreaded code in a simpler way.

## Used Unity Version
This project uses the **2020.3.12f1** LTS version.
