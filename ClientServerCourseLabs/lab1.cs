using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServerCourseLabs
{
    class lab1
    {
        static void Main()
        {
            Console.WriteLine("Основной поток начал работу");

            Thread thread1 = new Thread(() => PrintNumbers(5, 15));
            Thread thread2 = new Thread(() => PrintNumbers(10, 20));

            thread1.Start();
            thread2.Start();

            Console.WriteLine("Основной поток завершил работу");
        }

        static void PrintNumbers(int start, int end)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начал работу");

            for (int i = start; i <= end; i++)
            {
                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
                Thread.Sleep(100);
            }

            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} завершил работу");
        }
    }
}
