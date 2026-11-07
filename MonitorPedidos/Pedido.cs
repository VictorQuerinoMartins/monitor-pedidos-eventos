using System;

namespace MonitorPedidos
{
    // Etapas pelas quais o pedido passa.
    public enum StatusPedido
    {
        Criado,
        EmPreparo,
        Pronto,
        SaiuParaEntrega,
        Entregue
    }

    // Classe que representa o pedido.
    public class Pedido
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = "";
        public string Item { get; set; } = "";
        public int Quantidade { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.Criado;
    }

    // Envelope com o pedido que vai junto nos eventos.
    public class PedidoEventArgs : EventArgs
    {
        public Pedido Pedido { get; }

        public PedidoEventArgs(Pedido pedido)
        {
            Pedido = pedido;
        }
    }
}
