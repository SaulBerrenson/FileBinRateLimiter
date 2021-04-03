using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileRateLimiter
{
    public class ExtentedBinaryReader : BinaryReader
    {
        public static ExtentedBinaryReader CreateInstance(Stream input, Encoding encoding, bool leaveOpen)
        {
            return new ExtentedBinaryReader(input, encoding, leaveOpen);
        }

        public ExtentedBinaryReader(Stream input) : base(input)
        {
        }

        public ExtentedBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        private ExtentedBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        private List<IFilter> _filters = new List<IFilter>();

        public override int Read(byte[] buffer, int index, int count)
        {
            var output = base.Read(buffer, index, count);
            MakeFilters<byte>(buffer: buffer);
            return output;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var output = base.Read(buffer, index, count);
            MakeFilters<char>(buffer);
            return output;
        }

        public override byte[] ReadBytes(int count)
        {
            var bytes = base.ReadBytes(count);
            MakeFilters<byte>(bytes);
            return bytes;
        }

        private void MakeFilters<T>(ArraySegment<T> buffer)
        {
            foreach (var filter in _filters)
                filter.Process(buffer);
        }
    }
}