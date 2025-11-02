Keep working on:
1. Introduce HTTPS/TLS between services
2. Add a service to receive commands from all platforms.
3. Add Kafka to pub/sub for commands and platforms, latency is 50 to 200ms.
4. Add extra low latency commands service by redis pub/sub, < 5ms.
5. Use Flink on log real time and batch analysis
6. More elaborate use-case
7. Service Discovery
8. Authentication and authorization
9. Multiple cluster release

c# services docker + k8s
Architecture
<img width="1472" height="761" alt="image" src="https://github.com/user-attachments/assets/51e3c1e0-78d9-418c-b848-c5736bd0c64c" />

K8s Architecture
<img width="1536" height="849" alt="image" src="https://github.com/user-attachments/assets/5d266e4b-2df9-46fe-a758-608680e5b441" />
