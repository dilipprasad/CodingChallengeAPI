using CodingChallenge.Models;
using System.Collections;

namespace CodingChallenge.Business.Interfaces
{
    public interface IAlgoChallengeBusinessProvider
    {
        Task<ArrayList> SolveChallenge(int totalNumberOfRecords, ShapeAndColor shapeandColorObj);
    }
}
