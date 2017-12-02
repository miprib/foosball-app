using System.Drawing;

namespace xamarin_android.Recognition
{
    class Coordinates
    {
        public int x;
        public int y;

        public Coordinates()
        {
            this.x = 0;
            this.y = 0;
        }
        public Coordinates(float xaxis, float yaxis)
        {
            this.x = (int)xaxis;
            this.y = (int)yaxis;
        }

        public Coordinates(PointF pointF)
        {
            this.x = (int)pointF.X;
            this.y = (int)pointF.Y;
        }

        public override string ToString()
        {
            string coo = "Coordinates: x " + this.x + "    y " + this.y;
            return coo;
        }
    }
}