namespace xxtest;
public static class UserGenerator
{
  static public User GenerateRandom()
  {
    return new User(1,"First", "Last");
  }
}