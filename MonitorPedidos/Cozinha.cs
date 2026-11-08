using System;
using System.Threading.Tasks;

namespace MonitorPedidos
{
    // Escuta PedidoCriado, simula o preparo (async/await) e dispara PreparoIniciado e PedidoPronto.
    public class Cozinha
    {
        public event EventHandler<PedidoEventArgs> PreparoIniciado;
        public event EventHandler<PedidoEventArgs> PedidoPronto;

        // async void para manipulador de evento.
        public async void AoPedidoCriado(object sender, PedidoEventArgs e)
        {
            e.Pedido.Status = StatusPedido.EmPreparo;
            PreparoIniciado?.Invoke(this, e);

            await Task.Delay(TimeSpan.FromSeconds(2)); // simula o preparo

            e.Pedido.Status = StatusPedido.Pronto;
            PedidoPronto?.Invoke(this, e);
        }
    }
}