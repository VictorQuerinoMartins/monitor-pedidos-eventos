using System;
using System.Threading.Tasks;

namespace MonitorPedidos
{
    // Escuta PedidoPronto, simula a entrega (async/await) e dispara SaiuParaEntrega e PedidoEntregue.
    public class Entregador
    {
        public event EventHandler<PedidoEventArgs> SaiuParaEntrega;
        public event EventHandler<PedidoEventArgs> PedidoEntregue;

        // async void para manipulador de evento.
        public async void AoPedidoPronto(object sender, PedidoEventArgs e)
        {
            e.Pedido.Status = StatusPedido.SaiuParaEntrega;
            SaiuParaEntrega?.Invoke(this, e);

            await Task.Delay(TimeSpan.FromSeconds(3)); // simula a entrega

            e.Pedido.Status = StatusPedido.Entregue;
            PedidoEntregue?.Invoke(this, e);
        }
    }
}
