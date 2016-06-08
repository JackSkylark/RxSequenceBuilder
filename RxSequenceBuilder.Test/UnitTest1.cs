using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RxSequenceBuilder.Test.Models;

namespace RxSequenceBuilder.Test
{
    [TestClass]
    public class SequenceTests
    {

        public IEnumerable<ExampleClass1> ExampleClassOnes { get; set; }

        public SequenceTests()
        {
            
        }

        [TestMethod]
        public void TransformTest()
        {
            var transform = new Transformer<ExampleClass1, ExampleClass2>(
                ExampleClass1Factory.Subscribe, 
                example1 => new ExampleClass2(example1.Name + "_ex2")
            );

            var observable = transform.TransformObv;
            observable.Subscribe(x => Console.WriteLine(x.Name));
            Console.ReadLine();
        }
    }
}
