using CodingChallenge.Models;
using System.Collections;

namespace CodingChallenge.Business.Interfaces
{
    public interface IAlgoChallengeBusinessProvider
    {
        Task<ColorObject[]> SolveChallenge(int totalNumberOfRecords, ShapeAndColor shapeandColorObj);
    }
}
