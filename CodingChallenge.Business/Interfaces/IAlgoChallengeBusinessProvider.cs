namespace CodingChallenge.Business.Interfaces
{
    public interface IAlgoChallengeBusinessProvider
    {
        public Task SolveChallenge(ref string[] shape, ref string[] color);
    }
}
