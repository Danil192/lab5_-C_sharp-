using event_handler.Objects;
using System;

namespace event_handler
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        DisappearingObject disappearingObject;
        int score = 0;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            objects.Add(player);
            disappearingObject = new DisappearingObject(pbMain.Width / 2, pbMain.Height / 2, 0, pbMain);
            objects.Add(disappearingObject);
            marker = new Marker(pbMain.Width / 2 + 25, pbMain.Height / 2 + 50, 0);
            objects.Add(marker);

            player.OnOverlap += (p, obj) =>
            {
                if (obj is DisappearingObject)
                {
                    score++;
                    Invoke((MethodInvoker)(() => lbScore.Text = score.ToString()));
                    (obj as DisappearingObject).ResetPosition();
                    objects.Remove(obj);
                    CreateNewDisappearingObject();
                }
                txtlog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtlog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
        }

        private void CreateNewDisappearingObject()
        {
            disappearingObject = new DisappearingObject(
                random.Next(20, pbMain.Width - 20),
                random.Next(20, pbMain.Height - 20),
                0,
                pbMain
            );
            objects.Add(disappearingObject);
        }


        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            UpdatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlaps(obj);
                    obj.Overlaps(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        public void UpdatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void pbMain_Click(object sender, EventArgs e)
        {
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}