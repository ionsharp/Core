using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Imagin.Common.Controls
{
    public class MemberRoute : ObservableCollection<MemberRouteElement>
    {
        public ObservableCollection<MemberCrumb> Crumbs { get; private set; } = new();

        //...

        internal MemberRoute() : base() { }

        //...

        internal MemberRouteData Clean(object oldValue, object newValue)
        {
            if (newValue == null)
                return null;

            if (ReferenceEquals(oldValue, newValue))
                return new(null);

            if (newValue is ListItemModel listItem)
                newValue = new MemberRouteItem(listItem);

            else if (newValue is MemberModel member)
                newValue = new MemberRouteChild(member);

            if (newValue is MemberRouteElement result)
            {
                if (result is MemberRouteChild child)
                {
                    //This should theoretically never occur!
                    if (result.Value == oldValue)
                        return new(null);

                    return new(child);
                }
                //Safely assume this already exists and start from scratch
                else if (result is MemberRouteSource source)
                    return new(source);

                throw new NotSupportedException();
            }
            //Safely assume we are starting from scratch
            else return new(new MemberRouteSource(newValue));
        }

        internal void Append(MemberRouteElement input)
        {
            if (input == null || input is MemberRouteSource)
                Clear();

            var append = new List<MemberRouteElement>();
            if (input is MemberRouteChild child)
            {
                //If it already exists in the route, we must navigate to it!
                if (Contains(input))
                {
                    var index = IndexOf(input);
                    //Remove every child (including this one)
                    for (var i = Count - 1; i >= index; i--)
                        RemoveAt(i);

                    //The child gets added back at the end
                }
                //If it doesn't yet exist, we need to add it, but first, check if anything should come in between!
                //This happens when something deep in the hierarchy is edited directly. This only applies because part of
                //the hierarchy already exists in the root. Doing this allows all of the hierarchy to be edited from the route
                //regardless of what part is directly edited.
                //
                //Normal hierarchy looks like this:
                //(root) MemberCollection -> MemberModel -> (child) MemberCollection -> MemberModel -> (child) MemberCollection ...
                //
                //Hierarchy with a list member looks like this:
                //(root) MemberCollection -> MemberModel -> (child) MemberCollection -> ListMemberModel -> ListItemModel -> (child) MemberCollection ...
                //
                //The goal is to enumerate the hierarchy backwards and stop when we reach the root.
                //
                //This needs tested...
                else
                {
                    goto skip;
                    MemberModel parentMember = child.Member;
                    while (parentMember != null)
                    {
                        if (parentMember is ListItemModel listItem)
                        {
                            parentMember = listItem.Parent;
                        }
                        else if (child.Member.Collection.ParentMember == null)
                        {
                            break;
                        }
                        else
                        {
                            parentMember = child.Member.Collection.ParentMember;
                        }

                        if (!this.Contains<MemberRouteChild>(i => ReferenceEquals(i.Member, parentMember)))
                        {
                            MemberRouteChild result = null;
                            if (parentMember is ListItemModel)
                                result = new MemberRouteItem(parentMember);

                            else result = new MemberRouteChild(parentMember);
                            append.Insert(0, result);
                        }
                        else break;
                    }
                }
            }
            skip: append.Add(input);
            append.ForEach(i => Add(i));
        }

        //...

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (Count > 1)
                        Crumbs.Add(new MemberCrumbSeparator());

                    if (e.NewItems[0] is MemberRouteChild child)
                    {
                        if (child is MemberRouteItem item)
                            Crumbs.Add(new MemberCrumbItem(item));

                        else Crumbs.Add(new MemberCrumbChild(child));
                    }

                    else if (e.NewItems[0] is MemberRouteSource source)
                        Crumbs.Add(new MemberCrumbSource(source));

                    break;

                case NotifyCollectionChangedAction.Remove:
                    Crumbs.RemoveLast();
                    if (Crumbs.Count > 0)
                        Crumbs.RemoveLast();

                    break;
            }
        }
    }
}