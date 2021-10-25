using System;
using System.Threading;
using System.Timers;

namespace Multithreading
{
    class MyCounter
    {
        public static int count = 0;
        public static Mutex MuTexLock = new Mutex();
    }
    class IncThread
    {
        public Thread th;
        public IncThread()
        {
            th = new Thread(this.Go);
            th.Start();
        }
        void Go()
        {
            Console.WriteLine("IncThread is waiting for the mutex.");
            MyCounter.MuTexLock.WaitOne();
            Console.WriteLine("IncThread acquires the mutex.");

            int num = 5;
            do
            {
                Thread.Sleep(50);
                MyCounter.count++;
                Console.WriteLine("In IncThread, MyCounter.count is " + MyCounter.count);
                num--;
            } while (num > 0);
            Console.WriteLine("IncThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();
        }

        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("IncThread is at: " + DateTime.Now); ;
        }
    }
    class DecThread
    {
        public Thread th;
        public DecThread()
        {
            th = new Thread(new ThreadStart(this.Go));
            th.Start();
        }  
        void Go()
        {  
            Console.WriteLine("DecThread is waiting for the mutex.");  
            MyCounter.MuTexLock.WaitOne();  
            Console.WriteLine("DecThread acquires the mutex.");

            int num = 5;
            do
            {
                Thread.Sleep(50);  
                MyCounter.count--;  
                Console.WriteLine("In DecThread, MyCounter.count is " + MyCounter.count);  
                num--;  
            } while (num > 0) ;
            Console.WriteLine("DecThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();  
        }
        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("DecThread is at: " + DateTime.Now); ;
        }
    }

    class Program
    {
        public static void Main()
        {
            IncThread myt1 = new IncThread();
            DecThread myt2 = new DecThread();
            myt1.th.Join();
            myt2.th.Join();
            Console.Read();
        }
    }

}