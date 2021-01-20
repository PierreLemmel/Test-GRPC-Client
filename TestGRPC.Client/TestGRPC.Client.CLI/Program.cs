using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TestGRPC.Client.CLI;
using static TestGRPC.Client.ClientSide;
using static TestGRPC.Client.OneToOne;
using static TestGRPC.Client.ServerSide;

GrpcChannel CreateChannel()
{
    const string serverUrl = "https://localhost:5001";
    return GrpcChannel.ForAddress(serverUrl);
};

int? choice = null;
Func<ISample>[] sampleFactories = new Func<ISample>[]
{
    () =>
    {
        GrpcChannel channel = CreateChannel();
        OneToOneClient client = new(channel);
        OneToOneSample sample = new(client);
        return sample;
    },
    () =>
    {
        GrpcChannel channel = CreateChannel();
        ServerSideClient client = new(channel);
        ServerSideSample sample = new(client);
        return sample;
    },
    () =>
    {
        GrpcChannel channel = CreateChannel();
        ClientSideClient client = new(channel);
        ClientSideSample sample = new(client);
        return sample;
    }
};

int sampleCount = sampleFactories.Length;

do
{
    Console.WriteLine("Choose sample to run :");
    Console.WriteLine(" 1 - OneToOne");
    Console.WriteLine(" 2 - ServerSide streaming");
    Console.WriteLine(" 3 - ClientSide streaming");
    Console.WriteLine();
    int input = MoreConsole.ReadInt();

    if (input <= 0 || input > sampleCount)
    {
        Console.WriteLine();
        Console.WriteLine($"Invalid choice: choose a number between {1} and {sampleCount}");
        Console.WriteLine();
    }
    else
    {
        choice = input - 1;
    }
}
while (choice is null);

ISample sample = sampleFactories[choice.Value].Invoke();

await sample.RunSample();