using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TestGRPC.Client.ClientSide;

namespace TestGRPC.Client.CLI
{
    public class ClientSideSample : ISample
    {
        private readonly ClientSideClient client;

        public ClientSideSample(ClientSideClient client)
        {
            this.client = client;
        }

        public async Task RunSample()
        {
            AsyncClientStreamingCall<GatherRequest, GatherResponse> streaming = client.Gather();

            IClientStreamWriter<GatherRequest> streamWriter = streaming.RequestStream;


            while (true)
            {
                Console.WriteLine("Write a name to add to the list (press Enter to finish)");
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    Console.WriteLine($"Streaming name '{input}' to server");
                    await streamWriter.WriteAsync(new() { Msg = input });
                    Console.WriteLine("Name successfully streamed to server");
                    Console.WriteLine();
                }
                else
                    break;
            }

            await streamWriter.CompleteAsync();
            Console.WriteLine("Submitting data to server...");

            GatherResponse response = await streaming.ResponseAsync;
            Console.WriteLine("Server responded: ");
            Console.WriteLine(response.Result);
        }
    }
}