using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TestGRPC.Client.ServerSide;

namespace TestGRPC.Client.CLI
{
    public class ServerSideSample : ISample
    {
        private readonly ServerSideClient client;

        public ServerSideSample(ServerSideClient client)
        {
            this.client = client;
        }

        public async Task RunSample()
        {
            int from, to;

            do
            {
                Console.WriteLine("Enter min number");
                from = MoreConsole.ReadInt();
                Console.WriteLine();
                Console.WriteLine("Enter max number");
                to = MoreConsole.ReadInt();
                Console.WriteLine();

                if (from > to)
                {
                    Console.WriteLine("Max number must be greater than min");
                    Console.WriteLine();
                }
            }
            while (from > to);

            Console.WriteLine("Asking server...");

            CountRequest request = new() { From =from, To = to };
            using AsyncServerStreamingCall<CountResponse> serverResponse = client.Count(request);

            Console.WriteLine("Server responded: ");
            IAsyncEnumerable<CountResponse> responses = serverResponse.ResponseStream.ReadAllAsync();
            await foreach (CountResponse countResponse in responses)
            {
                Console.WriteLine($"Count: {countResponse.Current}");
            }
        }
    }
}