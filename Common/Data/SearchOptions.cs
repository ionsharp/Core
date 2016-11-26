using Imagin.Common.Data.Collections;
using Imagin.Common.Web;
using System;
using System.IO;
using System.Linq;

namespace Imagin.Common.Data
{
    [Serializable]
    public class SearchOptions : AbstractObject
    {
        MatchType matchType = MatchType.All;
        public MatchType MatchType
        {
            get
            {
                return this.matchType;
            }
            set
            {
                this.matchType = value;
                OnPropertyChanged("MatchType");
            }
        }

        ConditionCollection conditions = new ConditionCollection();
        public ConditionCollection Conditions
        {
            get
            {
                return this.conditions;
            }
            set
            {
                this.conditions = value;
                OnPropertyChanged("Conditions");
            }
        }

        /// <summary>
        /// Evaluates given conditions and returns true if all conditions evaluate to true.
        /// </summary>
        public bool Evaluate(FileSystemInfo Info)
        {
            if (this.Conditions.Count() == 0)
                return true;

            if (Info == null)
                return false;

            foreach (var c in this.Conditions)
            {
                bool IsMatched = false;
                if (c == null)
                    continue;
                switch (c.Property)
                {
                    case ServerObjectProperty.Name:
                        IsMatched = c.Evaluate(Info.Name);
                        break;
                    case ServerObjectProperty.Path:
                        IsMatched = c.Evaluate(Info.FullName);
                        break;
                    case ServerObjectProperty.Extension:
                        IsMatched = c.Evaluate(Info.Extension);
                        break;
                    case ServerObjectProperty.Size:
                        if (Info is FileInfo)
                            IsMatched = c.Evaluate((Info as System.IO.FileInfo).Length);
                        break;
                    case ServerObjectProperty.Accessed:
                        IsMatched = c.Evaluate(Info.LastAccessTime);
                        break;
                    case ServerObjectProperty.Created:
                        IsMatched = c.Evaluate(Info.CreationTime);
                        break;
                    case ServerObjectProperty.Modified:
                        IsMatched = c.Evaluate(Info.LastWriteTime);
                        break;
                }
                if (IsMatched)
                {
                    if (this.MatchType == MatchType.Any)
                        return true;
                    else if (this.MatchType == MatchType.None)
                        return false;
                }
                else if (this.MatchType == MatchType.All)
                    return false;
            }
            if (this.MatchType == MatchType.Any)
                return false;
            else if (this.MatchType == MatchType.None)
                return true;
            else if (this.MatchType == MatchType.All)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Evaluates given conditions and returns true if all conditions evaluate to true.
        /// </summary>
        public bool Evaluate(string Name, string FullName, string Extension, long Size, DateTime Accessed, DateTime Created, DateTime Modified)
        {
            if (this.Conditions.Count() == 0)
                return true;
            foreach (Condition c in this.Conditions)
            {
                bool IsMatched = false;
                if (c == null)
                    continue;
                switch (c.Property)
                {
                    case ServerObjectProperty.Name:
                        IsMatched = c.Evaluate(Name);
                        break;
                    case ServerObjectProperty.Path:
                        IsMatched = c.Evaluate(FullName);
                        break;
                    case ServerObjectProperty.Extension:
                        IsMatched = c.Evaluate(Extension);
                        break;
                    case ServerObjectProperty.Size:
                        IsMatched = c.Evaluate(Size);
                        break;
                    case ServerObjectProperty.Accessed:
                        IsMatched = c.Evaluate(Accessed);
                        break;
                    case ServerObjectProperty.Created:
                        IsMatched = c.Evaluate(Created);
                        break;
                    case ServerObjectProperty.Modified:
                        IsMatched = c.Evaluate(Modified);
                        break;
                }
                if (IsMatched)
                {
                    if (this.MatchType == MatchType.Any)
                        return true;
                    else if (this.MatchType == MatchType.None)
                        return false;
                }
                else if (this.MatchType == MatchType.All)
                    return false;
            }
            if (this.MatchType == MatchType.Any)
                return false;
            else if (this.MatchType == MatchType.None)
                return true;
            else if (this.MatchType == MatchType.All)
                return true;
            else
                return false;
        }

        public SearchOptions()
        {
            this.MatchType = MatchType;
            this.Conditions = new ConditionCollection();
        }

        public SearchOptions(MatchType MatchType)
        {
            this.MatchType = MatchType;
            this.Conditions = new ConditionCollection();
        }

        public SearchOptions(MatchType MatchType, ConditionCollection Conditions)
        {
            this.MatchType = MatchType;
            this.Conditions = Conditions;
        }
    }
}
