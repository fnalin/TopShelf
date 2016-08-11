using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace DemoTopShelf
{
    public class EmailServico : ServiceControl
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public EmailServico()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool Start(HostControl hostControl)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    var envio = new EnvioDeEmail();
                    var destinatarios = new string[] { "xx@gmail.com", "xxxx@sawluz.com.br" };
                    envio.EnviarMensagemAsync(destinatarios.ToList());
                    Console.WriteLine($"E-mail disparado às {DateTime.Now}");
                }
            }, _cancellationTokenSource.Token);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            return true;
        }
    }
}
