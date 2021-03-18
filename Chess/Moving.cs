using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class Moving
    {
        public Moving(XmlNode xmlNode)
        {
            int i = 0;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "repetition":
                        Repetition = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "offsetX":
                        OffsetX = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "offsetY":
                        OffsetY = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    default:

                        break;
                }
                i++;
            }
        }

        public int Repetition;
        public int OffsetX;
        public int OffsetY;
    }
}
