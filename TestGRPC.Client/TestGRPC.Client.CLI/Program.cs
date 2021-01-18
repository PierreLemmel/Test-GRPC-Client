using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using TestGRPC.Client.CLI;
using static TestGRPC.Client.OneToOne;

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
        OneToOneClient client = new OneToOneClient(channel);
        OneToOneSample sample = new OneToOneSample(client);
        return sample;
    }
};

int sampleCount = sampleFactories.Length;

do
{
    Console.WriteLine("Choose sample to run :");
    Console.WriteLine(" 1 - OneToOne");
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