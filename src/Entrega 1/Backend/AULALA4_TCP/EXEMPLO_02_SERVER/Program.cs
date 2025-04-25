using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace EXEMPLO_02_SERVER
{
    

    class Program
    {
       
        static TcpListener tcpServer;
        static bool fim = false;
        static bool summerTime = false;

        static void Main(string[] args)
        {
            
            tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            tcpServer.Start();
            Console.WriteLine("Servidor iniciado...");

            Thread serverThread = new Thread(serverListener);
            serverThread.Start();
        }

        public static void serverListener()
        {
            while (!fim)
            {
                TcpClient client = tcpServer.AcceptTcpClient();

                Thread thread = new Thread(() => responseMessage(client));
                thread.Start();
            }
        }

        public static void responseMessage(TcpClient client)
        {
            Console.WriteLine($"Conexão recebida de {client.Client.RemoteEndPoint}");
            Thread.Sleep(10000);
            string msg = receiveTCPMessage(client);
            Console.WriteLine("Comando recebido: " + msg);
            string resposta = parseMsg(msg);
            sendTCPMessage(client, resposta);
        }

        private static string parseMsg(string msg)
        {
            string[] partes = msg.Split(' ');
            //if (partes.Length < 2) return "Comando inválido.";

            if (partes[0].ToUpper() == "GET")
            {
                if (partes[1].ToUpper() == "AMBIENTE")
                {
                    string nomeAmbiente = string.Join(" ", partes.Skip(2));
                    return "erro";
                }

            }

            return "Comando não reconhecido.";
        }

       

        public static string receiveTCPMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            int tentativas = 0;

            while (!stream.DataAvailable && tentativas < 10)
            {
                Thread.Sleep(50);
                tentativas++;
            }

            while (stream.DataAvailable)
            {
                bytesRead += stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
            }

            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public static void sendTCPMessage(TcpClient client, string msg)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(msg);

            if (stream.CanWrite)
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            //client.Close();
        }
    }
}
