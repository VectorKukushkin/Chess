using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Chess
{
    class Chessman
    {
        public Chessman(XmlNode xmlNode)
        {
            Name = xmlNode.Attributes.GetNamedItem("name").Value;
            SpecialMoves = new int[0];
            int[] vs;
            int i = 0;
            int j;
            int k;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "abbreviation":
                        Abbreviation = xmlNode.ChildNodes[i].InnerText;
                        break;
                    case "relativeValue":
                        RelativeValue = Convert.ToInt32(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "checkmate":
                        Checkmate = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "movement":
                        Movement = Storage.GetSetOfMoves(xmlNode.ChildNodes[i].InnerText);
                        if (Movement == Capturing)
                        {
                            IsSame = true;
                        }
                        else
                        {
                            IsSame = false;
                        }
                        break;
                    case "capturing":
                        Capturing = Storage.GetSetOfMoves(xmlNode.ChildNodes[i].InnerText);
                        if (Movement == Capturing)
                        {
                            IsSame = true;
                        }
                        else
                        {
                            IsSame = false;
                        }
                        break;
                    case "special":
                        j = 0;
                        while (j < xmlNode.ChildNodes[i].ChildNodes.Count)
                        {
                            if (xmlNode.ChildNodes[i].ChildNodes[j].Name == "move")
                            {
                                k = 0;
                                vs = Storage.GetSpecialMove(xmlNode.ChildNodes[i].ChildNodes[j].InnerText);
                                while (k < vs.Length)
                                {
                                    Array.Resize(ref SpecialMoves, SpecialMoves.Length + 1);
                                    SpecialMoves[SpecialMoves.Length - 1] = vs[k];
                                    k++;
                                }
                            }
                            j++;
                        }
                        break;
                    case "imageWhite":
                        ImageWhite = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/" + xmlNode.ChildNodes[i].InnerText));
                        break;
                    case "imageBlack":
                        ImageBlack = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/" + xmlNode.ChildNodes[i].InnerText));
                        break;
                    default:

                        break;
                }
                i++;
            }
            if (Abbreviation == null)
            {
                Abbreviation = Name;
            }
        }

        public string Name;
        public string Abbreviation;
        public int RelativeValue;
        public int Movement;
        public int Capturing;
        public int[] SpecialMoves;
        public bool IsSame;
        public bool Checkmate;
        public ImageSource ImageWhite;
        public ImageSource ImageBlack;
    }
}
