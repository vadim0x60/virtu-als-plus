using System;

public interface IRespectfulObservable<T>: IObservable<T> {
    void Unsubscribe(IObserver<T> subscribee);
}

public class Unsubscriber<T> : IDisposable
{
   private IObserver<T> subscriber;
   private IRespectfulObservable<T> subscribee;

   public Unsubscriber(IObserver<T> subscriber, IRespectfulObservable<T> subscribee) {
       this.subscriber = subscriber;
       this.subscribee = subscribee;
   }

   public void Dispose()
   {
      subscribee.Unsubscribe(subscriber);
   }
}