using CodingChallenge.Models;
using System.Collections;

namespace CodingChallenge.Business.Interfaces
{
    public interface IAlgoChallengeBusinessProvider
    {
        Task<ShapeObjects[]> SolveChallenge(int totalNumberOfRecords, ArrayList shapeObj, Hashtable ColorWithCounter);
    }
}
