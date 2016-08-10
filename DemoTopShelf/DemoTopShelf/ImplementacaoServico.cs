using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace DemoTopShelf
{
    public class ImplementacaoServico : ServiceControl
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ImplementacaoServico()
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
                    Console.WriteLine("It is {0} and all is well", DateTime.Now);
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
