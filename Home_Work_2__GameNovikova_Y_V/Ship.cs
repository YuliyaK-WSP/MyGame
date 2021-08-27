using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace MyGame
{
    class Ship :BaseObject
    {
        private int _energy = 100;
        private int _count = 0;
        public int Count => _count;

        public int Energy => _energy;

        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        public void CountPlus()
        {
            _count++;
        }
        public static event Message MessageDie;
        public void Die()
        {
            MessageDie?.Invoke();
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        Image img = Image.FromFile(@"..\..\Resource\ship.png");
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        
    }
}
