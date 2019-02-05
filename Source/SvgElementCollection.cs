using System;
using System.Collections.Generic;
using System.Linq;

namespace Svg
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a collection of <see cref="T:Svg.SvgElement" />s.
    /// </summary>
    public sealed class SvgElementCollection : IList<SvgElement>
    {
        private readonly List<SvgElement> _elements;
        private readonly SvgElement _owner;
        private readonly bool _mock;

        /// <summary>
        /// Initialises a new instance of an <see cref="SvgElementCollection"/> class.
        /// </summary>
        /// <param name="owner">The owner <see cref="SvgElement"/> of the collection.</param>
        internal SvgElementCollection(SvgElement owner)
            : this(owner, false)
        {

        }

        internal SvgElementCollection(SvgElement owner, bool mock)
        {
            _elements = new List<SvgElement>();
            _owner = owner ?? throw new ArgumentNullException("owner");
            _mock = mock;
        }

        /// <summary>
        /// Returns the index of the specified <see cref="SvgElement"/> in the collection.
        /// </summary>
        /// <param name="item">The <see cref="SvgElement"/> to search for.</param>
        /// <returns>The index of the element if it is present; otherwise -1.</returns>
        public int IndexOf(SvgElement item)
        {
            return _elements.IndexOf(item);
        }

        /// <summary>
        /// Inserts the given <see cref="SvgElement"/> to the collection at the specified index.
        /// </summary>
        /// <param name="index">The index that the <paramref name="item"/> should be added at.</param>
        /// <param name="item">The <see cref="SvgElement"/> to be added.</param>
        public void Insert(int index, SvgElement item)
        {
            InsertAndForceUniqueId(index, item, true, true, LogIDChange);
        }
        
        private void LogIDChange(SvgElement elem, string oldId, string newId)
        {
        	 System.Diagnostics.Debug.WriteLine("ID of SVG element " + elem + " changed from " + oldId + " to " + newId);
        }

        public void InsertAndForceUniqueId(int index, SvgElement item, bool autoForceUniqueId = true, bool autoFixChildrenId = true, Action<SvgElement, string, string> logElementOldIdNewId = null)
        {
            AddToIdManager(item, _elements[index], autoForceUniqueId, autoFixChildrenId, logElementOldIdNewId);
            _elements.Insert(index, item);
            item.SvgParent.OnElementAdded(item, index);
        }

        public void RemoveAt(int index)
        {
            SvgElement element = this[index];

            if (element != null)
            {
                Remove(element);
            }
        }

        public SvgElement this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        public void Add(SvgElement item)
        {
            AddAndForceUniqueId(item, true, true, LogIDChange);
        }

        public void AddAndForceUniqueId(SvgElement item, bool autoForceUniqueId = true, bool autoFixChildrenId = true, Action<SvgElement, string, string> logElementOldIdNewId = null)
        {
            AddToIdManager(item, null, autoForceUniqueId, autoFixChildrenId, logElementOldIdNewId);
            _elements.Add(item);
            item.SvgParent.OnElementAdded(item, Count - 1);
        }

        private void AddToIdManager(SvgElement item, SvgElement sibling, bool autoForceUniqueId = true, bool autoFixChildrenId = true, Action<SvgElement, string, string> logElementOldIdNewId = null)
        {
            if (!_mock)
            {
            	if (_owner.OwnerDocument != null)
            	{
                    _owner.OwnerDocument.IdManager.AddAndForceUniqueId(item, sibling, autoForceUniqueId, logElementOldIdNewId);

                    if (!(item is SvgDocument)) //don't add subtree of a document to parent document
                    {
                        foreach (var child in item.Children)
                        {
                            child.ApplyRecursive(e => _owner.OwnerDocument.IdManager.AddAndForceUniqueId(e, null, autoFixChildrenId, logElementOldIdNewId));
                        }
                    }
                }
                
                //if all checked, set parent
                item.SvgParent = _owner;
            }
        }

        public void Clear()
        {
            while (Count > 0)
            {
                SvgElement element = this[0];
                Remove(element);
            }
        }

        public bool Contains(SvgElement item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(SvgElement[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public bool Remove(SvgElement item)
        {
            var removed = _elements.Remove(item);

            if (!removed) return false;
            _owner.OnElementRemoved(item);

            if (_mock) return true;
            if (item == null) return true;
            item.SvgParent = null;

            if (_owner.OwnerDocument != null)
            {
                item.ApplyRecursiveDepthFirst(_owner.OwnerDocument.IdManager.Remove);
            }

            return true;
        }

		/// <summary>
		/// expensive recursive search for nodes of type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<T> FindSvgElementsOf<T>() where T : SvgElement
		{
			return _elements.OfType<T>().Concat(_elements.SelectMany(x => x.Children.FindSvgElementsOf<T>()));
		}

		/// <summary>
		/// expensive recursive search for first node of type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T FindSvgElementOf<T>() where T : SvgElement
		{
			return _elements.OfType<T>().FirstOrDefault() ?? _elements.Select(x => x.Children.FindSvgElementOf<T>()).FirstOrDefault(x => x != null);
		}

		public T GetSvgElementOf<T>() where T : SvgElement
		{
			return _elements.FirstOrDefault(x => x is T) as T;
		}


        public IEnumerator<SvgElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}