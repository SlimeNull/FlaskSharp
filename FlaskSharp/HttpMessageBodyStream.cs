using System.Text;

namespace FlaskSharp
{
    public sealed class HttpMessageBodyStream : MemoryStream
    {
        public HttpMessage Owner { get; }
        public HttpMessageBodyStream(HttpMessage owner)
        {
            Owner = owner;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            Owner.Headers.ContentLength = Length;
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            base.Write(buffer);
            Owner.Headers.ContentLength = Length;
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await base.WriteAsync(buffer, offset, count, cancellationToken);
            Owner.Headers.ContentLength = Length;
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await base.WriteAsync(buffer, cancellationToken);
            Owner.Headers.ContentLength = Length;
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
            Owner.Headers.ContentLength = Length;
        }

        public string GetText()
        {
            return Encoding.UTF8.GetString(ToArray());
        }
    }
}