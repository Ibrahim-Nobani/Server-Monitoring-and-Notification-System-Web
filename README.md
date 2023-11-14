# Server Monitoring and Notification System

Welcome to the Server Monitoring and Notification System! This C# application is designed to collect and publish server statistics, including memory usage, available memory, and CPU usage. It accomplishes this by periodically gathering server data and encapsulating it into statistics objects, which are then published to a message queue. Additionally, the system can detect anomalies or high resource usage and send alerts using SignalR, providing you with real-time insights into your server's performance. With a focus on flexibility and modularity, this project offers a comprehensive solution for monitoring and managing server resources effectively.

## Table of Contents

- [Overview](#overview)
- [Task 1: Server Statistics Collection Service](#task-1-server-statistics-collection-service)
- [Task 2: Message Processing and Anomaly Detection Service](#task-2-message-processing-and-anomaly-detection-service)
- [Task 3: SignalR Event Consumer Service](#task-3-signalr-event-consumer-service)
- [Prerequisites](#prerequisites)
- [Cloning the Project](#cloning-the-project)
- [Contributing](#contributing)
- [License](#license)

## Overview

This project goal is to create a Server Monitoring and Notification System in C#. It includes three main tasks:

- Task 1: Server Statistics Collection Service
- Task 2: Message Processing and Anomaly Detection Service
- Task 3: SignalR Event Consumer Service

Each task serves a specific purpose within the system.

## Task 1: Server Statistics Collection Service

**Objective:** Develop a C# process that collects and publishes server statistics, including memory usage, available memory, and CPU usage.

**Requirements:**

- Collect server statistics (memory usage, available memory, and CPU usage) at regular intervals.
- Encapsulate collected statistics into a ServerStatistics object.
- Publish statistics to a message queue under the topic `ServerStatistics.<ServerIdentifier>`.
- Implement an abstraction for message queuing.

**Code Snippets:**

- [ServerStatistics Class Definition](#server-statistics-data-type-class-definition)
- [Server Statistics Configuration (appsettings.json)](#server-statistics-configuration-appsettingsjson)

## Task 2: Message Processing and Anomaly Detection Service

**Objective:** Develop a C# process that receives server statistics from the message queue, persists the data to a MongoDB instance, and sends alerts if anomalies or high usage is detected.

**Requirements:**

- Receive messages from the message queue and deserialize them into objects.
- Persist deserialized data to a MongoDB instance.
- Implement anomaly detection logic based on configurable threshold percentages.
- Send "Anomaly Alerts" and "High Usage Alerts" via SignalR.
- Implement abstractions for MongoDB and message queuing.

**Alert Calculation Equations:**

- For Anomaly Alert:
    - Memory Usage Anomaly Alert: `if (CurrentMemoryUsage > (PreviousMemoryUsage * (1 + MemoryUsageAnomalyThresholdPercentage))`
    - CPU Usage Anomaly Alert: `if (CurrentCpuUsage > (PreviousCpuUsage * (1 + CpuUsageAnomalyThresholdPercentage))`
- For High Usage Alert:
    - Memory High Usage Alert: `if ((CurrentMemoryUsage / (CurrentMemoryUsage + CurrentAvailableMemory)) > MemoryUsageThresholdPercentage)`
    - CPU High Usage Alert: `if (CurrentCpuUsage > CpuUsageThresholdPercentage)`

**Code Snippets:**

- [ServerStatistics Class Definition](#server-statistics-data-type-class-definition)
- [Anomaly Detection Service Configuration (appsettings.json)](#anomaly-detection-service-configuration-appsettingsjson)

## Task 3: SignalR Event Consumer Service

**Objective:** Develop a C# process that connects to a SignalR hub and prints received events to the console.

**Requirements:**

- Establish a connection to a SignalR hub.
- Subscribe to events and print them to the console as they are received.

**Code Snippet:**

- [SignalR Event Consumer Service Configuration (appsettings.json)](#signalr-event-consumer-service-configuration-appsettingsjson)

## Prerequisites

Before getting started, ensure you have the following prerequisites:

- .NET Core [Version]
- RabbitMQ Server
- MongoDB Database
- SignalR Hub URL

## Cloning the Project

To clone this project, follow these steps:

1. Open your terminal or command prompt.

2. Navigate to the directory where you want to clone the project.

3. Run the following command to clone the repository:

```bash
git clone https://github.com/your-username/server-monitoring-project.git
```

## Contributing

Explain how others can contribute to the project and provide guidelines for contributions.

- Fork the repository
- Create a new branch
- Make your changes
- Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
