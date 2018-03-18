using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DialogButton
    {
        /// <summary>
        /// 
        /// </summary>
        public Action Action
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCancel
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Label
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="id"></param>
        /// <param name="isDefault"></param>
        /// <param name="isCancel"></param>
        public DialogButton(string label, int id, bool isDefault = false, bool isCancel = false)
        {
            Label = label;
            Id = id;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="isDefault"></param>
        /// <param name="isCancel"></param>
        public DialogButton(string label, int id, Action action, bool isDefault = false, bool isCancel = false)
        {
            Label = label;
            Id = id;
            Action = action;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }
    }
}
