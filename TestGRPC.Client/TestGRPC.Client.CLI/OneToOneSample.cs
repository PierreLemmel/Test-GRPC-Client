using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static TestGRPC.Client.OneToOne;

namespace TestGRPC.Client.CLI
{
    public class OneToOneSample : ISample
    {
        private readonly OneToOneClient client;

        public OneToOneSample(OneToOneClient client)
        {
            this.client = client;
        }

        public async Task RunSample()
        {
            Console.WriteLine("Enter first number");
            int lhs = MoreConsole.ReadInt();
            Console.WriteLine();
            Console.WriteLine("Enter second number");
            int rhs = MoreConsole.ReadInt();
            Console.WriteLine();

            Console.WriteLine("Asking server...");

            AddNumbersRequest request = new() { Lhs = lhs, Rhs = rhs };
            AddNumbersResponse response = await client.AddNumbersAsync(request);

            Console.WriteLine("Server responded: ");
            Console.WriteLine($"{lhs} + {rhs} = {response.Result}");
        }
    }
}
