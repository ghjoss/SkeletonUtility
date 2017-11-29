using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Pitman.Printing
{
    public class PrintHelper
    {
        public PrintHelper(){}

        /// <summary>
        /// Gets an image that shows the entire tree, not just what is visible on the form
        /// </summary>
        /// <param name="tree"></param>
        public Image PrepareTreeImage(TreeView tree, TreeView treeClone)
        {
            treeClone.Width = tree.Width;
            treeClone.Height = tree.Height;

            treeClone.Nodes[0].EnsureVisible();
            treeClone.ExpandAll();
            int height = tree.Nodes[0].Bounds.Height;
            int width = tree.Nodes[0].Bounds.Right;
            TreeNode node = treeClone.Nodes[0].NextVisibleNode;
            while (node != null)
            {
                height += node.Bounds.Height;
                if (node.Bounds.Right > width)
                {
                    width = node.Bounds.Right;
                }
                node = node.NextVisibleNode;
            }
            //setup the tree to take the snapshot
            treeClone.SelectedNode = treeClone.Nodes[0];
            treeClone.Height = height + (tree.Height - tree.ClientSize.Height);
            treeClone.Width = width + (tree.Width - tree.ClientSize.Width);
            treeClone.BorderStyle = BorderStyle.None;
            treeClone.Dock = DockStyle.None;
            //get the image of the tree

            // .Net 2.0 supports drawing controls onto bitmaps natively now
            // However, the full tree didn't get drawn when I tried it, so I am
            // sticking with the P/Invoke calls
            //_controlImage = new Bitmap(height, width);
            //Bitmap bmp = _controlImage as Bitmap;
            //tree.DrawToBitmap(bmp, tree.Bounds);
            return GetImage(treeClone.Handle, treeClone.Width, treeClone.Height);
        }

        // Returns an image of the specified width and height, of a control represented by handle.
        private Image GetImage(IntPtr handle, int width, int height)
        {
            IntPtr screenDC = GetDC(IntPtr.Zero);
            IntPtr hbm = CreateCompatibleBitmap(screenDC, width, height);
            Image image = Bitmap.FromHbitmap(hbm);
            Graphics g = Graphics.FromImage(image);
            IntPtr hdc = g.GetHdc();
            SendMessage(handle, 0x0318 /*WM_PRINTCLIENT*/, hdc, (0x00000010 | 0x00000004 | 0x00000002));
            g.ReleaseHdc(hdc);
            ReleaseDC(IntPtr.Zero, screenDC);
            return image;
        }
        //External function declarations
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int width, int height);
    }
}
