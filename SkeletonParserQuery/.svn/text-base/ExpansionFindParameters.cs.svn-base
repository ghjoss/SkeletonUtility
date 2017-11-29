using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace SkeletonParserQuery
{
    public partial class ExpansionFindParameters : Form
    {
        private Query _parent;

        private DialogResult _dr = DialogResult.Ignore;
        public DialogResult Result
        {
            get { return _dr; }
        }

        private string _findStr;
        public string FindString
        {
            get { return _findStr; }
        }

        private bool _matchWord;
        public bool MatchWord
        {
            get { return _matchWord; }
        }

        private bool _matchCase;
        public bool MatchCase
        {
            get { return _matchCase; }
        }

        public ExpansionFindParameters(Query parent)
        {
            InitializeComponent();
            _parent = parent;
            _parent.AddForm(this);
        }
 

        private void OK_Click(object sender, EventArgs e)
        {
            _dr = DialogResult.OK;
            _findStr = txtFindStr.Text;
            _matchCase = cbMatchCase.Checked;
            _matchWord = cbMatchWord.Checked;
            this.Close();
       }

        private void Cancel_Click(object sender, EventArgs e)
        {
            _dr = DialogResult.Cancel;
            _findStr = "";
            _matchCase = false;
            _matchWord = false;
            this.Close();
        }

        public DialogResult ShowDialog(ref string findstr, ref bool matchCase, ref bool matchWord)
        {
            this.ShowDialog();
            findstr = _findStr;
            matchCase = _matchCase;
            matchWord = _matchWord;
            return _dr;
        }

    }
}
