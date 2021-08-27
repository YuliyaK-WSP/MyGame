using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using Color = System.Drawing.Color;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;
using FontFamily = System.Drawing.FontFamily;



namespace MyGame
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(50, 50));
        private static List<Bullet> _bullets = new List<Bullet>();
        private static List<Asteroid> _asteroids = new List<Asteroid>(10);

        private static Timer _timer = new Timer { Interval = 100 };
        public static Random Rnd = new Random();
        public static void Init(Form form)
        {

            try
            {
                // Графическое устройство для вывода графики            
                Graphics g;
                // Предоставляет доступ к главному буферу графического контекста для текущего приложения
                _context = BufferedGraphicsManager.Current;


                g = form.CreateGraphics();
                // Создаем объект (поверхность рисования) и связываем его с формой
                // Запоминаем размеры формы


                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

                if (Width > 1000 || Height > 1000 || Width < 0 || Height < 0)
                {
                    ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException();
                    exception.Source = "form";
                    throw exception;
                }
                //Application.Run(form);
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("Error: {0}", outOfRange.Message);
            }


            form.KeyDown += Form_KeyDown;
            Load();
            //Timer timer = new Timer { Interval = 10 };
            _timer.Start();
            _timer.Tick += Timer_Tick;
            Ship.MessageDie += Finish;
        }
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();

        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {

                a?.Draw();
            }
            foreach (Bullet b in _bullets) b.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Count:" + _ship.Count, SystemFonts.DefaultFont, Brushes.White, 0, 15);


            Buffer.Render();

        }
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }
        public static BaseObject[] _objs;
        public static void Load()
        {
            _objs = new BaseObject[30];


            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));

            }
            for (var i = 0; i < 10; i++)
            {
                int r = rnd.Next(5, Game.Height);

                _asteroids.Add(new Asteroid(new Point(1000 - i, rnd.Next(0, Game.Width)), new Point(-r, r), new Size(30, 30)));

            }

        }


        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (Bullet b in _bullets) b.Update();
            foreach (Asteroid a in _asteroids) a?.Update();
            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                for (int j = 0; j < _bullets.Count; j++)
                    if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids[i] = null;
                        _bullets.RemoveAt(j);
                        j--;
                        _ship.CountPlus();
                    }
                if (_asteroids[i] == null || !_ship.Collision(_asteroids[i])) continue;
                _ship.EnergyLow(Rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.Energy <= 0) _ship.Die();
            }
            bool asteroidsIsNull = true;
            for (int i = 0; i < _asteroids.Count; i++)
                if (_asteroids[i] != null)
                {
                    asteroidsIsNull = false;
                }
            // Переопределение коллекции с увеличением элементов на 1
            if (asteroidsIsNull == true)
            {
                _asteroids = new List<Asteroid>(_asteroids.Capacity + 1);
                var rnd = new Random();
                for (var i = 0; i < _asteroids.Capacity; i++)
                {
                    
                    int r = rnd.Next(5, Game.Height);

                    _asteroids.Add(new Asteroid(new Point(1000 - i, rnd.Next(0, Game.Width)), new Point(-r, r), new Size(30, 30)));

                }
            }




        }
    }
}
