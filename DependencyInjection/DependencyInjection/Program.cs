using System;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            Console.WriteLine("Console Logger 생성 됨.");
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class TransientObject
    {
        ILogger _logger;
        ScopedObject _scoped;

        public TransientObject(ILogger logger, ScopedObject scoped)
        {
            _logger = logger;
            _scoped = scoped;

            _logger.Log("TransientObject 생성 됨.");
        }
    }


    public class ScopedObject
    {
        ILogger _logger;

        public ScopedObject(ILogger logger)
        {
            _logger = logger;

            _logger.Log("ScopedObject 생성 됨.");
        }
    }

    static class Program
    {
        public static void Main(string[] args)
        {
            ServiceCollection collection = new();
            
            collection.AddSingleton<ILogger, ConsoleLogger>();
            collection.AddTransient<TransientObject>();
            collection.AddScoped<ScopedObject>();

            ServiceProvider provider = collection.BuildServiceProvider();

            // loggerA와, loggerB는 서로 같은 인스턴스.
            Console.WriteLine("Singleton");

            ILogger loggerA = provider.GetService<ILogger>();
            ILogger loggerB = provider.GetService<ILogger>();

            // transientA, transientB 서로 다른 인스턴스.
            Console.WriteLine("Transient");

            TransientObject transientA = provider.GetService<TransientObject>();
            TransientObject transientB = provider.GetService<TransientObject>();

            using (IServiceScope scope = provider.CreateScope())
            {// scopedA와, scopedB는 서로 같은 인스턴스.
                Console.WriteLine("Scoped 0");

                ScopedObject scopedA = scope.ServiceProvider.GetService<ScopedObject>();
                ScopedObject scopedB = scope.ServiceProvider.GetService<ScopedObject>();
            }

            using (IServiceScope scope = provider.CreateScope())
            {// scopedC와, scopedD는 서로 같은 인스턴스.
                Console.WriteLine("Scoped 1");

                ScopedObject scopedC = scope.ServiceProvider.GetService<ScopedObject>();
                ScopedObject scopedD = scope.ServiceProvider.GetService<ScopedObject>();
            }
        }
    }
}
