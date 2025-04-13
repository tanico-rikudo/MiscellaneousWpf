
namespace LiveChartPlay.Services.Core
{

    public interface IMessengerService
    {
        void Publish<T>(T payload);
        void Subscribe<T>(Action<T> action);

    }
    public class MessengerService: IMessengerService
    {

        private readonly IEventAggregator _eventAggregator;

        public MessengerService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Publish<T>(T payload)
        {
            _eventAggregator.GetEvent<PubSubEvent<T>>().Publish(payload);
        }

        public void Subscribe<T>(Action<T> action)
        {
            // ThreadOption.UIThread, UI スレッドで呼び出される（ViewModel で ReactiveProperty に触れるなら必須）
            // keepSubscriberReferenceAlive: true	GC によって購読が破棄されないようにするため
            _eventAggregator.GetEvent<PubSubEvent<T>>().Subscribe(action,
                ThreadOption.UIThread,
                keepSubscriberReferenceAlive: true);
        }


    }
}





