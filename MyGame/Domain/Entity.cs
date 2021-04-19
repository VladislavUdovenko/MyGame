using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Domain
{
    public class Entity
    {
        //public int CurrentAnumation { get; set; }
        //public int CurrentFrame { get; set; }

        //public int IdleFrames { get; set; }
        //public int RunFrames { get; set; }
        //public int AtackFrames { get; set; }
        //public int DeathFrames { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }

        public bool IsMoving { get; set; }

        public Image CurrentSprite { get; set; }
        // public int Size { get; set; }

        public Entity(int x, int y, Image sprite)
        {
            X = x;
            Y = y;
            CurrentSprite = sprite;
            //Size = 50;
            //IdleFrames = idleFrames;
            //RunFrames = runFrames;
            //AtackFrames = atackFrames;
            //DeathFrames = deathFrames;
            //CurrentAnumation = 0;
            //CurrentFrame = 0;
        }

        public void Move()
        {
            X += DirectionX;
            Y += DirectionY;
        }

        //public void PlayAnimation(Graphics graphics)
        //{
        //    graphics.DrawImage(SpriteSheet, new Rectangle(new Point(X, Y), new Size(Size, Size)), 0, 0, Size, Size, GraphicsUnit.Pixel);
        //    if (CurrentFrame < IdleFrames - 1)
        //        CurrentFrame++;
        //    else
        //        CurrentFrame = 0;
        //}
    }
}
