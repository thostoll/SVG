using System;

namespace Svg.Exceptions
{
    public class SvgException : FormatException
    {
        public SvgException(string message) : base(message)
        {
        }
    }

    public class SvgIdException : FormatException
    {
        public SvgIdException(string message)
            : base(message)
        {
        }
    }

    public class SvgIdExistsException : SvgIdException
    {
        public SvgIdExistsException(string message)
            : base(message)
        {
        }
    }

    public class SvgIdWrongFormatException : SvgIdException
    {
        public SvgIdWrongFormatException(string message)
            : base(message)
        {
        }
    }
}