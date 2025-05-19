using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeDashboard
{
    public class SmartHomeMonitor
    {
        // Classe para representar dados do sensor
        public class SensorData
        {
            public DateTime Timestamp { get; set; }
            public int SensorId { get; set; }
            public double Temperature { get; set; }
            public double Humidity { get; set; }
            public int Movement { get; set; }
        }

        // Mapeamento de sensores para cômodos
        private Dictionary<int, string> sensorToRoom = new Dictionary<int, string>()
        {
            { 1, "Sala" },
            { 2, "Cozinha" },
            { 3, "Quarto Principal" },
            { 4, "Banheiro" },
            { 5, "Escritório" }
        };

        // Lista para armazenar todos os dados dos sensores
        private List<SensorData> allSensorData = new List<SensorData>();

        // Preço do kWh
        private const double PRICE_PER_KWH = 0.75; // R$ 0,75 por kWh

        // Consumo base por dispositivo em cada cômodo (em Watts)
        private Dictionary<string, List<(string, double)>> roomDevices = new Dictionary<string, List<(string, double)>>()
        {
            { "Sala", new List<(string, double)> { ("TV", 120), ("Iluminação", 60), ("Ar Condicionado", 1200) } },
            { "Cozinha", new List<(string, double)> { ("Geladeira", 300), ("Microondas", 1000), ("Iluminação", 80) } },
            { "Quarto Principal", new List<(string, double)> { ("Iluminação", 40), ("Ar Condicionado", 900), ("Carregadores", 20) } },
            { "Banheiro", new List<(string, double)> { ("Iluminação", 40), ("Chuveiro Elétrico", 4500), ("Ventilador", 65) } },
            { "Escritório", new List<(string, double)> { ("Computador", 200), ("Iluminação", 60), ("Impressora", 45) } }
        };

        // Construtor
        public SmartHomeMonitor()
        {
            LoadSensorData();
        }

        public void LoadSensorData()
        {
            try
            {
                // Caminho para o seu arquivo CSV
                string csvFilePath = "D:/Banco de Dados/BD_Casa_Inteligente (1).csv";

                // Ler todas as linhas do arquivo
                string[] lines = System.IO.File.ReadAllLines(csvFilePath);

                // Pular o cabeçalho se existir
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(',');

                    if (values.Length >= 5)
                    {
                        SensorData data = new SensorData();

                        // Tratamento da data
                        if (values[0].Contains("/"))
                        {
                            // Formato de data como "18/1/25 10:28"
                            data.Timestamp = DateTime.Parse(values[0]);
                        }
                        else
                        {
                            // Formato numérico (Excel)
                            double excelDate;
                            if (double.TryParse(values[0], out excelDate))
                            {
                                data.Timestamp = DateTime.FromOADate(excelDate);
                            }
                            else
                            {
                                continue; // Pular linha com formato inválido
                            }
                        }

                        // Converter os outros valores
                        int sensorId;
                        double temperature, humidity;
                        int movement;

                        if (int.TryParse(values[1], out sensorId) &&
                            double.TryParse(values[2], out temperature) &&
                            double.TryParse(values[3], out humidity) &&
                            int.TryParse(values[4], out movement))
                        {
                            data.SensorId = sensorId;
                            data.Temperature = temperature;
                            data.Humidity = humidity;
                            data.Movement = movement;

                            allSensorData.Add(data);
                        }
                    }
                }

                // Ordenar por data
                allSensorData = allSensorData.OrderBy(d => d.Timestamp).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao carregar dados: " + ex.Message);
            }
        }

        // Função para calcular o consumo atual baseado nos dados dos sensores
        public double CalculateCurrentConsumption(List<SensorData> latestData)
        {
            double totalConsumption = 0;

            foreach (var data in latestData)
            {
                if (sensorToRoom.ContainsKey(data.SensorId))
                {
                    string roomName = sensorToRoom[data.SensorId];

                    // Obter dispositivos do cômodo
                    var devices = roomDevices[roomName];

                    foreach (var device in devices)
                    {
                        string deviceName = device.Item1;
                        double baseConsumption = device.Item2;

                        // Ajustar consumo baseado nos sensores
                        double adjustedConsumption = baseConsumption;

                        // Ar condicionado consome mais quando está quente
                        if (deviceName.Contains("Ar Condicionado") && data.Temperature > 25)
                        {
                            adjustedConsumption *= 1.5;
                        }

                        // Iluminação só consome quando há movimento
                        if (deviceName.Contains("Iluminação") && data.Movement == 0)
                        {
                            adjustedConsumption *= 0.1; // Standby
                        }

                        // Adicionar ao total (convertendo de W para kW)
                        totalConsumption += adjustedConsumption / 1000;
                    }
                }
            }

            return totalConsumption;
        }

        // Função para estimar o custo mensal de energia
        public double EstimateMonthlyEnergyCost(List<SensorData> latestData)
        {
            // Calcular consumo atual
            double currentConsumption = CalculateCurrentConsumption(latestData);

            // Estimar consumo mensal (assumindo 24 horas por 30 dias)
            double monthlyConsumptionKWh = currentConsumption * 24 * 30;

            // Calcular custo
            return monthlyConsumptionKWh * PRICE_PER_KWH;
        }

        // Função para calcular o consumo mensal de um cômodo específico
        public double CalculateRoomMonthlyConsumption(string roomName, SensorData data)
        {
            double roomConsumption = 0;

            // Obter dispositivos do cômodo
            var devices = roomDevices[roomName];

            foreach (var device in devices)
            {
                string deviceName = device.Item1;
                double baseConsumption = device.Item2;

                // Ajustar consumo baseado nos sensores
                double adjustedConsumption = baseConsumption;

                // Ar condicionado consome mais quando está quente
                if (deviceName.Contains("Ar Condicionado") && data.Temperature > 25)
                {
                    adjustedConsumption *= 1.5;
                }

                // Iluminação só consome quando há movimento
                if (deviceName.Contains("Iluminação") && data.Movement == 0)
                {
                    adjustedConsumption *= 0.1; // Standby
                }

                // Diferentes padrões de uso para diferentes dispositivos
                double hoursPerDay = 0;

                if (deviceName.Contains("Geladeira"))
                {
                    hoursPerDay = 24; // Ligada o tempo todo
                }
                else if (deviceName.Contains("Ar Condicionado"))
                {
                    hoursPerDay = 8; // 8 horas por dia
                }
                else if (deviceName.Contains("Iluminação"))
                {
                    hoursPerDay = data.Movement == 1 ? 6 : 0; // 6 horas se tiver movimento
                }
                else if (deviceName.Contains("TV"))
                {
                    hoursPerDay = 5; // 5 horas por dia
                }
                else if (deviceName.Contains("Computador"))
                {
                    hoursPerDay = 8; // 8 horas por dia
                }
                else if (deviceName.Contains("Chuveiro"))
                {
                    hoursPerDay = 0.5; // 30 minutos por dia
                }
                else
                {
                    hoursPerDay = 2; // Padrão: 2 horas por dia
                }

                // Calcular consumo mensal (kWh)
                double deviceMonthlyConsumption = (adjustedConsumption / 1000) * hoursPerDay * 30;
                roomConsumption += deviceMonthlyConsumption;
            }

            return roomConsumption;
        }

        // Função para encontrar o cômodo com maior consumo
        public string FindHighestConsumptionRoom(List<SensorData> latestData)
        {
            string highestRoom = "";
            double highestConsumption = 0;

            foreach (var roomEntry in sensorToRoom)
            {
                string roomName = roomEntry.Value;
                int sensorId = roomEntry.Key;

                var sensorLatestData = latestData.FirstOrDefault(d => d.SensorId == sensorId);
                if (sensorLatestData != null)
                {
                    double roomConsumption = CalculateRoomMonthlyConsumption(roomName, sensorLatestData);

                    if (roomConsumption > highestConsumption)
                    {
                        highestConsumption = roomConsumption;
                        highestRoom = roomName;
                    }
                }
            }

            return $"{highestRoom} ({highestConsumption:F2} kWh/mês)";
        }
        
        // Método para obter os dados mais recentes de cada sensor
        public List<SensorData> GetLatestSensorData()
        {
            return allSensorData
                .GroupBy(d => d.SensorId)
                .Select(g => g.OrderByDescending(d => d.Timestamp).First())
                .ToList();
        }
        
        // Método para obter todos os dados de um sensor específico
        public List<SensorData> GetSensorData(int sensorId)
        {
            return allSensorData
                .Where(d => d.SensorId == sensorId)
                .OrderBy(d => d.Timestamp)
                .ToList();
        }
        
        // Método para obter o nome do cômodo a partir do ID do sensor
        public string GetRoomName(int sensorId)
        {
            if (sensorToRoom.ContainsKey(sensorId))
                return sensorToRoom[sensorId];
                
            return $"Sensor {sensorId}";
        }
        
        // Método para obter os dispositivos de um cômodo
        public List<(string, double)> GetRoomDevices(string roomName)
        {
            if (roomDevices.ContainsKey(roomName))
                return roomDevices[roomName];
                
            return new List<(string, double)>();
        }
    }
}