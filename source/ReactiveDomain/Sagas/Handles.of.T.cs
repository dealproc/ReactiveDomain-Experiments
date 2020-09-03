using System.Threading;
using System.Threading.Tasks;

namespace ReactiveDomain.Sagas {
    public interface Handles<T> where T : IDocument {
        Task HandleAsync(T document, CancellationToken token = default);
    }
}