using Apps.Domain.Common.Models;
using Apps.Domain.Host.ValueObjects;

namespace Apps.Domain.Common.ValueObjects;

public sealed class AverageRating : ValueObject
{
  public double Value { get; private set;}
  public int NumRatings { get; private set; }
  private AverageRating(double value, int numRatings)
  {
    Value = value;
    NumRatings = numRatings;
  }
  public static AverageRating CreateNew(double rating = 0, int numRatings = 0)
  {
    return new AverageRating(rating, numRatings);
  }

  public void AddNewRating(Rating rating)
  {
    Value = ((Value * NumRatings) + rating.Value) / ++NumRatings;
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
    yield return NumRatings;
  }
  #pragma warning disable CS8618
  private AverageRating(){}
  #pragma warning restore CS8618

}