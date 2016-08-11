using Topshelf;

namespace DemoTopShelf
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(configurator =>
            {
                configurator.Service<EmailServico>(s =>
                {
                    s.ConstructUsing(name => new EmailServico());
                    s.WhenStarted((service, control) => service.Start(control));
                    s.WhenStopped((service, control) => service.Stop(control));
                });
                configurator.RunAsLocalSystem();

                //It is recommended that service names not contains spaces or other whitespace characters.
                configurator.SetServiceName("MyServiceByTopshelf");
                configurator.SetDescription("Sample Topshelf Host");
                configurator.SetDisplayName("Stuff");
            });
        }
    }
}
