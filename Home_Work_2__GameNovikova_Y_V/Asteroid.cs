using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    class Asteroid : BaseObject
    {
        public int Power { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }
        Image img = Image.FromFile(@"..\..\Resource\asteroid.png");
        public override void Draw()
        {
            
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X - 3 /*- Dir.X*/;

            if (Pos.X < 0) 
            {
                Pos.X = Game.Width + Size.Width;

                var rnd = new Random();
                Pos.Y = rnd.Next(0, Game.Height);


            } 

        }

    }
}
