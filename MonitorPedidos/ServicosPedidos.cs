using System;

namespace MonitorPedidos
{
    // Publicador, cria o pedido e dispara PedidoCriado. Sem conhecer ninguém.
    public class ServicoPedidos
    {
        private int _proximoId = 1;

        public event EventHandler<PedidoEventArgs> PedidoCriado;

        public Pedido Criar(string cliente, string item, int quantidade)
        {
            var pedido = new Pedido
            {
                Id = _proximoId++,
                Cliente = cliente,
                Item = item,
                Quantidade = quantidade
            };

            // síncrono: só retorna depois que todos os assinantes executarem.
            PedidoCriado?.Invoke(this, new PedidoEventArgs(pedido));

            return pedido;
        }
    }
}