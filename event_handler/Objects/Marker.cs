using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace event_handler.Objects
{
    internal class Marker : BaseObject
    {
        public Marker(float x, float y, float agnel) : base(x, y, agnel) 
        {
        }    
        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Red), -3, -3, 6, 6);
            g.DrawEllipse(new Pen(Color.Red), -6, -6, 12, 12);
            g.DrawEllipse(new Pen(Color.Red), -10, -10, 20, 20);
        }
    }
}
