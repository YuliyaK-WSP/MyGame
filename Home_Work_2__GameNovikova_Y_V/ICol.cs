using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    interface ICol
    {
        //Закладывает поведение, по которому два объекта, поддерживающие его,
        //могут определить, столкнулись ли они

        bool Collision(ICol obj);
        Rectangle Rect { get; }

    }
}
