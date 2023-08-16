// See https://aka.ms/new-console-template for more information
Console.WriteLine("IntrView 0.1.0");
Console.WriteLine("Dump interrupt handlers from IBM PC memory dumps!");

Console.WriteLine("MUST BE FULL ADDRESS SPACE DUMP");

#region Constants
const int INTERRUPT_SPACE_SIZE = 0x400;
#endregion

#region Command-line parsing and file loading

if (args.Length < 1)
{
    Console.WriteLine($"A file must be provided!");
    Environment.Exit(1);
}

if (!File.Exists(args[0]))
{
    Console.WriteLine($"File {args[0]} does not exist!");
    Environment.Exit(2);    
}

if (!args[0].Contains(".bin", StringComparison.InvariantCultureIgnoreCase))
{
    Console.WriteLine($"Memory dump must have .bin extension!");
    Environment.Exit(3);    
}

byte[] bytes = File.ReadAllBytes(args[0]);

if (bytes.Length < INTERRUPT_SPACE_SIZE)
{
    Console.WriteLine("File cannot have interrupt list - less than 1kb in size!");
    Environment.Exit(4);
}
#endregion

#region Program

Console.WriteLine("\nMemory dump:\n");

using (BinaryReader br = new(new MemoryStream(bytes)))
{
    for (int interrupt = 0; interrupt < INTERRUPT_SPACE_SIZE / 4; interrupt++)
    {
        ushort ip = br.ReadUInt16();
        ushort cs = br.ReadUInt16();


        Console.WriteLine($"Interrupt 0x{interrupt:X2}\t\t\t{cs:X4}:{ip:X4}");
    }
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Done!");
Console.ResetColor();

#endregion