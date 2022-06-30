namespace Task2_Kapserskiy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ScanClient scanClient = new ScanClient();
            while (true)
            {
                string? buf = Console.ReadLine();
                Console.WriteLine("Client : "+buf);
                if (buf == null)
                    continue;

                if (buf == "exit")
                {
                    break;
                }

                string[] parts = buf.Split(' ');

                if (parts.Length != 2)
                {
                    Console.WriteLine("Wrong input");
                }
                else
                {
                    try
                    {
                        switch (parts[0])
                        {
                            case "scan":
                                {
                                    ScanDto? dto = scanClient.Scan(parts[1]);
                                    Console.Write(dto?.ToString());
                                    break;
                                }
                            case "status":
                                {
                                    ScanDto? dto = scanClient.Status(parts[1]);
                                    if (dto != null)
                                    {
                                        if (dto.Errors != null)
                                        {
                                            Console.WriteLine("====== Scan result ======");
                                            Console.Write(dto);
                                            Console.WriteLine("=========================");
                                        }
                                        else
                                        {
                                            Console.Write(dto);
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Incorrect command");
                                    break;
                                }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Scan service error");
                    }
                }
            }
        }
    }
}