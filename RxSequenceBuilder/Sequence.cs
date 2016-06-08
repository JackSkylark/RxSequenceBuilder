using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxSequenceBuilder
{

    public static class SequenceBuilder
    {

        public static IObservable<KeyValuePair<string, IList<T>>> Batch<T>(IObservable<T> recordStream,
            Func<T, string> groupKeySelector)
        {
            var batchGroupObv = recordStream.GroupBy(groupKeySelector, x => x);

            // Using a subject because it keeping track of a batch event.
            var returnObv = new Subject<KeyValuePair<string, IList<T>>>();

            batchGroupObv.Subscribe(x =>
            {
                var sourcePub = x.Publish().RefCount();
                var output = sourcePub.Buffer(TimeSpan.FromSeconds(10));
                output.Select(y => new KeyValuePair<string, IList<T>>(x.Key, y))
                .Subscribe(y =>
                {
                    returnObv.OnNext(y);
                });
            });

            return returnObv;
        }

        public static IObservable<TOut> Transform<TIn, TOut>(
            IObservable<TIn> recordStream,
            Func<TIn, TOut> transformFunc)
        {
            return recordStream.Select(transformFunc);
        }
    }

    public class Sequence
    {
        public Sequence()
        {
        }

    }

    public abstract class Step<TRecord>
    {
        protected Step(Func<IObserver<TRecord>, IDisposable> feeder)
        {
            RecordProcessingObservable = Observable.Create(feeder);
        }

        public IObservable<TRecord> RecordProcessingObservable { get; set; }
    }

    public class Batch<T>
    {
        public Batch(Func<IObserver<T>, IDisposable> generator, Func<T, string> groupKeySelector)
        {
            var batchRecordsObv = Observable.Create(generator);
            var batchGroupObv = batchRecordsObv.GroupBy(groupKeySelector, x => x);
        }
    }

    public class Transformer<TIn, TOut>
    {
        public Transformer(Func<IObserver<TIn>, IDisposable> transformIn, Func<TIn, TOut> transformFunc)
        {

            TransformObv = Observable.Create(transformIn).Select(transformFunc);
        }

        public IObservable<TOut> TransformObv;
    }
}
