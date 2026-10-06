using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFVisionInspect
{
    public partial class FormSetMorphology : Form
    {
        public MorphologyParam Param { get; set; }

        public FormSetMorphology()
        {
            InitializeComponent();

            comboType0.Items.Add(enMorphologyType.None.ToString());
            comboType0.Items.Add(enMorphologyType.Erosion.ToString());
            comboType0.Items.Add(enMorphologyType.Dilation.ToString());
            comboType0.Items.Add(enMorphologyType.Closing.ToString());
            comboType0.Items.Add(enMorphologyType.Opening.ToString());

            comboType1.Items.Add(enMorphologyType.None.ToString());
            comboType1.Items.Add(enMorphologyType.Erosion.ToString());
            comboType1.Items.Add(enMorphologyType.Dilation.ToString());
            comboType1.Items.Add(enMorphologyType.Closing.ToString());
            comboType1.Items.Add(enMorphologyType.Opening.ToString());
            comboType1.SelectedIndex = 0;

            comboType2.Items.Add(enMorphologyType.None.ToString());
            comboType2.Items.Add(enMorphologyType.Erosion.ToString());
            comboType2.Items.Add(enMorphologyType.Dilation.ToString());
            comboType2.Items.Add(enMorphologyType.Closing.ToString());
            comboType2.Items.Add(enMorphologyType.Opening.ToString());
            comboType2.SelectedIndex = 0;

            comboType3.Items.Add(enMorphologyType.None.ToString());
            comboType3.Items.Add(enMorphologyType.Erosion.ToString());
            comboType3.Items.Add(enMorphologyType.Dilation.ToString());
            comboType3.Items.Add(enMorphologyType.Closing.ToString());
            comboType3.Items.Add(enMorphologyType.Opening.ToString());
            comboType3.SelectedIndex = 0;

            Param = new MorphologyParam();


        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Param.Type[0] = (enMorphologyType)Enum.ToObject(typeof(enMorphologyType), comboType0.SelectedIndex); ;
            Param.Type[1] = (enMorphologyType)Enum.ToObject(typeof(enMorphologyType), comboType1.SelectedIndex); ;
            Param.Type[2] = (enMorphologyType)Enum.ToObject(typeof(enMorphologyType), comboType2.SelectedIndex); ;
            Param.Type[3] = (enMorphologyType)Enum.ToObject(typeof(enMorphologyType), comboType3.SelectedIndex); ;

            Param.Length[0] = (int)numLength0.Value;
            Param.Length[1] = (int)numLength1.Value;
            Param.Length[2] = (int)numLength2.Value;
            Param.Length[3] = (int)numLength3.Value;

        }

        private void FormSetMorphology_Load(object sender, EventArgs e)
        {

            comboType0.SelectedIndex = (int)Param.Type[0];
            comboType1.SelectedIndex = (int)Param.Type[1];
            comboType2.SelectedIndex = (int)Param.Type[2];
            comboType3.SelectedIndex = (int)Param.Type[3];

            numLength0.Value = Param.Length[0];
            numLength1.Value = Param.Length[1];
            numLength2.Value = Param.Length[2];
            numLength3.Value = Param.Length[3];
        }
    }
}
