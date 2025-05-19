# Documentação do Sistema de Controle de Casa Inteligente

## Visão Geral

Este sistema implementa um painel de controle para uma casa inteligente, permitindo o monitoramento e controle de diversos dispositivos em diferentes cômodos. O sistema captura dados de sensores (temperatura, umidade e movimento) e status dos dispositivos (ligado/desligado), enviando essas informações para uma aplicação web.

## Funcionalidades

1. *Controle de Dispositivos*:
   - Ligar/desligar dispositivos em cada cômodo (ar condicionado, iluminação, eletrodomésticos, etc.)
   - Visualização do status atual de cada dispositivo
   - Monitoramento do consumo de energia estimado

2. *Monitoramento de Sensores*:
   - Temperatura em cada cômodo
   - Umidade em cada cômodo
   - Detecção de movimento

3. *Sincronização com Aplicação Web*:
   - Envio automático de dados para servidor web
   - Opção de sincronização manual
   - Backup local em caso de falha na comunicação

## Arquitetura do Sistema

### Hardware Simulado

O sistema simula sensores e dispositivos de uma casa inteligente:

- *Sensores*:
  - 5 sensores de temperatura/umidade (um por cômodo)
  - 5 sensores de movimento (um por cômodo)

- *Dispositivos Controlados*:
  - Ar condicionados
  - Sistemas de iluminação
  - Eletrodomésticos (TV, geladeira, microondas, etc.)
  - Outros dispositivos específicos por cômodo

### Comunicação com a Web

O sistema utiliza o protocolo HTTP para enviar dados para a aplicação web:

1. Os dados são coletados em tempo real (simulados no código)
2. São formatados em JSON
3. Enviados via requisição POST para a API
4. Em caso de falha na comunicação, os dados são armazenados localmente

Formato dos dados enviados:
```json
{
  "DeviceData": [
    {
      "DeviceId": "sala_ac",
      "DeviceName": "Ar Condicionado",
      "RoomName": "Sala",
      "IsOn": true,
      "PowerConsumption": 1200,
      "LastStatusChange": "2023-05-18 14:30:22"
    },
    ...
  ],
  "SensorData": [
    {
      "SensorId": 1,
      "RoomName": "Sala",
      "Temperature": 23.5,
      "Humidity": 50.2,
      "Movement": 1,
      "Timestamp": "2023-05-18 14:30:25"
    },
    ...
  ],
  "Timestamp": "2023-05-18 14:30:30"
}