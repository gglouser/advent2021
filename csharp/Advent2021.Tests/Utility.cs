using System.Collections.Generic;
using System.IO;

namespace Advent2021.Tests
{
    // Why is this not in stdlib? Oh well, thanks again stackoverflow.
    public static class LineSplitter
    {
        public static IEnumerable<string> ReadLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
