using System;
using System.Threading;

namespace ClientServerCourseLabs
{
    class lab2
    {
        static Thread thread1;

        static void Main()
        {
            Console.WriteLine("Основной поток начал работу");

            Console.WriteLine("\nЭксперимент 1: Без задержки между запусками");
            thread1 = new Thread(PrintNumbersFirst);
            Thread thread2 = new Thread(PrintNumbersSecond);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("\nЭксперимент 2: С задержкой 1с между запусками");
            thread1 = new Thread(PrintNumbersFirst);
            thread2 = new Thread(PrintNumbersSecond);

            thread1.Start();
            Thread.Sleep(1000);
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("\nОсновной поток завершил работу");
        }

        static void PrintNumbersFirst()
        {
            Console.WriteLine("Поток 1 начал работу");
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine($"Поток 1: {i}");
            }
            Console.WriteLine("Поток 1 завершил работу");
        }

        static void PrintNumbersSecond()
        {
            Console.WriteLine("Поток 2 ожидает завершения потока 1...");

            Console.WriteLine("Поток 2 начал работу");
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine($"Поток 2: {i}");
            }
            Console.WriteLine("Поток 2 завершил работу");
        }
    }
}
