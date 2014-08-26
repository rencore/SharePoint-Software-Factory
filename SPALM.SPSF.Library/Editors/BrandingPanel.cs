using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.WizardFramework;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.IO;
using Microsoft.Practices.RecipeFramework.Services;
using System.Xml;

namespace SPALM.SPSF.Library
{

    /// <summary>
    /// Controls changes the background image in the header of a wizard.
    /// </summary>
    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class BrandingPanel : ArgumentPanelTypeEditor
    {
        Microsoft.VisualStudio.VSHelp.Help helpService;

        protected string GetBasePath()
        {
            IConfigurationService s = this.GetService(typeof(IConfigurationService)) as IConfigurationService;
            return s.BasePath;
        }

        private bool logosChanged = false;
        public BrandingPanel()
            : base()
        {
        }

        private string helpPage = "";

        public void UpdateHelpPage(string basepath, string recipeName, Microsoft.VisualStudio.VSHelp.Help helpService)
        {
            this.helpService = helpService;
            try
            {
                helpPage = GetHelpPage(basepath, recipeName);
                if (helpPage != "")
                {
                    this.WizardPage.Wizard.HelpButton = true;
                    this.WizardPage.Wizard.HelpButtonClicked += new CancelEventHandler(Wizard_HelpButtonClicked);
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void UpdateValue(object newValue)
        {
            base.UpdateValue("test");

            UpdateImage(GetBasePath());
        }

        public void UpdateImage(string basePath)
        {
            Bitmap image  = SPALM.SPSF.Library.Properties.Resources.SPSFBannerLogo;
            WizardForm wizard = this.WizardPage.Wizard;
            Control header = wizard.Controls["_headerBannerPanel"];

            double width = (double)header.Height / (double)image.Height * (double)image.Width; 
            image = new Bitmap(image, new Size((int)width, header.Height));

            helpPage = GetHelpPage(basePath, "");
            if (helpPage != "")
            {
                this.WizardPage.Wizard.HelpButton = true;
                this.WizardPage.Wizard.HelpButtonClicked += new CancelEventHandler(Wizard_HelpButtonClicked);
            }

            if (!logosChanged)
            {
                logosChanged = true;
                try
                {
                    for (int i = 0; i < this.WizardPage.Wizard.PageCount; i++)
                    {
                        this.WizardPage.Wizard.GetPage(i).Logo = image;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ex" + ex.ToString());
                }
            }
        }

        void Wizard_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            if (helpPage != "")
            {
                if (helpService == null)
                {
                    helpService = (Microsoft.VisualStudio.VSHelp.Help)base.GetService(typeof(Microsoft.VisualStudio.VSHelp.Help));
                }
                if (helpService != null)
                {
                    try
                    {
                        helpService.DisplayTopicFromURL(helpPage);
                    }
                    catch (Exception)
                    {
                    }
                }
                e.Cancel = true;
            }
        }
        private string GetHelpPage(string basePath, string recipeName)
        {

            if (recipeName == "")
            {
                try
                {
                    IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                    recipeName = dictionaryService.GetValue("RecipeName").ToString();
                }
                catch (Exception)
                {

                }
            }

            string localPath = string.Empty;

            if (recipeName != "")
            {
                string uriString = basePath += "\\Help\\OutputHTML\\SPSF_RECIPE_" + recipeName.ToUpper() + ".html";
                if (File.Exists(uriString))
                {
                    Uri result = null;
                    if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out result))
                    {
                    }
                    if (result != null)
                    {
                        if (result.IsAbsoluteUri)
                        {
                            if (result.IsFile)
                            {
                                localPath = result.LocalPath;
                            }
                            else
                            {
                                localPath = result.AbsoluteUri;
                            }
                        }
                        else
                        {
                            //localPath = Path.Combine(this.basePath, uriString);
                        }
                    }
                }
            }
            return localPath;
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            try
            {
                WizardForm wizard = this.WizardPage.Wizard;
                for (int i = 0; i < wizard.PageCount; i++ )
                {
                    Microsoft.WizardFramework.WizardPage page = wizard.GetPage(i);
                    page.InfoRTBoxIcon = null;
                }
               
            }
            catch { }

            try
            {
                WizardForm wizard = this.WizardPage.Wizard;
                Control header = wizard.Controls["_headerBannerPanel"];
                header.BackColor = Color.White;
                //header.BackgroundImage = SPALM.SPSF.Library.Properties.Resources.SPSFBannerTitle;
                Image title = SPALM.SPSF.Library.Properties.Resources.SPSFBannerTitle;
                double width = (double)header.Height / (double)title.Height * (double)title.Width;
                header.BackgroundImage = (Image)new Bitmap(title, new Size((int)width, header.Height));
                header.BackgroundImageLayout = ImageLayout.None;
                header.Visible = true;
                
                PictureBox pictureBox = header.Controls["_pageLogo"] as PictureBox;
                pictureBox.Dock = DockStyle.Right;
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                // resize not working, for whatever reason, taking two images for now to support VisualStyles (Win8)
                //Image logo = header.Height < 74?SPALM.SPSF.Library.Properties.Resources.SPSFBannerLogoSmall:SPALM.SPSF.Library.Properties.Resources.SPSFBannerLogo;
                Image logo = SPALM.SPSF.Library.Properties.Resources.SPSFBannerLogo;
                width = (double)header.Height / (double)logo.Height * (double)logo.Width;
                logo = (Image)new Bitmap(logo, new Size((int)width, header.Height));
                pictureBox.Image = logo;
                pictureBox.InitialImage = logo;
                pictureBox.Visible = true;
                //pictureBox.Visible = false;

                Label headlineLabel = header.Controls["_headlineLabel"] as Label;
                headlineLabel.Visible = false;
                //headlineLabel.ForeColor = Color.Red;
                //headlineLabel.TextAlign = ContentAlignment.MiddleRight;
                //headlineLabel.Font = new Font(this.Font.FontFamily, 11);
                //headlineLabel.BringToFront();

                pictureBox.SendToBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ex" + ex.ToString());
            }

            //hide the argumentpanelcontrol
            this.Visible = false;
        }
    }
}