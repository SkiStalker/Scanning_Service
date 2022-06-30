using System.Diagnostics;

namespace Task2_Kapserskiy
{
    class Program
    {

        static void Main(string[] args)
        {
            ScanTerminal scanTerminal = new ScanTerminal(@"C:\Users\Nikita\source\repos\Task2_Server\Task2_Client\bin\Debug\net6.0\Task2_Client.exe",
                @"C:\Users\Nikita\source\repos\Task2_Server\Task2_Server\bin\Debug\net6.0\Task2_Server.exe");
            bool alive = true;
            while (alive)
            {
                string? buf = Console.ReadLine();
                alive = scanTerminal.WriteToTerminal(buf);
            }
        }
    }
}