using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileRateLimiter
{
    public class ExtentedBinaryReader : BinaryReader
    {
        #region FactoryMethods

        public static ExtentedBinaryReader CreateInstance(Stream input, Encoding encoding)
        {
            return new ExtentedBinaryReader(input, encoding);
        }

        public static ExtentedBinaryReader CreateInstance(Stream input)
        {
            return new ExtentedBinaryReader(input);
        }

        public static ExtentedBinaryReader CreateInstance(Stream input, Encoding encoding, bool leaveOpen)
        {
            return new ExtentedBinaryReader(input, encoding, leaveOpen);
        }


        #endregion

        #region Constructors

        private ExtentedBinaryReader(Stream input) : base(input)
        {
        }

        private ExtentedBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        private ExtentedBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }
        #endregion
        

        private List<IFilter> _filters = new List<IFilter>();

        public void AddFilter(IFilter filter)
        {
            if(!_filters.Contains(filter))
                _filters.Add(filter);
        }

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