using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace event_handler.Objects
{
    internal class DisappearingObject : BaseObject
    {

        Random random = new Random();
        PictureBox pbMain;
        System.Timers.Timer timer;

        public double RemainingTime { get; private set; } // Оставшееся время

        public DisappearingObject(float x, float y, float angle, PictureBox pbMain) : base(x, y, angle)
        {
            this.pbMain = pbMain;
            ResetPositionWithinPictureBox();

            // Установка начального значения времени
            RemainingTime = 5;

            // Создаем таймер
            timer = new System.Timers.Timer(1000); // Интервал - 1 секунда
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Уменьшаем оставшееся время на 1 секунду
            RemainingTime--;

            // Если время закончилось, останавливаем таймер
            if (RemainingTime <= 0)
            {
                timer.Stop();
                // Сбрасываем позицию объекта и перезапускаем таймер
                ResetPositionWithinPictureBox();
                RemainingTime = 5;
                timer.Start();
            }

            // Обновляем PictureBox
            pbMain.Invoke((MethodInvoker)delegate
            {
                pbMain.Invalidate();
            });
        }

        private void ResetPositionWithinPictureBox()
        {
            // Получаем границы PictureBox
            Rectangle pbBounds = pbMain.Bounds;

            // Генерируем случайные координаты в пределах границ PictureBox
            X = random.Next(pbBounds.Left + 20, pbBounds.Right - 20);
            Y = random.Next(pbBounds.Top + 20, pbBounds.Bottom - 20);

            // Сбрасываем время
            RemainingTime = 15;
        }

        public void ResetPosition()
        {
            X = random.Next(20, 780); // Перемещение в новую случайную позицию
            Y = random.Next(20, 580);
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Gray), -10, -10, 20, 20);
            g.DrawEllipse(new Pen(Color.Gray), -10, -10, 20, 20);

            var stringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            g.DrawString(
                RemainingTime.ToString(), // Оставшееся время
                new Font("Verdana", 8),
                new SolidBrush(Color.Green),
                10,
                10
            );
        }

        public override void Overlaps(BaseObject obj)
        {
            base.Overlaps(obj);
            if (obj is Player)
            {
                // Генерируем новое случайное местоположение
                ResetPositionWithinPictureBox();
            }
        }
    }
}
