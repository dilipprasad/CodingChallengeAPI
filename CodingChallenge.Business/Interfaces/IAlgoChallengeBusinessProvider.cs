using System.Collections;

namespace CodingChallenge.Business.Interfaces
{
    public interface IAlgoChallengeBusinessProvider
    {
        public Task SolveChallenge(ref ArrayList shape, ref ArrayList color);
    }
}
