using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace event_handler.Objects
{
    internal class Player : BaseObject
    {
        public Action<Marker> OnMarkerOverlap;
        public Action<Marker> OnPlayerOverlap;

        public Player(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.DeepPink), -15, -15, 30, 30);
            g.DrawEllipse(new Pen(Color.HotPink, 2), -15, -15, 30, 30);
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 25, 0);
        }
 
        public override void Overlaps(BaseObject obj)
        {
            base.Overlaps(obj);
            if (obj is Marker marker)
            {
                // Безопасный вызов события
                OnMarkerOverlap?.Invoke(marker);
            }
        }

    }
}
