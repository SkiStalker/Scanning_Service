
namespace Task2_Kapserskiy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool alive = true;
            ScanServiceNetwork? scanService = null;
            while (alive)
            {
                string? buf = Console.ReadLine();
                switch (buf)
                {
                    case "start":
                        {
                            if (scanService == null)
                            {
                                scanService = new ScanServiceNetwork();
                                scanService.Start();
                                Console.WriteLine("Scan server was started");
                            }
                            else
                            {
                                Console.WriteLine("Scan server already started");
                            }
                            break;
                        }
                    case "stop":
                        {
                            if (scanService != null)
                            {
                                scanService.Stop();
                                Console.WriteLine("Scan server was stopped");
                            }
                            else
                            {
                                Console.WriteLine("Scan server has not started");
                            }
                            break;
                        }
                    case "exit":
                        {
                            alive = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Unknown server command");
                            break;
                        }
                }
            }
        }
    }
}