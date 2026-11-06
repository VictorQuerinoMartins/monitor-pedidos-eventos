# Monitor de Pedidos: Programação Orientada a Eventos em C#

Trabalho da disciplina de Paradigmas de Programação (UNESPAR, Campus Apucarana), tema **Programação Orientada a Eventos**, linguagem **C#**.

Simula o ciclo de pedidos de um restaurante. Os componentes (serviço de pedidos, cozinha, estoque, entregador e notificador) **não se conhecem**: eles se comunicam apenas por eventos.

## Fluxo

```
PedidoCriado ──► Cozinha ──► PreparoIniciado
      │             └──────► PedidoPronto ──► Entregador ──► SaiuParaEntrega
      │                                            └───────► PedidoEntregue
      ├──► Estoque ──► EstoqueBaixo
      └──► Notificar (avisa o cliente em cada etapa)
```

## Como executar

Pré-requisito: [SDK do .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash

dotnet run --project MonitorPedidos

```

## Estrutura

| Pasta / arquivo | Conteúdo |
|---|---|
| `MonitorPedidos/ServicoPedidos.cs` | Publica `PedidoCriado` (evento síncrono) |
| `MonitorPedidos/Cozinha.cs` | Assina `PedidoCriado`; preparo assíncrono com `async/await` |
| `MonitorPedidos/Entregador.cs` | Assina `PedidoPronto`; entrega assíncrona |
| `MonitorPedidos/Estoque.cs` | Assina `PedidoCriado`; publica `EstoqueBaixo` |
| `MonitorPedidos/Notificador.cs` | Recebe um callback (`Action<string>`) para enviar mensagens |
| `MonitorPedidos/Program.cs` | Só liga as assinaturas e dispara pedidos |

## Onde cada conceito aparece

| Conceito | Onde |


## Autores

- Victor Querino Martins
- Raul Soares
