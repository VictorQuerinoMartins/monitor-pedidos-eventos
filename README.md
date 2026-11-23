# Monitor de Pedidos: Programação Orientada a Eventos em C#

Trabalho da disciplina de Paradigmas de Programação (UNESPAR, Campus Apucarana), tema **Programação Orientada a Eventos**, linguagem **C#**.

Simula o ciclo de pedidos de um restaurante. Os componentes (serviço de pedidos, cozinha, estoque, entregador e notificador) **não se conhecem**: eles se comunicam apenas por eventos. O `Program` é o único ponto que conhece todos e faz as inscrições com `+=`.

## Fluxo

```
PedidoCriado ──► Cozinha ──► PreparoIniciado
      │             └──────► PedidoPronto ──► Entregador ──► SaiuParaEntrega
      │                                            └───────► PedidoEntregue
      ├──► Estoque ──► EstoqueBaixo
      └──► Notificador (avisa o cliente em cada etapa)
```

## Como executar

Pré-requisito: [SDK do .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
dotnet run --project MonitorPedidos
```

O programa cria três pedidos, mostra as notificações no console e fica aguardando **Enter** no final. Isso é necessário porque o preparo (2 s) e a entrega (3 s) rodam em handlers assíncronos que terminam depois do `Main`.

Saída esperada (a ordem das últimas linhas pode variar):

```text
[16:24:02] Pedido 1 recebido: 3x Pizza para Ana.
[16:24:02] Pedido 1 em preparo.
[16:24:02] Atenção: estoque baixo (2 unidades restantes).
[16:24:02] Pedido 2 recebido: 5x Hambúrguer para Bruno.
[16:24:02] Pedido 2 em preparo.
Pedido recusado: Estoque insuficiente para o pedido 3: precisa de 8, saldo 2.
Aguardando preparo e entrega... (Enter para sair)
[16:24:04] Pedido 1 pronto.
[16:24:04] Pedido 2 pronto.
[16:24:04] Pedido 1 saiu para entrega.
[16:24:04] Pedido 2 saiu para entrega.
[16:24:07] Pedido 2 entregue.
[16:24:07] Pedido 1 entregue.
```

Para apenas compilar, sem executar:

```bash
dotnet build
```

## Estrutura

| Pasta / arquivo | Conteúdo |
|---|---|
| `MonitorPedidos/Pedido.cs` | Modelo `Pedido`, enum `StatusPedido` e `PedidoEventArgs` |
| `MonitorPedidos/ServicosPedidos.cs` | Publica `PedidoCriado` (evento síncrono) |
| `MonitorPedidos/Cozinha.cs` | Assina `PedidoCriado`; preparo assíncrono com `async/await` |
| `MonitorPedidos/Entregador.cs` | Assina `PedidoPronto`; entrega assíncrona |
| `MonitorPedidos/Estoque.cs` | Assina `PedidoCriado`; publica `EstoqueBaixo` |
| `MonitorPedidos/Notificador.cs` | Recebe um callback (`Action<string>`) para enviar mensagens |
| `MonitorPedidos/Program.cs` | Composition root: liga as assinaturas e dispara pedidos |

## Contrato de eventos

| Evento | Publicador | Assinantes | Payload |
|---|---|---|---|
| `PedidoCriado` | `ServicoPedidos` | `Estoque`, `Notificador`, `Cozinha` | `PedidoEventArgs` |
| `PreparoIniciado` | `Cozinha` | `Notificador` | `PedidoEventArgs` |
| `PedidoPronto` | `Cozinha` | `Notificador`, `Entregador` | `PedidoEventArgs` |
| `SaiuParaEntrega` | `Entregador` | `Notificador` | `PedidoEventArgs` |
| `PedidoEntregue` | `Entregador` | `Notificador` | `PedidoEventArgs` |
| `EstoqueBaixo` | `Estoque` | `Notificador` | `int` (saldo) |

## Onde cada conceito aparece

| Conceito | Onde |
|---|---|
| Evento | `ServicosPedidos.PedidoCriado`, `Cozinha.PedidoPronto`, `Estoque.EstoqueBaixo` |
| Manipulador de evento | `Cozinha.AoPedidoCriado`, `Entregador.AoPedidoPronto`, `Estoque.AoPedidoCriado` |
| Listener / assinante | `Program.cs`, nos `+=` |
| Callback | `Notificador`, que recebe `Action<string>` no construtor |
| Fluxo orientado a eventos | Cadeia `PedidoCriado → PreparoIniciado → PedidoPronto → SaiuParaEntrega → PedidoEntregue` |
| Inversão de controle | `ServicoPedidos` dispara o evento sem saber quem reage |
| Evento síncrono | Todos os eventos do projeto: o publicador só continua depois dos handlers |
| Assincronismo | `Cozinha.AoPedidoCriado` e `Entregador.AoPedidoPronto` com `async void` e `Task.Delay` |
| Delegate multicast | Um evento com vários `+=` recebe vários handlers na ordem de inscrição |
| Composition root | `Program.cs`, o único que conhece todos os componentes |
| Tratamento de exceção | `Estoque` lança `InvalidOperationException` e o `Program` captura com `try/catch` |

## Observações

- O `Estoque` é único e em memória; não há banco de dados, interface gráfica nem broker de mensagens.
- A ordem de inscrição importa: o `Estoque` é inscrito antes do `Notificador` e da `Cozinha` para que um pedido recusado não seja anunciado nem preparado.
- Os handlers demorados usam `async void`, formato comum em eventos, mas que não permite aguardar a conclusão nem capturar exceções ocorridas depois do `await`.

## Autores

- Raul Soares de Almeida
- Victor Querino Martins
