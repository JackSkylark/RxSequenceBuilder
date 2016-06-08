using System;
using System.Security.Cryptography.X509Certificates;

namespace RxSequenceBuilder.Experiment.Steps
{
    public class Step1:Step<Step1.Record>
    {
        public class Record
        {
            public string Id { get; set; }
            public DateTime TimeStamp { get; set; }

            public Record(string id)
            {
                Id = id;
                TimeStamp = DateTime.Now;
            }

            public override string ToString()
            {
                return $"RecordId: {Id}\nTimestamp: {TimeStamp}";
            }
        }

        public Step1(Func<IObserver<Record>, IDisposable> feeder) 
            : base(feeder)
        {
            RecordProcessingObservable.Subscribe(x => Console.WriteLine(x.ToString()));
        }
    }
}
