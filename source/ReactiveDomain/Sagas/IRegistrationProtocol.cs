using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public interface IRegistrationProtocol<TDocument>
        where TDocument : IDocument {
        void Register(Uri uri, Func<TDocument, CancellationToken, Task<bool>> filter);
        bool IsRegistered(Uri uri);
        void Revoke(Uri uri);
    }
}