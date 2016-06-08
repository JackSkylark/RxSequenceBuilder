using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RxSequenceBuilder.Experiment.Models
{
    public class ExampleClass1
    {
        public ExampleClass1(string name, string group)
        {
            Name = name;
            Group = group;
        }

        public string Name { get; set; }
        public string Group { get; set; }
    }

    public class ExampleClass1Factory : IDisposable
    {
        private bool _isActive = true;
        private readonly Random _randomNumberGenerator;

        public ExampleClass1Factory(object observer)
        {
            Task.Factory.StartNew(Generate, observer);
            _randomNumberGenerator = new Random();
        }

        public void Generate(object observer)
        {
            var exampleClass1Observer = (IObserver<ExampleClass1>)observer;
            var randomNumberGenerator = new Random();
            while (_isActive)
            {
                var model = new ExampleClass1("Test" + randomNumberGenerator.Next(0, 100), GetGroup());
                exampleClass1Observer.OnNext(model);
                Thread.Sleep(100);
            }
        }

        private string GetGroup()
        {
            var num = _randomNumberGenerator.Next(0, 10);
            return num <= 5 ? "Group_1" : "Group_2";
        }

        public void Dispose()
        {
            _isActive = false;
        }

        public static IDisposable Subscribe(object observer)
        {
            var factory = new ExampleClass1Factory(observer);
            return factory;
        }
    }
}
