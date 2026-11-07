using System;

namespace MonitorPedidos
{
    // Avisa o cliente usando a função de envio recebida no construtor.
    public class Notificador
    {
        private readonly Action<string> _enviar;

        public Notificador(Action<string> enviar)
        {
            _enviar = enviar;
        }

        public void AoPedidoCriado(object sender, PedidoEventArgs e)
        {
            Enviar($"Pedido {e.Pedido.Id} recebido: {e.Pedido.Quantidade}x {e.Pedido.Item} para {e.Pedido.Cliente}.");
        }

        public void AoPreparoIniciado(object sender, PedidoEventArgs e)
        {
            Enviar($"Pedido {e.Pedido.Id} em preparo.");
        }

        public void AoPedidoPronto(object sender, PedidoEventArgs e)
        {
            Enviar($"Pedido {e.Pedido.Id} pronto.");
        }

        public void AoSaiuParaEntrega(object sender, PedidoEventArgs e)
        {
            Enviar($"Pedido {e.Pedido.Id} saiu para entrega.");
        }

        public void AoPedidoEntregue(object sender, PedidoEventArgs e)
        {
            Enviar($"Pedido {e.Pedido.Id} entregue.");
        }

        public void AoEstoqueBaixo(object sender, int saldo)
        {
            Enviar($"Atenção: estoque baixo ({saldo} unidades restantes).");
        }

        private void Enviar(string mensagem)
        {
            _enviar($"[{DateTime.Now:HH:mm:ss}] {mensagem}");
        }
    }
}
