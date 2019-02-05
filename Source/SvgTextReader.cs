using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Svg
{
    internal sealed class SvgTextReader : XmlTextReader
    {
        private Dictionary<string, string> _entities;
        private string _value;
        private bool _customValue;
        private string _localName;

        public SvgTextReader(Stream stream, Dictionary<string, string> entities)
            : base(stream)
        {
            EntityHandling = EntityHandling.ExpandEntities;
            _entities = entities;
        }

        public SvgTextReader(TextReader reader, Dictionary<string, string> entities)
            : base(reader)
        {
            EntityHandling = EntityHandling.ExpandEntities;
            _entities = entities;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the text value of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The value returned depends on the <see cref="P:System.Xml.XmlTextReader.NodeType" /> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.Node Type Value AttributeThe value of the attribute. CDATAThe content of the CDATA section. CommentThe content of the comment. DocumentTypeThe internal subset. ProcessingInstructionThe entire content, excluding the target. SignificantWhitespaceThe white space within an xml:space= 'preserve' scope. TextThe content of the text node. WhitespaceThe white space between markup. XmlDeclarationThe content of the declaration. </returns>
        public override string Value => (_customValue) ? _value : base.Value;

        /// <summary>
        /// Gets the local name of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.</returns>
        public override string LocalName => (_customValue) ? _localName : base.LocalName;

        private IDictionary<string, string> Entities => _entities ?? (_entities = new Dictionary<string, string>());

        /// <inheritdoc />
        /// <summary>
        /// Moves to the next attribute.
        /// </summary>
        /// <returns>
        /// true if there is a next attribute; false if there are no more attributes.
        /// </returns>
        public override bool MoveToNextAttribute()
        {
            var moved = base.MoveToNextAttribute();

            if (!moved) return false;
            _localName = base.LocalName;

            if (ReadAttributeValue())
            {
                if (NodeType == XmlNodeType.EntityReference)
                {
                    ResolveEntity();
                }
                else
                {
                    _value = base.Value;
                }
            }
            _customValue = true;

            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Reads the next node from the stream.
        /// </summary>
        /// <returns>
        /// true if the next node was read successfully; false if there are no more nodes to read.
        /// </returns>
        /// <exception cref="T:System.Xml.XmlException">An error occurred while parsing the XML. </exception>
        public override bool Read()
        {
            _customValue = false;
            var read = base.Read();

            if (NodeType == XmlNodeType.DocumentType)
            {
                ParseEntities();
            }

            return read;
        }

        private void ParseEntities()
        {
            const string entityText = "<!ENTITY";
            var entities = Value.Split(new[]{entityText}, StringSplitOptions.None);

            foreach (var entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Trim()))
                {
                    continue;
                }

                var name = entity.Trim();
                var quoteIndex = name.IndexOf(QuoteChar);
                if (quoteIndex <= 0) continue;
                var value = name.Substring(quoteIndex + 1, name.LastIndexOf(QuoteChar) - quoteIndex - 1);
                name = name.Substring(0, quoteIndex).Trim();
                Entities.Add(name, value);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Resolves the entity reference for EntityReference nodes.
        /// </summary>
        public override void ResolveEntity()
        {
            if (NodeType != XmlNodeType.EntityReference) return;
            _value = _entities.ContainsKey(Name) ? _entities[Name] : string.Empty;

            _customValue = true;
        }
    }
}