using System;

namespace MonitorPedidos
{
    internal class Program
    {
        static void Main()
        {
            var servico = new ServicoPedidos();
            var estoque = new Estoque(saldoInicial: 10, limiteBaixo: 3);
            var notificador = new Notificador(Console.WriteLine);
            var cozinha = new Cozinha();
            var entregador = new Entregador();

            // Estoque primeiro: se recusar (exceção), os próximos assinantes não executam.
            servico.PedidoCriado += estoque.AoPedidoCriado;
            servico.PedidoCriado += notificador.AoPedidoCriado;
            servico.PedidoCriado += cozinha.AoPedidoCriado;

            cozinha.PreparoIniciado += notificador.AoPreparoIniciado;
            cozinha.PedidoPronto += notificador.AoPedidoPronto;
            cozinha.PedidoPronto += entregador.AoPedidoPronto;

            entregador.SaiuParaEntrega += notificador.AoSaiuParaEntrega;
            entregador.PedidoEntregue += notificador.AoPedidoEntregue;

            estoque.EstoqueBaixo += notificador.AoEstoqueBaixo;

            servico.Criar("Ana", "Pizza", 3);
            servico.Criar("Bruno", "Hambúrguer", 5); // saldo cai para 2 -> estoque baixo

            try
            {
                servico.Criar("Carla", "Pizza", 8); // maior que o saldo: recusado
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Pedido recusado: {ex.Message}");
            }

            // async void: sem isso o app fecha antes de preparo e entrega terminarem.
            Console.WriteLine("Aguardando preparo e entrega... (Enter para sair)");
            Console.ReadLine();
        }
    }
}
