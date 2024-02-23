using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class Vector3
    {
        public float x; public float y; public float z;
        public Vector3() { 
           x = 0; y = 0; z = 0;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }


    }

    public class Vector2
    {
        public int x; public int y;

        public Vector2()
        {
            x = 0; y = 0; 
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
        public Vector2 GetDifference(Vector2 vecTarget)
        {
            return new Vector2(vecTarget.x - x, vecTarget.y - y);
        }
        public double DistTo(Vector2 vec)
        {
            Vector2 vecDelta = new Vector2(x - vec.x, y - vec.y);
            return vecDelta.Length();
        }
        public float DotProduct(Vector2 vecDot)
        {
            return (x * vecDot.x) + (y * vecDot.y);
        }
        public float LengthSqr() 
        {
            return DotProduct(new Vector2(x,y));
        }
        public double Length()
        {
             return Math.Sqrt(LengthSqr());
        }

        public static Vector2 operator +(Vector2 This, Vector2 Other)
        {
            return new Vector2(This.x + Other.x, This.y + Other.y);
        }

        public static bool operator ==(Vector2 This, Vector2 Other)
        {
            return This.x == Other.x && This.y == Other.y;
        }
        public static bool operator !=(Vector2 This, Vector2 Other)
        {
            return This.x != Other.x || This.y != Other.y;
        }


    }
}
