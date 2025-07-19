using Confluent.Kafka;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        Console.WriteLine("🚀 Kafka Chat Producer Started!");
        Console.WriteLine("Type messages (type 'quit' to exit):");

        while (true)
        {
            Console.Write("You: ");
            var message = Console.ReadLine();

            if (message?.ToLower() == "quit")
                break;

            try
            {
                var result = await producer.ProduceAsync("chat-messages", 
                    new Message<Null, string> { Value = $"[{DateTime.Now:HH:mm:ss}] {message}" });
                
                Console.WriteLine($"✅ Message sent to partition {result.Partition}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}