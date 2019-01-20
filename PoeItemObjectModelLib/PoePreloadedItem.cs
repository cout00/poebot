﻿using PoeItemObjectModelLib.Bases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PoeItemObjectModelLib {
    [Serializable]
    public class PoePreloadedItem : IXmlSerializable {

        public ItemClass Class { get; set; }
        public BaseNames BaseName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public PoePreloadedItem() {

        }

        public XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(XmlReader reader) {
            reader.MoveToContent();
            reader.ReadStartElement();
            Class = reader.ReadElementContentAsString().ToItemClass();
            BaseName = reader.ReadElementContentAsString().ToBaseName();
            Height = reader.ReadElementContentAsInt();
            Width = reader.ReadElementContentAsInt();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer) {

        }
    }
}
