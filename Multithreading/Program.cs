using NLog;
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
            
            Program.WriteToFile("IncThread is waiting for the mutex.");
            MyCounter.MuTexLock.WaitOne();
            Program.WriteToFile("IncThread acquires the mutex.");

            int num = 5;
            do
            {
                Thread.Sleep(50);
                MyCounter.count++;
                Program.WriteToFile("In IncThread, MyCounter.count is " + MyCounter.count);
                num--;
            } while (num > 0);
            Program.WriteToFile("IncThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();
        }

        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Program.WriteToFile("IncThread is at: " + DateTime.Now); ;
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
            Program.WriteToFile("DecThread is waiting for the mutex.");
            MyCounter.MuTexLock.WaitOne();
            Program.WriteToFile("DecThread acquires the mutex.");

            int num = 5;
            do
            {
                Thread.Sleep(50);
                MyCounter.count--;
                Program.WriteToFile("In DecThread, MyCounter.count is " + MyCounter.count);
                num--;
            } while (num > 0);
            Program.WriteToFile("DecThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();
        }
        public static void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Program.WriteToFile("DecThread is at: " + DateTime.Now); ;
        }
    }


    class Program
    {
        readonly static Logger logger = LogManager.GetLogger("fileLogger");

        public static void WriteToFile(string Message)
        {
            logger.Info(Message);
        }
        public static void Main()
        {

            IncThread myt1 = new IncThread();
            DecThread myt2 = new DecThread();
            myt1.th.Join();
            myt2.th.Join();
            NLog.LogManager.Shutdown();

            Console.Read();            
        }
    }

}