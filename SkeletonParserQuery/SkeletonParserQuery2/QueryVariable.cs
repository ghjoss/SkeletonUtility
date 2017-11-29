using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkeletonParserDSDef;

namespace SkeletonParserQuery
{
    public partial class Query : Form
    {
        private SkeletonParserDS ds = new SkeletonParserDS();
        private bool initializing = true;
        private const string ALL_KEYS = " -All- ";
        public Query()
        {
            UserInit();
            InitializeComponent();
            UserInit2();
        }

        private void UserInit()
        {
            string s = @"C:\source\Skeleton Utility 2008\SkeletonUtility\Utility\XMLFile1.DBDump.xml";
            ds.ReadXml(s, XmlReadMode.ReadSchema);
        }

        private void UserInit2()
        {
            var results =
                from o in ds.Variables.AsEnumerable()
                    select o;

            dataGridView1.DataSource = results.AsDataView();
            InitializeLists();
        }

        private void InitializeLists()
        {
            string[] results =
                (from o in ds.Variables.AsEnumerable()
                 select o.Skeleton).Distinct().ToArray();

            ArrayList results2 = new ArrayList(results.Length + 1);
            results2.Add(ALL_KEYS);
            for (int i = 0; i < results.Length; ++i)
                results2.Add(results[i]);

            results2.Sort();

            cboSkeletons.DataSource = results2;

            results = (from o in ds.Variables.AsEnumerable()
                       select o.Variable).Distinct().ToArray();
            ArrayList results3 = new ArrayList(results.Length + 1);
            results3.Add(ALL_KEYS);
            for (int i = 0; i < results.Length; ++i)
                results3.Add(results[i]);

            results3.Sort();
            cboVariables.DataSource = results3;

            results = (from o in ds.Variables.AsEnumerable()
                       select o.Type).Distinct().ToArray();
            ArrayList results4 = new ArrayList(results.Length + 1);
            results4.Add(ALL_KEYS);
            for (int i = 0; i < results.Length; ++i)
                results4.Add(results[i]);
            results4.Sort();
            cboType.DataSource = results4;

            initializing = false;
            
            SkelQ(ALL_KEYS,ALL_KEYS, ALL_KEYS);
        }

        private void SkelQ(string skelName, string varName, string varType)
        {
            var results = (
                  skelName == ALL_KEYS && varName == ALL_KEYS && varType == ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where true
                 select o)
                : skelName == ALL_KEYS && varName != ALL_KEYS && varType == ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Variable == varName
                 select o)
                : skelName == ALL_KEYS && varName == ALL_KEYS && varType != ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Type == varType
                 select o)
                : skelName != ALL_KEYS && varName == ALL_KEYS && varType == ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Skeleton == skelName
                 select o)
                : skelName != ALL_KEYS && varName != ALL_KEYS && varType == ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Skeleton == skelName && o.Variable == varName
                 select o)
                : skelName != ALL_KEYS && varName == ALL_KEYS && varType != ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Skeleton == skelName && o.Type == varType
                 select o)
                : skelName == ALL_KEYS && varName != ALL_KEYS && varType != ALL_KEYS ?
                (from o in ds.Variables.AsEnumerable()
                 where o.Variable == varName && o.Type == varType
                 select o)
                :
                (from o in ds.Variables.AsEnumerable()
                 where o.Skeleton == skelName
                 && o.Variable == varName
                 && o.Type == varType
                 select o));
            dataGridView1.DataSource = results.AsDataView();
        }

        private void ComboBoxes_TextChanged(object sender, EventArgs e)
        {
            if (!initializing)
                SkelQ(cboSkeletons.Text, cboVariables.Text, cboType.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboSkeleton_TextChanged(object sender, EventArgs e)
        {
            if (initializing)
                return;
            if (cboSkeletons.Text != ALL_KEYS)
            {
                var results = (from o in ds.Variables.AsEnumerable()
                           where o.Skeleton == cboSkeletons.Text
                           select o.Variable).Distinct().ToArray();
                ArrayList results3 = new ArrayList(results.Length + 1);
                results3.Add(ALL_KEYS);
                for (int i = 0; i < results.Length; ++i)
                    results3.Add(results[i]);

                results3.Sort();

                cboVariables.DataSource = results3;

                results = (from o in ds.Variables.AsEnumerable()
                           where o.Skeleton == cboSkeletons.Text
                           select o.Type).Distinct().ToArray();
                ArrayList results4 = new ArrayList(results.Length + 1);
                results4.Add(ALL_KEYS);
                for (int i = 0; i < results.Length; ++i)
                    results4.Add(results[i]);
                results4.Sort();
               
                cboType.DataSource = results4;
            }
            ComboBoxes_TextChanged(sender, e);
        }
    }
}
