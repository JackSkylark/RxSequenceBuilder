using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RxSequenceBuilder.Experiment.Models;
using RxSequenceBuilder.Experiment.Steps;

namespace RxSequenceBuilder.Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            SequenceTest();
            Console.ReadLine();
        }

        public static void SequenceTest()
        {
            var recordGeneratorObv = Observable.Create<ExampleClass1>(x => ExampleClass1Factory.Subscribe(x));

            var transformedRecordsStream = SequenceBuilder.Transform(recordGeneratorObv,
                model => new ExampleClass2(model.Name, model.Group));

            var batchedRecordsStream = SequenceBuilder.Batch(transformedRecordsStream, x => x.Group);

            var secondtransformedRecordsStream = SequenceBuilder.Transform<KeyValuePair<string, IList<ExampleClass2>>, IList<ExampleClass1>>(batchedRecordsStream, 
                x => x.Value.Select(y => new ExampleClass1(y.Name, y.Group)).ToList());

            secondtransformedRecordsStream.Subscribe(x =>
            {
                Console.WriteLine($"Processed {x.Count} Records");
            });

        }


        public static void TransformTest()
        {
            var recordGeneratorObv = Observable.Create<ExampleClass1>(x => ExampleClass1Factory.Subscribe(x));

            var transformedRecordsStream = SequenceBuilder.Transform(recordGeneratorObv,
                model => new ExampleClass2(model.Name, model.Group));

            transformedRecordsStream.Subscribe(x => Console.WriteLine(x.Name));
        }

        public static void BatchTest()
        {
            var recordGeneratorObv = Observable.Create<ExampleClass1>(x => ExampleClass1Factory.Subscribe(x));
            var batchedRecordsStream = SequenceBuilder.Batch(recordGeneratorObv, x => x.Group);
            batchedRecordsStream.Subscribe(x =>
            {
                Console.WriteLine($"Processing {x.Value.Count} records for batch group: {x.Key}");
            });
        }
    }
}
