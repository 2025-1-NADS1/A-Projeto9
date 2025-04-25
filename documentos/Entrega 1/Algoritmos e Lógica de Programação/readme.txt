Código de Controles:

// Controle do ar-condicionado baseado em temperatura e presença
if (idSensor == 1 || idSensor == 2 ) {
if(movimento == 1 && temperatura > 26)
{
Console.WriteLine("Ar-condicionado: Ligado");
}
else
{
Console.WriteLine("Ar-condicionado: Desligado");
}
}

// Controle da piscina (bomba e aquecedor)
if (idSensor == 5)
{
if (temperatura < 25)
{
Console.WriteLine("Piscina: Aquecedor Ligado");
}
else
{
Console.WriteLine("Piscina: Aquecedor Desligado\n");
}
}

Código de Iniciação do Sistema:

using System;
class CasaInteligente
{
static void Main()
{
// IDs dos sensores
int[] idSensor = { 1, 2, 3, 4, 5 };
// Temperatura de cada ambiente
int[] temperatura = { 24, 22, 27, 30, 26 };
// Umidade de cada ambiente
int[] umidade = { 60, 55, 50, 40, 70 };
// Presença detectada pelos sensores (1 = Sim, 0 = Não)
int[] movimento = { 1, 1, 0, 1, 0 };
// Consumo energético médio por ambiente (kW/h)
double[] energia = { 1.5, 1.5, 0.05, 3, 7 };
// Nome dos locais da casa
string[] locais = { "Quarto 1", "Quarto 2", "Sala", "Cozinha", "Piscina" };
// Estrutura de repetição para percorrer os sensores
for (int i = 0; i < idSensor.Length; i++)
{
Console.WriteLine($"\nTimeStamp: {DateTime.Now.ToString("dd/MM/yyyy
HH:mm:ss")}");
Console.WriteLine($"== Status do {locais[i]} ==");
Console.WriteLine($"Temperatura: {temperatura[i]}°C | Umidade:
{umidade[i]}%");
Console.WriteLine($"Consumo energético médio: {energia[i]} kW/h");
// Controle de luz baseado em movimento
Console.WriteLine(movimento[i] == 1 ? "Luz: Ligada" : "Luz: Desligada");
// Controle do ar-condicionado baseado em temperatura e presença (somente
para os quartos)
bool sensorAr = idSensor[i] == 1 || idSensor[i] == 2;
if (sensorAr)
{
if (movimento[i] == 1 && temperatura[i] > 26)
Console.WriteLine("Ar-condicionado: Ligado");
else
Console.WriteLine("Ar-condicionado: Desligado");
}
// Controle da piscina (bomba e aquecedor)
if (idSensor[i] == 5)
{
if (temperatura[i] < 25)
Console.WriteLine("Piscina: Aquecedor Ligado");
else
Console.WriteLine("Piscina: Aquecedor Desligado\n");
}
 }
 }
}
