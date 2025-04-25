using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EXEMPLO_02_CLIENT
{
    class Program
    {
        static void Main(string[] args)
        {

            String IPServer = "127.0.0.1";

            TcpClient client = new TcpClient();
            client.Connect(IPServer, 80);
            String msg = "SET SUMMERTIME TRUE";

            Byte[] dados = System.Text.Encoding.UTF8.GetBytes(msg);
            NetworkStream stream = client.GetStream();
            stream.Write(dados, 0, dados.Length);
            client.Close();

            client = new TcpClient();
            client.Connect(IPServer, 80);
            msg = "GET TIME";

            dados = System.Text.Encoding.UTF8.GetBytes(msg);
            stream = client.GetStream();
            stream.Write(dados, 0, dados.Length);
            Byte[] dadosLidos = new Byte[client.ReceiveBufferSize];
            int tentativas = 0;
            while (!stream.DataAvailable && tentativas < 5) { tentativas++; Thread.Sleep(10); }
            int numBytes = stream.Read(dadosLidos, 0, dadosLidos.Length);
            String resposta = "";
            tentativas = 0;
            while (numBytes > 0)
            {

                resposta = resposta + System.Text.Encoding.UTF8.GetString(dadosLidos, 0, numBytes);
                tentativas = 0;
                numBytes = 0;
                while (!stream.DataAvailable && tentativas < 5) { tentativas++; }
                if (stream.DataAvailable)
                {
                    numBytes = stream.Read(dadosLidos, 0, dadosLidos.Length);
                }
            }
            System.Console.Write(resposta);
            Console.ReadKey();
        }
    }
}
