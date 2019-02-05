using System;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
        public abstract Matrix Matrix { get; }
        public abstract string WriteToString();

    	public abstract object Clone();
    	
    	#region Equals implementation
    	public override bool Equals(object obj)
		{
			var other = obj as SvgTransform;
			if (other == null)
				return false;
			
			var thisMatrix = Matrix.Elements;
			var otherMatrix = other.Matrix.Elements;
			
			for (var i = 0; i < 6; i++) 
			{
				if(Math.Abs(thisMatrix[i] - otherMatrix[i]) > 0.01)
					return false;
			}
			
			return true;
		}
    	
    	public override int GetHashCode()
		{
    		var hashCode = Matrix.GetHashCode();
			return hashCode;
		}

    	
		public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
    	
		public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
		{
			return !(lhs == rhs);
		}
    	#endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}