using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SPALM.SPSF.Library.CustomWizardPages
{
  public partial class SiteColumnWizard : UserControl
  {
    public event SchemaChangedDelegate SchemaChanged;
    public delegate void SchemaChangedDelegate();
    
    public SiteColumnWizard()
    {
      InitializeComponent();

      RegisterChangeEvent(this);

      UpdateControls();
    }

    private void RegisterChangeEvent(Control control)
    {
      if (control is TextBox)
      {
        (control as TextBox).TextChanged += new EventHandler(required_YES_CheckedChanged); 
      }
      else if (control is CheckBox)
      {
        (control as CheckBox).CheckedChanged += new EventHandler(required_YES_CheckedChanged);
      }
      else if (control is RadioButton)
      {
        (control as RadioButton).CheckedChanged += new EventHandler(required_YES_CheckedChanged);
      }
      else if (control is ComboBox)
      {
        (control as ComboBox).SelectedIndexChanged += new EventHandler(required_YES_CheckedChanged);
      }

      foreach (Control subcontrol in control.Controls)
      {
        RegisterChangeEvent(subcontrol);
      }
    }

    private void required_YES_CheckedChanged(object sender, EventArgs e)
    {
      this.RaiseSchemaChangedEvent();
    }


    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      UpdateControls();
    }

    private void RaiseSchemaChangedEvent()
    {
      if (SchemaChanged != null)
      {
        SchemaChanged();
      }
    }

    private string GetRequired()
    {
      if (required_YES.Checked)
      {
        return "TRUE";
      }
      return "FALSE";
    }

    private string GetMaxLength()
    {
      return maxLength.Text;
    }

    private string GetDefaultValue()
    {
      if (textBoxdefaultValue.Text != "")
      {
        if (defaultValueIsCalc.Checked)
        {
          return "<DefaultFormula>" + textBoxdefaultValue.Text + "</DefaultFormula>";
        }
        else
        {
          return "<Default>" + textBoxdefaultValue.Text + "</Default>";
        }
      }
      return "";

    }

    public string UpdateXML()
    {
      string Schema = "<Field Group=\"ColumnsGroup\" DisplayName=\"Column\" ID=\"{cfcb2a43-2a4e-4406-8fa4-e9d1fcf99818}\" SourceID=\"http://schemas.microsoft.com/sharepoint/v3\" StaticName=\"Multiline\" Name=\"MultilinePlain\" ";
      if (comboBox1.SelectedIndex == 0)
      {
        Schema += " Type=\"Text\" Required=\"" + GetRequired() + "\" MaxLength=\"" + GetMaxLength() + "\" >";
        if (GetDefaultValue() != "")
        {
          Schema += GetDefaultValue();
        }
      }
      else if (comboBox1.SelectedIndex == 1)
      {
        Schema += " Type=\"Note\" Sortable=\"FALSE\"";
        Schema += " NumLines=\"" + GetNumLines() + "\"";
        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " AppendOnly=\"" + GetAppendOnly() + "\"";
        Schema += " UnlimitedLengthInDocumentLibrary=\"" + GetUnlimitedLength() + "\"";

        if (multilineplain.Checked)
        {
          //plain
          Schema += " RichText=\"FALSE\"";
        }
        else if (multilinerich.Checked)
        {
          //rich
          Schema += " RichText=\"TRUE\" RichTextMode=\"Compatible\"";
        }
        else if (multilineenhancedrich.Checked)
        {
          //extendedrich
          Schema += " RichText=\"TRUE\" RichTextMode=\"FullHtml\" IsolateStyles=\"TRUE\"";
        }
        Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 2)
      {
        if (choiceCheckboxes.Checked)
        {
          Schema += " Type=\"MultiChoice\"";
        }
        else
        {
          Schema += " Type=\"Choice\"";
          if (choiceDropdown.Checked)
          {
            Schema += " Format=\"Dropdown\"";
          }
          else
          {
            Schema += " Format=\"RadioButtons\"";
          }
        }

        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " FillInChoice=\"" + GetFillInChoice() + "\"";

        Schema += ">";
        Schema += GetDefaultValue();
        Schema += GetChoices();

      }
      else if (comboBox1.SelectedIndex == 3)
      {
        //number
        /*
         * <Field Type="Number" Min="-.10" Max=".10" Percentage="TRUE" Decimals="2" 
         * */

        Schema += " Type=\"Number\"";
        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " Percentage=\"" + GetPercentage() + "\"";
        Schema += " Min=\"" + GetMinNumber(checkBoxPercentage.Checked) + "\"";
        Schema += " Max=\"" + GetMaxNumber(checkBoxPercentage.Checked) + "\"";
        if (GetDecimals() != "")
        {
          Schema += " Decimals=\"" + GetDecimals() + "\"";
        }
        Schema += ">";
        Schema += GetDefaultValue();
      }
      else if (comboBox1.SelectedIndex == 4)
      {
        Schema += " Type=\"Currency\"";
        Schema += " Required=\"" + GetRequired() + "\"";

        Schema += " Min=\"" + GetMinNumber(false) + "\"";
        Schema += " Max=\"" + GetMaxNumber(false) + "\"";
        if (GetDecimals() != "")
        {
          Schema += " Decimals=\"" + GetDecimals() + "\"";
        }
        Schema += " LCID=\"" + GetLCID() + "\"";
        /*currency
         <Field Type="Currency" Min="-10" Max="10" Decimals="2" LCID="2057"
        */
        Schema += ">";
        Schema += GetDefaultValue();
      }
      else if (comboBox1.SelectedIndex == 5)
      {
        Schema += " Type=\"DateTime\"";
        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " Format=\"" + GetDateFormat() + "\"";
        //datetime
        //<Field Type="DateTime" Format="DateOnly"

        //<Default>[today]</Default>
        Schema += ">";
        Schema += GetDefaultDateValue();
      }
      else if (comboBox1.SelectedIndex == 6)
      {
        Schema += " Type=\"Lookup\"";
        Schema += " List=\"" + textBox10.Text + "\"";
        Schema += " ShowField=\"" + textBox11.Text + "\"";
        Schema += " Mult=\"" + checkBox3.Checked.ToString() + "\"";
        Schema += " UnlimitedLengthInDocumentLibrary=\"" + checkBox4.Checked.ToString() + "\"";
        //lookup
        //<Field Type="Lookup" L   

        Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 7)
      {
        Schema += " Type=\"Boolean\"";
        Schema += ">";
        if (comboBoxDefaultBool.Text == "Yes")
        {
          Schema += "<Default>1</Default>";
        }
        else
        {
          Schema += "<Default>0</Default>";
        }
      }
      else if (comboBox1.SelectedIndex == 8)
      {

        if (AllowMultiUserYES.Checked)
        {
          Schema += " Type=\"UserMulti\"";
          Schema += " Mult=\"TRUE\" Sortable=\"FALSE\"";
        }
        else
        {
          Schema += " Type=\"User\"";
        }

        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " List=\"UserInfo\"";
        Schema += " ShowField=\"ImnName\"";
        //person group
        //<Field Type="User" List="UserInfo" ShowField="ImnName" 

        //<Field Type="UserMulti" List="UserInfo" ShowField="ImnName" 

        Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 9)
      {
        Schema += " Type=\"URL\"";
        Schema += " Required=\"" + GetRequired() + "\"";
        Schema += " Format=\"" + this.comboBoxHyperlinkOrPicture.Text + "\"";
        Schema += ">";
        //hyperlink picture
        //Type="URL" Format="Hyperlink"
      }
      else if (comboBox1.SelectedIndex == 10)
      {
          Schema += " Type=\"Calculated\"";
          //calc
          /*
          <Field Type="Calculated" DisplayName="CalculatedColumn" Format="DateOnly" LCID="1033" ResultType="Number" ReadOnly="TRUE" Group="Custom Columns" ID="{6ae3d7eb-3b8c-42a8-9d1e-b5baea1492b2}" SourceID="{c6cb73a7-763c-4a1d-a39a-06b0b3877f3e}" StaticName="CalculatedColumn" Name="CalculatedColumn" xmlns="http://schemas.microsoft.com/sharepoint/">
            <Formula>=ActualWork</Formula>
            <FieldRefs>
              <FieldRef Name="ActualWork" ID="{b0b3407e-1c33-40ed-a37c-2430b7a5d081}" />
            </FieldRefs>
          </Field>
          */
          Schema += ">";
      }
      // http://www.cnblogs.com/aivdesign/articles/1308759.html
      else if (comboBox1.SelectedIndex == 11){
        // Full HTML content with formatting and constraints for publishing
          Schema += " Type=\"HTML\" ";
          Schema += " RichText=\"TRUE\" RichTextMode=\"FullHtml\" ";
          Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 12)
      {
        // Image with formatting and constraints for publishing
          Schema += " Type=\"Image\" ";
          Schema += " RichText=\"TRUE\" RichTextMode=\"FullHtml\" ";
          Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 13)
      {
        // Hyperlink with formatting and constraints for publishing
          Schema += " Type=\"Link\" ";
          Schema += " RichText=\"TRUE\" RichTextMode=\"FullHtml\" ";
          Schema += ">";
      }
      else if (comboBox1.SelectedIndex == 14)
      {
          // Summary Links data
          Schema += " Type=\"SummaryLinks\" ";
          Schema += ">";
      }
      Schema += "</Field>";
      return Schema;
    }

    private string GetShowField()
    {
      return comboBox9.Text;
    }

    private string GetDefaultDateValue()
    {
      if (DefaultDateToday.Checked)
      {
        return "<DefaultFormula>[today]</DefaultFormula>";
      }
      else if (DefaultDateCalculatedYES.Checked)
      {
        return "<DefaultFormula>" + DefaultDateCalculated.Text + "</DefaultFormula>";
      }
      else if (DefaultDateFixed.Checked)
      {
        //<Default>2010-04-18T22:05:00Z</Default>
        return "<Default>" + dateTimePickerDefault.Value.ToString("MM-dd-yyyy") + "T" + dateTimePickerDefaultTime.Value.ToString("HH:mm:ss") + "Z</Default>";
      }
      return "";
    }

    private string GetDateFormat()
    {
      if (radioButtonDateOnlyYES.Checked)
      {
        return "DateOnly";
      }
      return "DateTime";
    }

    private string GetLCID()
    {
      return comboBoxLCID.Text;
    }

    private string GetDecimals()
    {
      try
      {
        int value = Int32.Parse(comboBoxDecimals.Text);
        return value.ToString();
      }
      catch (Exception)
      {
      }
      return "";
    }

    private string GetMaxNumber(bool percentage)
    {
      try
      {
        double value = 0;
        value = Double.Parse(textBoxMaxNumber.Text);
        if (percentage)
        {
          value = value / 100;
        }
        return value.ToString().Replace(",", ".");
      }
      catch (Exception)
      {
      }
      return "";
    }

    private string GetMinNumber(bool percentage)
    {
      try
      {
        double value = 0;
        value = Double.Parse(textBoxMinNumber.Text);
        if (percentage)
        {
          value = value / 100;
        }
        return value.ToString().Replace(",", ".");
      }
      catch (Exception)
      {
      }
      return "";
    }

    private string GetPercentage()
    {
      if (checkBoxPercentage.Checked)
      {
        return "TRUE";
      }
      return "FALSE";
    }

    private string GetChoices()
    {
      /*
      <CHOICES>
          <CHOICE>Enter Choice #1</CHOICE>
          <CHOICE>Enter Choice #2</CHOICE>
          <CHOICE>Enter Choice #3</CHOICE>
        </CHOICES>
       * */
      string res = "<CHOICES>";
      foreach (string s in textBoxChoices.Lines)
      {
        res += "<CHOICE>" + s + "</CHOICE>";
      }
      res += "</CHOICES>";
      return res;
    }

    private string GetFillInChoice()
    {
      if (AllowFillInYES.Checked)
      {
        return "TRUE";
      }
      return "FALSE";
    }

    private string GetUnlimitedLength()
    {
      if (unlimitedLengthYES.Checked)
      {
        return "TRUE";
      }
      return "FALSE";
    }

    private string GetAppendOnly()
    {
      if (AppendYES.Checked)
      {
        return "TRUE";
      }
      return "FALSE";
    }

    private string GetNumLines()
    {
      return textBoxNumLines.Text;
    }

    private void UpdateControls()
    {
      foreach (Control c in panel1.Controls)
      {
        c.Visible = false;
      }

      if (comboBox1.SelectedIndex == 0)
      {
        //single line
        panel_required.Visible = true;
        panel_MaxChars.Visible = true;
        panel_DefaultValue.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 1)
      {
        //multi line
        panel_required.Visible = true;
        panel_MaxChars.Visible = false;
        panel_DefaultValue.Visible = false;
        panel_MultiColumn.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 2)
      {
        //choice
        panel_required.Visible = true;
        panel_Choice.Visible = true;
        panel_DefaultValue.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 3)
      {
        //number
        panel_required.Visible = true;
        panel_Number.Visible = true;
        panel_DefaultValue.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 4)
      {
        //currency
        panel_required.Visible = true;
        panel_Number.Visible = true;
        panel_Currency.Visible = true;
        panel_DefaultValue.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 5)
      {
        //datetime
        panel_required.Visible = true;
        panel_DateTime.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 6)
      {
        //lookup
        panel_required.Visible = true;
        panel_LookUp.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 7)
      {
        //yes no
        panel_YesNo.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 8)
      {
        //person group
        panel_required.Visible = true;
        panel_User.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 9)
      {
        //hyperlink picture
        panel_required.Visible = true;
        panel_HyperlinkPicture.Visible = true;
      }
      else if (comboBox1.SelectedIndex == 10)
      {
        //calc
        panel_Calculated.Visible = true;
      }
    }

    private void label13_Click(object sender, EventArgs e)
    {

    }

    
  }
}
