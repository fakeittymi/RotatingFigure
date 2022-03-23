using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rotating
{
    public partial class Form2 : Form
    {
        SolidBrush bgcol = new SolidBrush(Color.White);//цвет фона 

        int cnt1 = 0; //счетчик таймера
        float speedE = 0;//переменная скорости вращения
        float newSpeed = 0; //относительная скорость из trackbar
        public bool stopped = false; //флаг закрытия формы
        int pbw = 0; //picturebox1.Width
        int pbh = 0; //picturebox1.Height
        Bitmap bmp; //объект изображения для отрисовки
        bool pause = false; //флаг паузы
        public Form2()
        {
            InitializeComponent();
            newSpeed = (float)trackBar1.Value / 10;
            pbw = pictureBox1.Width;
            pbh = pictureBox1.Height;           
            thread = new Thread(() =>
            {               
                while (true)
                {
                    cnt1++;//увеличиваем счетчик
                    speedE += newSpeed;
                    if (cnt1 == 3600) { cnt1 = 0; speedE = 0; }//по завершении анимации обнуляем значения переменных                 
                    bmp = RotatingFigures(); //вызываем функцию рисования фигур
                    Thread.Sleep(timer1.Interval);
                }           
            });
            thread.Start();
        }
        Thread thread; 

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = bmp;
        }

        private void SetPolygon(PointF center, int vertexes, float radius, Graphics graphics, Bitmap bmp, float speed) // создаем функцию, которая рисует многоугольник с заданным кол-вом вершин по радиусу описанной окружности и координатам центра окружности
        {
            double angle = Math.PI * 2 / vertexes; //угол правильного многоугольника вычисляется по формуле 2*pi/(кол-во вершин)
            var points = Enumerable.Range(0, vertexes).Select(i => PointF.Add(center, new SizeF((float)Math.Sin(i * angle) * radius, (float)Math.Cos(i * angle) * radius)));//создаем массив точек, по которым будем строить многоугольник
            SolidBrush transparent = new SolidBrush(Color.FromArgb(30, 255, 0, 0));//задаем полупрозрачную заливку

            graphics.TranslateTransform((float)pbw / 2, (float)pbh / 2);//смещаем начало координат в центр pictureBox
            graphics.RotateTransform(speed);//поворачиваем графику на определенный угол
            graphics.FillPolygon(transparent, points.ToArray()); //заполняем многоугольник
        }

        private Bitmap RotatingFigures()
        {
            Bitmap btmp = new Bitmap(pbw, pbh);
            Graphics e = Graphics.FromImage(btmp);

            e.FillRectangle(bgcol, 0, 0, 500, 500);//заливаем фон
            SetPolygon(new PointF(0, 0), 7, 120, e, btmp, speedE);
            return btmp; //переносим рисунок из буфера на pictureBox
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            newSpeed = (float)trackBar1.Value/10;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopped = true;
            if (pause)
            {
                thread.Resume();
                thread.Abort();
            }
            else
            {
                thread.Abort();
            }   
            thread.Join();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pause)
            {
                thread.Resume();
                pause = !pause;
            }
            else
            {
                thread.Suspend();
                pause = !pause;
            }
        }
    }
}
