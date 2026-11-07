using System;

namespace MonitorPedidos
{
    // Escuta o PedidoCriado, baixa o saldo e avisa quando fica baixo.
    public class Estoque
    {
        private readonly int _limiteBaixo;

        public int Saldo { get; private set; }

        // Evento disparado quando o saldo chega no limite.
        public event EventHandler<int> EstoqueBaixo;

        public Estoque(int saldoInicial, int limiteBaixo)
        {
            Saldo = saldoInicial;
            _limiteBaixo = limiteBaixo;
        }

        // Método ligado ao evento PedidoCriado no Program.
        public void AoPedidoCriado(object sender, PedidoEventArgs e)
        {
            // Se não tem saldo suficiente, o pedido é recusado com exceção.
            if (e.Pedido.Quantidade > Saldo)
            {
                throw new InvalidOperationException(
                    $"Estoque insuficiente para o pedido {e.Pedido.Id}: precisa de {e.Pedido.Quantidade}, saldo {Saldo}.");
            }

            Saldo -= e.Pedido.Quantidade;

            if (Saldo <= _limiteBaixo)
            {
                EstoqueBaixo?.Invoke(this, Saldo);
            }
        }
    }
}
