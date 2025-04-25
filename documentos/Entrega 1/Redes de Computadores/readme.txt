# EXEMPLO_02_SERVER

Este projeto é um exemplo simples de servidor TCP implementado em C# utilizando a classe `TcpListener`. Ele faz parte de uma solução de estudos chamada `AULA_04_LAB_TCP`, que contém diversos exemplos de comunicação em rede.

## 📌 Objetivo

Demonstrar como criar um servidor TCP que:
- Escuta conexões em uma porta específica (porta 5000).
- Aceita conexões de clientes.
- Envia uma mensagem de boas-vindas assim que a conexão é estabelecida.
- Fecha a conexão após enviar a mensagem.

## 🛠️ Tecnologias

- .NET Framework (compatível com Visual Studio 2019+)
- C#
- System.Net.Sockets

## 🧠 Como funciona

O servidor utiliza `TcpListener` para escutar conexões de entrada em todas as interfaces de rede (`IPAddress.Any`) na porta 5000. Quando um cliente se conecta, o servidor envia uma mensagem fixa e finaliza a conexão.

### Fluxo de execução

1. Inicia o `TcpListener` na porta 5000.
2. Aguarda conexões usando `AcceptTcpClient()`.
3. Ao conectar:
   - Envia uma mensagem: `Conexão estabelecida com o servidor!`.
   - Fecha a conexão.
4. Repete o processo infinitamente.

## ▶️ Como executar

1. Abra a solução `AULA_04_LAB_TCP.sln` no Visual Studio.
2. Defina o projeto `EXEMPLO_02_SERVER` como projeto de inicialização.
3. Pressione **F5** para executar o servidor.

⚠️ Certifique-se de que a **porta 5000 esteja liberada no firewall**.

## 🔌 Testando com um cliente

Você pode testar com o projeto `EXEMPLO_02_CLIENT`, também presente na mesma solução, ou com ferramentas como:

```bash
telnet <IP_DO_SERVIDOR> 5000