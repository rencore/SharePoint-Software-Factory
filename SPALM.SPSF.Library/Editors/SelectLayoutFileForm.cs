using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.IO;
using Microsoft.Practices.ComponentModel;

namespace SPALM.SPSF.Library.Editors
{
  [ServiceDependency(typeof(DTE))]
  public partial class SelectLayoutFileForm : Form
  {
    public string selectedPath = "";

    public SelectLayoutFileForm()
    {
      InitializeComponent();
    }

    public SelectLayoutFileForm(DTE envdte, object currentselection)
    {
      InitializeComponent();

      try
      {
        LoadTree(envdte);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void LoadTree(DTE envdte)
    {
      treeView1.Nodes.Clear();

      foreach (Project project in envdte.Solution.Projects)
      {
        if (project.Object is SolutionFolder)
        {
          SolutionFolder x = (SolutionFolder)project.Object;
          foreach (ProjectItem pitem in x.Parent.ProjectItems)
          {            
            if (pitem.Object != null)
            {
              if (pitem.Object is Project)
              {
                LoadProject(pitem.Object as Project);
              }
            }            
          }
        }
        else
        {
          LoadProject(project);
        }
      }
    }

    private void LoadProject(Project project)
    {
      Load12Hive(project);
    }

    private void Load12Hive(Project project)
    {    
      ProjectItem layoutsFolder = null;
      ProjectItem imagesFolder = null;
			ProjectItem adminFolder = null;
      
      try
      {
        layoutsFolder = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, Helpers.GetSharePointHive(project.DTE)).ProjectItems, "template").ProjectItems, "Layouts");
      }
      catch (Exception)
      {
      }

      try
      {
				imagesFolder = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, Helpers.GetSharePointHive(project.DTE)).ProjectItems, "template").ProjectItems, "Images");
      }
      catch (Exception)
      {
      }

			try
			{
				adminFolder = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, Helpers.GetSharePointHive(project.DTE)).ProjectItems, "template").ProjectItems, "Admin");
			}
			catch (Exception)
			{
			}

			if ((layoutsFolder != null) || (imagesFolder != null) || (adminFolder != null))
      {
        TreeNode node = treeView1.Nodes.Add(project.Name);
        node.Tag = project;
        node.ImageKey = "Folder";
        node.SelectedImageKey = "Folder";

        if (layoutsFolder != null)
        {
          TreeNode nodelayouts = new TreeNode("Layouts");
          nodelayouts.Tag = layoutsFolder;
          node.Nodes.Add(nodelayouts);
          SetIcon(nodelayouts, layoutsFolder);
          LoadItems(layoutsFolder, nodelayouts);
        }

        if (imagesFolder != null)
        {
          TreeNode nodelayouts = new TreeNode("Images");
          nodelayouts.Tag = imagesFolder;
          node.Nodes.Add(nodelayouts);
          SetIcon(nodelayouts, layoutsFolder);
          LoadItems(imagesFolder, nodelayouts);
        }

				if (adminFolder != null)
				{
					TreeNode nodelayouts = new TreeNode("Admin");
					nodelayouts.Tag = adminFolder;
					node.Nodes.Add(nodelayouts);
					SetIcon(nodelayouts, adminFolder);
					LoadItems(adminFolder, nodelayouts);
				}
      }
    }

    private void SetIcon(TreeNode nodelayouts, ProjectItem item)
    {
			if (item != null)
			{
				if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
				{
					try
					{
						switch (Path.GetExtension(item.get_FileNames(1)))
						{
							case ".ascx":
							case ".asmx":
							case ".aspx":
							case ".htm":
							case ".html":
								nodelayouts.ImageKey = "Page";
								nodelayouts.SelectedImageKey = "Picture";
								return;
							case ".jpg":
							case ".jpeg":
							case ".gif":
							case ".png":
							case ".bmp":
								nodelayouts.ImageKey = "Picture";
								nodelayouts.SelectedImageKey = "Picture";
								return;
							default:
								nodelayouts.ImageKey = "Document";
								nodelayouts.SelectedImageKey = "Document";
								return;
						}
					}
					catch (Exception)
					{
						nodelayouts.ImageKey = "Document";
						nodelayouts.SelectedImageKey = "Document";
					}
				}
				else
				{
					nodelayouts.ImageKey = "Folder";
					nodelayouts.SelectedImageKey = "Folder";
				}
			}
			else
			{
				nodelayouts.ImageKey = "Folder";
				nodelayouts.SelectedImageKey = "Folder";
			}
    }

    private void LoadItems(ProjectItem p, TreeNode parentNode)
    {
      foreach (ProjectItem child in p.ProjectItems)
      {
        TreeNode node = parentNode.Nodes.Add(child.Name);
        node.Tag = child;
        SetIcon(node, child);
        LoadItems(child, node);
      }
    }
       



    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {      
      if (e.Node.Tag != null)
      {
        if (e.Node.Tag is ProjectItem)
        {
          ProjectItem pitem = e.Node.Tag as ProjectItem;
          if (pitem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
          {
            SetText(e.Node.FullPath);
            return;
          }
        }
      }
      textBox1.Text = "";
    }

    private void SetText(string path)
    {
        DTE service = (DTE)this.GetService(typeof(DTE));

      //remove 
      path = path.Substring(path.IndexOf("/")).ToLower();
      if (path.StartsWith("/layouts/"))
      {
          path = path.Replace("/layouts/", "/_layouts" + Helpers.GetVersionedFolder(service) + "/");
      }
      if (path.StartsWith("/images/"))
      {
          path = path.Replace("/images/", "/_layouts" + Helpers.GetVersionedFolder(service) + "/images/");
      }
			if (path.StartsWith("/admin/"))
			{
				path = path.Replace("/admin/", "/_admin/");
			}
      textBox1.Text = path;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (textBox1.Text != "")
      {
        button_ok.Enabled = true;
      }
      else
      {
        button_ok.Enabled = false;
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      selectedPath = textBox1.Text;
      Close();
    }
  }
}
