using SSP1126.PcPos.BaseClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSHOP
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            PcPosFactory f = new PcPosFactory();
            f.CardSwiped += F_CardSwiped;
        }

        private void F_CardSwiped(SSP1126.PcPos.Infrastructure.PosResult posResult)
        {
            throw new NotImplementedException();
        }
    }
}
