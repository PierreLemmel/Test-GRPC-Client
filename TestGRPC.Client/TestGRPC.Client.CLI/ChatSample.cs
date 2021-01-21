using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static TestGRPC.Client.Chat;

namespace TestGRPC.Client.CLI
{
    public class ChatSample : ISample
    {
        private readonly ChatClient chat;

        public ChatSample(ChatClient chat)
        {
            this.chat = chat;
        }

        public async Task RunSample()
        {
            Console.WriteLine("Choose a chat name:");
            
            string user;
            while(true)
            {
                user = Console.ReadLine();

                if (!string.IsNullOrEmpty(user))
                    break;
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid user name");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Connected as '{user}'");
            Console.WriteLine("Start chating");
            Console.WriteLine();

            Metadata headers = new();
            headers.Add("user", user);

            var streamingCall = chat.Conversation(headers: headers);
            IClientStreamWriter<SentMessage> requestStream = streamingCall.RequestStream;
            IAsyncStreamReader<ReceivedMessage> responseStream = streamingCall.ResponseStream;

            CancellationTokenSource source = new();
            CancellationToken token = source.Token;

            await Task.WhenAll(
                ReceiveMessages(responseStream, token),
                SendMessages(requestStream, token)
            );
        }

        private async Task ReceiveMessages(IAsyncStreamReader<ReceivedMessage> responseStream, CancellationToken cancellation)
        {
            await foreach (ReceivedMessage received in responseStream.ReadAllAsync(cancellation))
            {
                PrintMessage(received.Body.Time, received.Author, received.Body.Content);
            }
        }

        private async Task SendMessages(IClientStreamWriter<SentMessage> requestStream, CancellationToken cancellation)
        {
            while(!cancellation.IsCancellationRequested)
            {
                string message = Console.ReadLine();
                string time = DateTime.Now.ToString("HH:mm:ss");

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                PrintMessage(time, "You", message);

                SentMessage sent = new()
                {
                    Body = new()
                    {
                        Content = message,
                        Time = time
                    }
                };

                await requestStream.WriteAsync(sent);
            }
        }

        private void PrintMessage(string time, string author, string message) => Console.WriteLine($"[{time}] - {author}: {message}");
    }
}