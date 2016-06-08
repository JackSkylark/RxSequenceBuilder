using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxSequenceBuilder.Test.Models
{
    public class ExampleClass1
    {
        public ExampleClass1(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class ExampleClass1Factory : IDisposable
    {
        private bool isActive = true;

        public ExampleClass1Factory(object observer)
        {
            Task.Factory.StartNew(Generate, observer);
        }

        public void Generate(object observer)
        {
            var exampleClass1Observer = (IObserver<ExampleClass1>)observer;
            var randomNumberGenerator = new Random();
            while (isActive)
            {
                var model = new ExampleClass1("Test" + randomNumberGenerator.Next(0, 100));
                exampleClass1Observer.OnNext(model);
                Thread.Sleep(3000);
            }
        }

        public void Dispose()
        {
            isActive = false;
        }

        public static IDisposable Subscribe(object observer)
        {
            var factory = new ExampleClass1Factory(observer);
            return factory;
        }
    }
}
