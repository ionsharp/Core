using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class VirtualFolder : AbstractContainer, ICloneable, IContainer<AbstractContainer>, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        string name = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns></returns>
        public IEnumerable<AbstractContainer> GetClones(VirtualFolder Folder)
        {
            foreach (var i in Folder.Items)
                yield return (AbstractContainer)i.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var Result = new VirtualFolder(Name);
            GetClones(this).ForEach(i => Result.Items.Add(i));
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public VirtualFolder() : this(string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public VirtualFolder(string name) : base()
        {
            Name = name;
            Items = new ConcurrentCollection<AbstractContainer>();
        }
    }
}
