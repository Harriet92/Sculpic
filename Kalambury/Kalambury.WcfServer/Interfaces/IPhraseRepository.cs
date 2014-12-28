using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    public interface IPhraseRepository: IRepository<Phrase>
    {
        Phrase GetRandomPhrase();
    }
}
