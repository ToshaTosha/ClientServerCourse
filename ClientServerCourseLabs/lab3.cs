using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServerCourseLabs
{
    internal class lab3
    {
        private static double sharedValue = 0.5;
        private static readonly object lockObj = new object();
        private static bool cosTurn = true;
        private static bool running = true;
        private const int MaxIterations = 10;
        private static int iterationCount = 0;
        static void Main()
        {
            Console.WriteLine($"Начальное значение: {sharedValue}");
            Console.WriteLine($"Максимальное количество итераций: {MaxIterations}");

            Thread cosThread = new Thread(CalculateCosine);
            Thread acosThread = new Thread(CalculateArccosine);

            cosThread.Start();
            acosThread.Start();

            cosThread.Join();
            acosThread.Join();

            Console.WriteLine("Программа завершена.");
        }

        static void CalculateCosine()
        {
            sharedValue = Math.Cos(sharedValue * 2);
            try
            {
                while (running)
                {
                    lock (lockObj)
                    {
                        if (!running || iterationCount >= MaxIterations)
                        {
                            running = false;
                            Monitor.Pulse(lockObj);
                            break;
                        }

                        while (!cosTurn)
                        {
                            Monitor.Wait(lockObj);
                            if (!running) break;
                        }

                        if (!running) break;

                        sharedValue = Math.Cos(sharedValue);
                        iterationCount++;
                        Console.WriteLine($"Поток косинуса [{iterationCount}/{MaxIterations}]: новое значение = {sharedValue:F6}");

                        if (sharedValue < -1.0 || sharedValue > 1.0)
                        {
                            Console.WriteLine("Значение вышло за пределы [-1, 1] для арккосинуса");
                            running = false;
                            Monitor.Pulse(lockObj);
                            break;
                        }

                        cosTurn = false;
                        Monitor.Pulse(lockObj);
                    }

                    Thread.Sleep(300);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Поток косинуса прерван");
            }
        }

        static void CalculateArccosine()
        {
            sharedValue = Math.Acos(sharedValue);
            try
            {
                while (running)
                {
                    lock (lockObj)
                    {
                        if (!running || iterationCount >= MaxIterations)
                        {
                            running = false;
                            Monitor.Pulse(lockObj);
                            break;
                        }

                        while (cosTurn)
                        {
                            Monitor.Wait(lockObj);
                            if (!running) break;
                        }

                        if (!running) break;

                        if (sharedValue < -1.0 || sharedValue > 1.0)
                        {
                            Console.WriteLine("Ошибка: значение вне диапазона для арккосинуса");
                            running = false;
                            Monitor.Pulse(lockObj);
                            break;
                        }

                        sharedValue = Math.Acos(sharedValue);
                        iterationCount++;
                        Console.WriteLine($"Поток арккосинуса [{iterationCount}/{MaxIterations}]: новое значение = {sharedValue:F6}");

                        cosTurn = true;
                        Monitor.Pulse(lockObj);
                    }

                    Thread.Sleep(300);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Поток арккосинуса прерван");
            }
        }
    }
}
