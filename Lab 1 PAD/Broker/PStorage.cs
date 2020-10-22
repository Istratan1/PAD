
using System.Collections.Concurrent;

namespace Broker
{
    static class PStorage
    {
        private static ConcurrentQueue<PHandler> _payloads;


        static PStorage()
        {
            _payloads = new ConcurrentQueue<PHandler>();
        }

        public static void Add(PHandler pHandler)
        {
            _payloads.Enqueue(pHandler);
        }

        public static PHandler getNext()
        {
            PHandler pHandler = null;

            _payloads.TryDequeue(out pHandler);

            return pHandler;
        }

        public static bool isEmpty()
        {
            return _payloads.IsEmpty;
        }

    }


}
