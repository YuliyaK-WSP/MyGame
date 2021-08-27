using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    
    class Program
    {
       

        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 1000;
            form.Height = 600;
            Console.WriteLine("Начать игру? yes/no");
            String userPr = Console.ReadLine();
            if(userPr == "yes")
            {
                Game.Init(form);
                form.Show();
                Game.Draw();
                Application.Run(form);
            }
            else if(userPr == "no")
            {
                Console.WriteLine("Жаль, очень интересная игра");
            }
            else
            {
                Console.WriteLine("Вы ввели что-то не понятное мне, повторите попытку:)");
            }
            
            
           
            
            

        }
    }
}
