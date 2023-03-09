using System.IO;
using System.Threading.Tasks;

namespace FlaskSharp
{
    internal static class StreamUtils
    {
        public static async Task ReadEnoughAsync(this Stream stream, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead);
                if (read == 0)
                    throw new EndOfStreamException();

                totalRead += read;
            }
        }
    }
}
