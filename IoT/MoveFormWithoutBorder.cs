using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace IoT
{
    public static class MoveFormWithoutBorder
    {
        private static bool isDragging;
        private static Point lastCursor;
        private static Point lastForm;

        public static void RegisterForm(Form form)
        {
            form.MouseDown += Form_MouseDown;
            form.MouseMove += Form_MouseMove;
            form.MouseUp += Form_MouseUp;
        }

        private static void Form_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastCursor = Cursor.Position;
            lastForm = ((Form)sender).Location;
        }

        private static void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentCursor = Cursor.Position;
                int deltaX = currentCursor.X - lastCursor.X;
                int deltaY = currentCursor.Y - lastCursor.Y;
                ((Form)sender).Location = new Point(lastForm.X + deltaX, lastForm.Y + deltaY);
            }
        }

        private static void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
