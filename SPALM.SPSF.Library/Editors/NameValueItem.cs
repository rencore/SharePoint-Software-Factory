using System;
using System.Collections.Generic;
using System.Text;

namespace SPALM.SPSF.Library
{
  [Serializable()]
  public class NameValueItem
  {
    public string _ItemType = "";
    public string _Name = "";
    public string _Value = "";
    public string _Group = "";
    public string _Description = "";

    private List<NameValueItem> childs;

    public List<NameValueItem> Childs
    {
        get
        {
            if (childs == null)
            {
                childs = new List<NameValueItem>();
            }
            return childs;
        }
        set
        {
            childs = value;
        }
    }

    public NameValueItem()
    {
    }

    public NameValueItem(string ItemType, string Name, string Value)
    {
      _ItemType = ItemType;
      _Name = Name;
      _Value = Value;
    }

    public override string ToString()
    {
      return _Value;
    }

    public string ItemType
    {
      get
      {
        return _ItemType;
      }
      set
      {
        _ItemType = value;
      }
    }

    public string Name
    {
      get
      {
        return _Name;
      }
      set
      {
        _Name = value;
      }
    }

    public string Value
    {
      get
      {
        return _Value;
      }
      set
      {
        _Value = value;
      }
    }

    public string Group
    {
      get
      {
        return _Group;
      }
      set
      {
        _Group = value;
      }
    }

    public string Description
    {
      get
      {
        return _Description;
      }
      set
      {
        _Description = value;
      }
    }

    public string DisplayName
    {
        get
        {
            if (Name != Value)
            {
                return Name + " (" + Value + ")";
            }
            else
            {
                return Value;
            }
        }
    }
  }
}
