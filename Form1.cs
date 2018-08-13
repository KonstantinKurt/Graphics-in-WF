using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WionForms_Paint
{
    public partial class Form1 : Form
    {
        Color color = Color.Black;
        
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int crColor);
        [DllImport("gdi32.dll")]
        public static extern bool ExtFloodFill(IntPtr hdc, int nXStart, int nYStart, int crColor, uint fuFillType);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern int GetPixel(IntPtr hdc, int x, int y);
        enum FLOODFILLTYPE { FLOODFILLBORDER, FLOODFILLSURFACE };
        public int x_start, y_start;
        

        public Form1()
        {
            InitializeComponent();
            label2.BackColor = color;
        }
        private void ExtFill(Graphics vGraphics, int x, int y, FLOODFILLTYPE floodFillType, Color fillColor, Color borderColor = default(Color))
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vBrush = CreateSolidBrush(ColorTranslator.ToWin32(fillColor));
            IntPtr vPreviouseBrush = SelectObject(vDC, vBrush);
            switch (floodFillType)
            {
                case FLOODFILLTYPE.FLOODFILLSURFACE:
                    ExtFloodFill(vDC, x, y, GetPixel(vDC, x, y), (uint)FLOODFILLTYPE.FLOODFILLSURFACE);
                    break;
                case FLOODFILLTYPE.FLOODFILLBORDER:
                    ExtFloodFill(vDC, x, y, ColorTranslator.ToWin32(borderColor), (uint)FLOODFILLTYPE.FLOODFILLBORDER);
                    break;
            }
            SelectObject(vDC, vPreviouseBrush);
            DeleteObject(vBrush);
            vGraphics.ReleaseHdc(vDC);
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
          this.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void elipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rectangleToolStripMenuItem.Checked = false;
            fillToolStripMenuItem.Checked = false;
            elipseToolStripMenuItem.Checked = true;
           
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            x_start = e.X;
            y_start = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Pen myPen = new Pen(color, 3);
            Graphics g = this.CreateGraphics();

            if (rectangleToolStripMenuItem.Checked == true)
            {
                g.DrawRectangle(myPen, x_start, y_start,e.X - x_start, e.Y - y_start);
            }
            else if (elipseToolStripMenuItem.Checked == true)
            {
               
                g.DrawEllipse(myPen, x_start, y_start, e.X - x_start, e.Y - y_start);
            }
            else if (fillToolStripMenuItem.Checked == true)
            {
                ExtFill(g, e.X, e.Y, FLOODFILLTYPE.FLOODFILLSURFACE, color);
            }
          
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rectangleToolStripMenuItem.Checked = true;
            fillToolStripMenuItem.Checked = false;
            elipseToolStripMenuItem.Checked = false;
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rectangleToolStripMenuItem.Checked = false;
            fillToolStripMenuItem.Checked = true;
            elipseToolStripMenuItem.Checked = false;
        }


        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowHelp = true;
            MyDialog.Color = color;
            if (MyDialog.ShowDialog() == DialogResult.OK)
                color = MyDialog.Color;
            label2.BackColor = color;
        }
    }
}
