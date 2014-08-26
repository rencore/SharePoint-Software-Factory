using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SPALM.SPSF.Library.Editors;

namespace SPALM.SPSF.Library
{
  internal class ImageListProvider
  {
    public static ImageList GetIcons()
    {
      ImageList list = new ImageList();
      list.ColorDepth = ColorDepth.Depth32Bit;
      list.ImageSize = new Size(16, 16);

      list.Images.Add("Feature", ResourceIcons.Feature);
      list.Images.Add("ContentType", ResourceIcons.icons_contenttype);

      return list;
    }
  }
}
