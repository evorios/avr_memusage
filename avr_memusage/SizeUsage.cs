using System;
using Aga.Controls.Tree;

namespace avr_memusage
{
    public enum SizeType { Text, Data, BSS };

    /// <summary>
    /// Inherits the node class to show how the class can be extended.
    /// </summary>
	public class SizeUsage : Node
	{
	    /// <exception cref="ArgumentNullException">Argument is null.</exception>
	    public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException();

				base.Text = value;
			}
		}

        private SizeType _type;
        public SizeType Type { get { return _type; } }

        private int _size;
        public int Size { get { return _size; } set { _size = value; } }

        /// <summary>
        /// Initializes a new SizeUsage class with a given Text property.
        /// </summary>
        /// <param name="text">String to set the text property with.</param>
        public SizeUsage(SizeType sizeType, int size, string name)
            : base(name)
		{
            _type = sizeType;
            _size = size;
		}
    
        public SizeUsage(string name)
            : base(name)
        {
        }
    }
}
