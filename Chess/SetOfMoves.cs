using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Chess
{
    class SetOfMoves
    {
        public SetOfMoves(XmlNode xmlNode)
        {
            Name = xmlNode.Attributes.GetNamedItem("name").Value;
            int i = 0;
            int j;
            while (i < xmlNode.ChildNodes.Count)
            {
                switch (xmlNode.ChildNodes[i].Name)
                {
                    case "direction":
                        Direction = Convert.ToBoolean(xmlNode.ChildNodes[i].InnerText);
                        break;
                    case "possibleMoves":
                        j = 0;
                        while (j < xmlNode.ChildNodes[i].ChildNodes.Count)
                        {
                            if (xmlNode.ChildNodes[i].ChildNodes[j].Name == "move")
                            {
                                Array.Resize(ref Moves, Moves.Length + 1);
                                Moves[Moves.Length - 1] = new Moving(xmlNode.ChildNodes[i].ChildNodes[j]);
                            }
                            j++;
                        }
                        break;
                    default:

                        break;
                }
                i++;
            }
        }

        public string Name;
        public bool Direction;
        public Moving[] Moves = new Moving[0];
    }
}
