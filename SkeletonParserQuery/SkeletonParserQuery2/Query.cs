using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkeletonParserDSDef;

namespace SkeletonParserQuery
{
    public partial class Query : Form
    {
        public SkeletonParserDS ds = new SkeletonParserDS();
        public Query()
        {
            UserInit();
            InitializeComponent();
        }

        public void UserInit()
        {
            string s = @"C:\source\Skeleton Utility 2008\SkeletonUtility\Utility\XMLFile1.DBDump.xml";
            ds.ReadXml(s, XmlReadMode.ReadSchema);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnVariables_Click(object sender, EventArgs e)
        {
            var qv = new QueryVariable(ds);
            qv.Show();
        }

        private void btnSkeleton_Click(object sender, EventArgs e)
        {
            var qs = new QuerySkeleton(ds);
            qs.Show();
        }
    }
}
