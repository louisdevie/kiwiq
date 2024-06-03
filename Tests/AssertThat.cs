using System.Text.RegularExpressions;

namespace KiwiQuery.Tests
{
    public static class AssertThat
    {
        public static void GroupsAreEqual(Match match, params int[] groups)
        {
            foreach (int i in groups.Skip(1))
            {
                Assert.Equal(match.Groups[groups[0]].Value, match.Groups[i].Value);
            }
        }

        public static void GroupsEqualUnordered(string[] expected, Match match, params int[] groups)
        {
            List<string> remaining = expected.ToList();
            foreach (int i in groups)
            {
                string groupValue = match.Groups[i].Value;
                Assert.True(remaining.Remove(groupValue));
            }
            Assert.Empty(remaining);
        }
    }
}